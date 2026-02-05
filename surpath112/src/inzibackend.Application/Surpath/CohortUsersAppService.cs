using inzibackend.Surpath;
using inzibackend.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using inzibackend.Surpath.Exporting;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Abp.Application.Services.Dto;
using inzibackend.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using inzibackend.Storage;
using inzibackend.Authorization.Users.Dto;
using Abp.Domain.Uow;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using inzibackend.Surpath.Compliance;
using Castle.Core.Logging;
using inzibackend.Surpath.ComplianceManager;
using Abp.Application.Features;
using inzibackend.Features;
using Abp.MultiTenancy;
using inzibackend.Authorization.Organizations;

namespace inzibackend.Surpath
{
    //[AbpAuthorize(AppPermissions.Pages_CohortUser)]
    [AbpAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
    public class CohortUsersAppService : inzibackendAppServiceBase, ICohortUsersAppService
    {
        //public ILogger Logger { get; set; } = NullLogger.Instance;

        //public TenantManager TenantManager { get; set; }
        private readonly IRepository<Tenant> _tenantRepository;

        private readonly IFeatureChecker _featureChecker;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly ICohortUsersExcelExporter _cohortUsersExcelExporter;
        private readonly ICohortUsersBGCExcelExporter _cohortUsersBGCExcelExporter;
        private readonly IRepository<Cohort, Guid> _cohortLookUpRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly SurpathManager _surpathManager;

        //private readonly ISurpathComplianceAppService _surpathComplianceAppService;
        //private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;

        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;

        private readonly IRepository<RecordStatus, Guid> _recordStatusLookUpRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;

        private readonly IRepository<UserPid, Guid> _userPidRepository;
        private readonly IRepository<PidType, Guid> _pidTypeLookUpRepository;

        private readonly IOUSecurityManager _ouSecurityManager;

        public CohortUsersAppService(
            IOUSecurityManager ouSecurityManager,
            IRepository<CohortUser, Guid> cohortUserRepository,
            ICohortUsersExcelExporter cohortUsersExcelExporter,
            ICohortUsersBGCExcelExporter cohortUsersBGCExcelExporter,
            IRepository<Cohort, Guid> lookup_cohortRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository,
            SurpathManager surpathManager,
            IRepository<Tenant> tenantRepository,
            ILogger _logger,
            ISurpathComplianceEvaluator surpathComplianceEvaluator,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<RecordCategory, Guid> recordCategoryRepository,
            IRepository<RecordCategoryRule, Guid> recordCategoryRuleRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IRepository<RecordStatus, Guid> lookup_recordStatusRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IFeatureChecker featureChecker,
            IRepository<UserPid, Guid> userPidRepository,
            IRepository<PidType, Guid> lookup_pidTypeRepository
            //,IRepository<RecordState, Guid> recordStateRepository
            //,ISurpathComplianceAppService surpathComplianceAppService
            )
        {
            _userPidRepository = userPidRepository;
            _pidTypeLookUpRepository = lookup_pidTypeRepository;
            _cohortUserRepository = cohortUserRepository;
            _cohortUsersExcelExporter = cohortUsersExcelExporter;
            _cohortUsersBGCExcelExporter = cohortUsersBGCExcelExporter;
            _cohortLookUpRepository = lookup_cohortRepository;
            _userLookUpRepository = lookup_userRepository;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;
            _surpathManager = surpathManager;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
            //_recordStateRepository = recordStateRepository;
            //_surpathComplianceAppService = surpathComplianceAppService;
            _tenantRepository = tenantRepository;
            _featureChecker = featureChecker;
            _recordRequirementRepository = recordRequirementRepository;
            _recordCategoryRepository = recordCategoryRepository;
            _recordCategoryRuleRepository = recordCategoryRuleRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _recordStatusLookUpRepository = lookup_recordStatusRepository;
            _recordStateRepository = recordStateRepository;
            Logger = _logger;
            _ouSecurityManager = ouSecurityManager;
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers)]
        public async Task<PagedResultDto<GetCohortUserForViewDto>> GetAll(GetAllCohortUsersInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var tenantList = new List<TenantEditDto>();
            if (AbpSession.TenantId == null)
            {
                var tDict = await TenantManager.GetTenancyNames();

                foreach (var t in tDict)
                {
                    var tDto = new TenantEditDto
                    {
                        Id = t.Key, // Assuming the key is the Id
                        TenancyName = t.Value // Assuming the value is the TenancyName
                    };
                    tenantList.Add(tDto);
                }
            }

            var filteredCohortUsers = _cohortUserRepository.GetAll()
                    .Include(e => e.CohortFk)
                    .Include(e => e.UserFk)
                    .Where(e => e.UserFk.IsDeleted == false)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.UserFk != null && e.UserFk.Name.Contains(input.Filter) || e.UserFk.Surname.Contains(input.Filter) || e.UserFk.EmailAddress.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CohortDescriptionFilter), e => e.CohortFk != null && e.CohortFk.Description == input.CohortDescriptionFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.Contains(input.UserNameFilter) || e.UserFk.Surname.Contains(input.UserNameFilter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CohortIdFilter.ToString()), e => e.CohortId != null && e.CohortId == input.CohortIdFilter);

            filteredCohortUsers = _ouSecurityManager.ApplyTenantDepartmentCohortUserVisibilityFilter(filteredCohortUsers, AbpSession.UserId.Value);

            var pagedAndFilteredCohortUsers = filteredCohortUsers
                .OrderBy(input.Sorting ?? "userFk.surname asc")
                .PageBy(input);
            //var m = input.MaxResultCount

            var cohortUsers = from o in pagedAndFilteredCohortUsers
                              join o1 in _cohortLookUpRepository.GetAll().Where(c => c.IsDeleted == false) on o.CohortId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _userLookUpRepository.GetAll().Where(u => u.IsDeleted == false) on o.UserId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {
                                  Id = o.Id,
                                  CohortDescription = s1 == null ? "" : string.IsNullOrEmpty(s1.Description) ? "" : s1.Description.ToString(),
                                  CohortName = s1 == null ? "" : string.IsNullOrEmpty(s1.Name) ? "" : s1.Name.ToString(),
                                  UserId = s2 == null ? 0 : s2.Id,
                                  CohortId = s1 == null ? Guid.Empty : s1.Id,
                                  UserName = s2 == null ? "" : string.IsNullOrEmpty(s2.Name) ? "" : s2.Name.ToString(),
                                  UserEmail = s2 == null ? "" : string.IsNullOrEmpty(s2.EmailAddress) ? "" : s2.EmailAddress.ToString(),
                                  User = s2 == null ? null : s2,
                                  TenantId = o.TenantId
                              };

            var totalCount = await filteredCohortUsers.CountAsync();

            var dbList = await cohortUsers.ToListAsync();
            var results = new List<GetCohortUserForViewDto>();

            //// Collect all user IDs and create initial result objects
            //var userIds = dbList.Select(o => o.UserId).ToList();
            //var bulkComplianceValues = await _surpathComplianceEvaluator.GetBulkComplianceValuesForUsers(userIds, (int)AbpSession.TenantId);

            foreach (var o in dbList)
            {
                var res = new GetCohortUserForViewDto()
                {
                    CohortUser = new CohortUserDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        CohortId = o.CohortId,
                        TenantId = o.TenantId
                    },
                    UserEditDto = new UserEditDto
                    {
                        Id = o.User.Id,
                        EmailAddress = o.User.EmailAddress,
                        UserName = o.User.UserName,
                        PhoneNumber = o.User.PhoneNumber,
                        Name = o.User.Name,
                        Middlename = o.User.MiddleName,
                        Surname = o.User.Surname,
                        Address = o.User.Address,
                        SuiteApt = o.User.SuiteApt,
                        City = o.User.City,
                        State = o.User.State,
                        Zip = o.User.Zip,
                        DateOfBirth = o.User.DateOfBirth,
                    },
                    CohortDescription = o.CohortDescription,
                    CohortName = o.CohortName,
                    UserName = o.UserName
                };

                // Get compliance values from bulk results
                // res.ComplianceValues = bulkComplianceValues.ContainsKey(o.UserId) ? bulkComplianceValues[o.UserId] : null;
                res.ComplianceValues = await _surpathComplianceEvaluator.GetDetailedComplianceValuesForUser(o.UserId); // GetUserComplianceValues((int)o.TenantId, o.UserId);

                if (tenantList.Select(t => t.Id).ToList().Contains((int)res.CohortUser.TenantId))
                {
                    res.TenantEditDto = tenantList.Where(t => t.Id == res.CohortUser.TenantId).First();
                }
                else
                {
                    res.TenantEditDto = new TenantEditDto() { Id = (int)res.CohortUser.TenantId, TenancyName = "" };
                }

                results.Add(res);
            }

            return new PagedResultDto<GetCohortUserForViewDto>(
                totalCount,
                results
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers)]
        public async Task<PagedResultDto<GetCohortUserForViewDto>> GetPrevNext(GetAllCohortUsersInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredCohortUsers = _cohortUserRepository.GetAll()
                    .Include(e => e.CohortFk)
                    .Include(e => e.UserFk)
                    .Where(e => e.UserFk.IsDeleted == false)
                    //.WhereIf(AbpSession.TenantId != null, e=>e.TenantId==AbpSession.TenantId)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CohortIdFilter.ToString()), e => e.CohortId != null && e.CohortId == input.CohortIdFilter);

            var pagedAndFilteredCohortUsers = filteredCohortUsers
                .OrderBy("userFk.surname asc");
            //.PageBy(input);

            var cohortUsers = from o in pagedAndFilteredCohortUsers

                              select new
                              {
                                  Id = o.Id,
                                  CohortDescription = "", //s1 == null ? "" : string.IsNullOrEmpty(s1.Description) ? "" : s1.Description.ToString(),
                                  CohortName = "", // s1 == null ? "" : string.IsNullOrEmpty(s1.Name) ? "" : s1.Name.ToString(),
                                  UserId = o.UserFk == null ? 0 : o.UserFk.Id,
                                  CohortId = Guid.Empty, // s1 == null ? Guid.Empty : s1.Id,
                                  UserName = o.UserFk == null ? "" : string.IsNullOrEmpty(o.UserFk.Name) ? "" : o.UserFk.Name.ToString(),
                                  UserEmail = o.UserFk == null ? "" : string.IsNullOrEmpty(o.UserFk.EmailAddress) ? "" : o.UserFk.EmailAddress.ToString(),
                                  User = o.UserFk == null ? null : o.UserFk,
                                  TenantId = o.TenantId
                              };

            var totalCount = 2; // await filteredCohortUsers.CountAsync();

            var dbList = await cohortUsers.ToListAsync();

            var tenantcount = dbList.Select(d => d.TenantId).Distinct().Count();
            Logger.Debug($"DEBUG Prev Next {tenantcount}");

            var _userIdx = dbList.IndexOf(dbList.Where(c => c.Id == input.Id).First());
            var _prev = new GetCohortUserForViewDto();
            var _next = new GetCohortUserForViewDto();
            if (_userIdx > 0)
            {
                var o = dbList[_userIdx - 1];
                _prev = new GetCohortUserForViewDto()
                {
                    CohortUser = new CohortUserDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        CohortId = o.CohortId,
                        TenantId = o.TenantId
                    },
                    UserEditDto = new UserEditDto
                    {
                        Id = o.User.Id,
                        EmailAddress = o.User.EmailAddress,
                        UserName = o.User.UserName,
                        PhoneNumber = o.User.PhoneNumber,
                        Name = o.User.Name,
                        Middlename = o.User.MiddleName,
                        Surname = o.User.Surname,
                        Address = o.User.Address,
                        SuiteApt = o.User.SuiteApt,
                        City = o.User.City,
                        State = o.User.State,
                        Zip = o.User.Zip,
                        DateOfBirth = o.User.DateOfBirth,
                    },
                    CohortDescription = o.CohortDescription,
                    CohortName = o.CohortName,
                    UserName = o.UserName
                };
            }

            if (_userIdx < dbList.Count() - 1)
            {
                var o = dbList[_userIdx + 1];
                _next = new GetCohortUserForViewDto()
                {
                    CohortUser = new CohortUserDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        CohortId = o.CohortId,
                        TenantId = o.TenantId
                    },
                    UserEditDto = new UserEditDto
                    {
                        Id = o.User.Id,
                        EmailAddress = o.User.EmailAddress,
                        UserName = o.User.UserName,
                        PhoneNumber = o.User.PhoneNumber,
                        Name = o.User.Name,
                        Middlename = o.User.MiddleName,
                        Surname = o.User.Surname,
                        Address = o.User.Address,
                        SuiteApt = o.User.SuiteApt,
                        City = o.User.City,
                        State = o.User.State,
                        Zip = o.User.Zip,
                        DateOfBirth = o.User.DateOfBirth,
                    },
                    CohortDescription = o.CohortDescription,
                    CohortName = o.CohortName,
                    UserName = o.UserName
                };
            }

            var results = new List<GetCohortUserForViewDto>();
            results.Add(_prev);
            results.Add(_next);

            return new PagedResultDto<GetCohortUserForViewDto>(
                totalCount,
                results
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        public async Task<GetCohortUserForViewDto> GetCohortUserForView(Guid? _id)
        {
            if (!_id.HasValue || _id == Guid.Empty)
            {
                Logger.Warn("GetCohortUserForView no id provided, getting for current user");
                // Get the cohortuser for this account
                //var _cohortUser = _cohortUserRepository.
                _id = await GetCohortUserIdForUser();
                Logger.Warn($"_id is null? {_id == null}");
                if (_id == null || _id == Guid.Empty)
                {
                    //_id = await _surpathManager.AddUserToDefaultCohort((long)AbpSession.UserId, (int)AbpSession.TenantId);
                    // We need to return an empty user and make them select user
                    Logger.Warn("User is not a member of a cohort");
                    return new GetCohortUserForViewDto()
                    {
                        CohortUser = new CohortUserDto(),
                        UserEditDto = new UserEditDto(),
                    };
                }
            }
            var id = (Guid)_id;
            Logger.Warn($"GetCohortUserForView, getting for user id: {id}");

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cohortUser = await _cohortUserRepository.GetAsync(id);
            await AuthorizedForSelf(cohortUser.UserId);

            var output = new GetCohortUserForViewDto { CohortUser = ObjectMapper.Map<CohortUserDto>(cohortUser) };
            if (output.CohortUser.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.CohortUser.CohortId);
                output.CohortDescription = _lookupCohort?.Description?.ToString();
                if (_lookupCohort != null)
                {
                    if (_lookupCohort.TenantDepartmentId != null)
                    {
                        var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)_lookupCohort.TenantDepartmentId);
                        output.TenantDepartmentDto = ObjectMapper.Map<TenantDepartmentDto>(_lookupTenantDepartment);
                    }
                    else
                    {
                        output.TenantDepartmentDto = new TenantDepartmentDto()
                        {
                            Description = L("None"),
                            Id = Guid.Empty,
                            Active = false,
                            Name = ""
                        };
                    }
                }
            }
            //if (output.CohortUser.UserId != null)
            try
            {
                if (output.CohortUser.UserId > 0)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.CohortUser.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                    output.UserEditDto = ObjectMapper.Map<UserEditDto>(_lookupUser);
                    output.UserEditDto.Password = "";
                    //var _lookupTenant = await TenantManager.GetByIdAsync((int)_lookupUser.TenantId);
                    var _lookupTenant = _tenantRepository.GetAll().IgnoreQueryFilters().Where(t => t.Id == _lookupUser.TenantId).FirstOrDefault();
                    if (_lookupTenant != null)
                        output.TenantEditDto = ObjectMapper.Map<TenantEditDto>(_lookupTenant);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting user for cohort user. Id: {output.CohortUser.UserId}");
                Logger.Error(ex.Message, ex);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit, AppPermissions.Pages_CohortUser_Edit)]
        public async Task<GetCohortUserForEditOutput> GetCohortUserForEdit(EntityDto<Guid> input)
        {
            var cohortUser = await _cohortUserRepository.FirstOrDefaultAsync(input.Id);

            await AuthorizedForSelf(cohortUser.UserId);

            var output = new GetCohortUserForEditOutput { CohortUser = ObjectMapper.Map<CreateOrEditCohortUserDto>(cohortUser) };

            if (output.CohortUser.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.CohortUser.CohortId);
                output.CohortDescription = _lookupCohort?.Description?.ToString();
            }

            if (output.CohortUser.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.CohortUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit, AppPermissions.Pages_CohortUser_Edit)]
        public async Task CreateOrEdit(CreateOrEditCohortUserDto input)
        {
            // check if user is already in a cohort, in which case we will edit

            var _cohortUser = await _cohortUserRepository.FirstOrDefaultAsync(c => c.UserId == input.UserId && c.CohortId == input.CohortId);
            if (_cohortUser != null)
            {
                input.Id = _cohortUser.Id;
            }

            // assign to the cohort
            input.CohortId = input.CohortId;

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                if (!PermissionChecker.IsGranted(AppPermissions.Pages_CohortUsers_Create))
                    throw new AbpAuthorizationException("You are not authorized to do this!");
                await Update(input);
                // remove from other cohorts for this user id -
                // this can likely be removed in future versions, initial launch had bug that allowed adding to multiple cohorts
                await _cohortUserRepository.GetAll().Where(c => c.UserId == input.UserId && c.Id != input.Id).DeleteFromQueryAsync();
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit)]
        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Create)]
        protected virtual async Task Create(CreateOrEditCohortUserDto input)
        {
            var cohortUser = ObjectMapper.Map<CohortUser>(input);

            if (AbpSession.TenantId != null)
            {
                cohortUser.TenantId = (int?)AbpSession.TenantId;
            }
            if (!_cohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.UserId == input.UserId && cu.CohortId == input.CohortId).Any())
            {
                await _cohortUserRepository.InsertAsync(cohortUser);
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit)]
        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit, AppPermissions.Pages_CohortUser_Edit)]
        protected virtual async Task Update(CreateOrEditCohortUserDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cohortUser = await _cohortUserRepository.FirstOrDefaultAsync((Guid)input.Id);
            await AuthorizedForSelf(cohortUser.UserId);
            ObjectMapper.Map(input, cohortUser);
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _cohortUserRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit)]
        public async Task<FileDto> GetCohortUsersToExcel(GetAllCohortUsersForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var validguid = Guid.TryParse(input.CohortId, out var cohortId);
            var filteredCohortUsers = _cohortUserRepository.GetAll()
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(validguid, e => e.CohortId == cohortId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.UserFk != null && e.UserFk.Name.Contains(input.Filter) || e.UserFk.Surname.Contains(input.Filter) || e.UserFk.EmailAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortDescriptionFilter), e => e.CohortFk != null && e.CohortFk.Description == input.CohortDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredCohortUsers
                         join o1 in _cohortLookUpRepository.GetAll() on o.CohortId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _userLookUpRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCohortUserForViewDto()
                         {
                             UserEditDto = new UserEditDto()
                             {
                                 Name = s2.Name.ToString(),
                                 Surname = s2.Surname.ToString()
                             },
                             CohortUser = new CohortUserDto
                             {
                                 Id = o.Id,
                                 UserId = o.UserId
                             },
                             TenantEditDto = new TenantEditDto
                             {
                                 Id = (int)o.TenantId
                             },
                             CohortDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var CohortUserComplianceExcelList = new List<CohortUserComplianceExcel>();
            var cohortUserListDtos = await query.ToListAsync();

            var _PidViewAccess = await PermissionChecker.IsGrantedAsync(AppPermissions.Surpath_Administration_View_Masked_Pids);

            var _tenantId = 0;
            if (cohortUserListDtos.Count > 0)
            {
                _tenantId = cohortUserListDtos.First().TenantEditDto.Id;
            }

            // Get all user IDs and fetch compliance values in bulk
            var userIds = cohortUserListDtos.Select(u => u.CohortUser.UserId).ToList();
            var bulkComplianceValues = await _surpathComplianceEvaluator.GetBulkComplianceValuesForUsers(userIds, _tenantId);

            foreach (var culd in cohortUserListDtos)
            {
                // Get compliance values from bulk results
                culd.ComplianceValues = bulkComplianceValues.ContainsKey(culd.CohortUser.UserId) ?
                    bulkComplianceValues[culd.CohortUser.UserId] : null;

                var _cohortusercomplianceexcelitem = new CohortUserComplianceExcel()
                {
                    CohortName = culd.CohortDescription,
                    FirstName = culd.UserEditDto.Name,
                    LastName = culd.UserEditDto.Surname,
                    Drug = culd.ComplianceValues?.Drug.ToString() ?? "Not Compliant",
                    Background = culd.ComplianceValues?.Background.ToString() ?? "Not Compliant",
                    Immunization = culd.ComplianceValues?.Immunization.ToString() ?? "Not Compliant",
                    OverallCompliance = culd.ComplianceValues?.InCompliance == true ? "Compliant" : "Not Compliant",
                    DrugStatusColor = GetStatusColorForCompliance(culd.ComplianceValues?.Drug == true),
                    BackgroundStatusColor = GetStatusColorForCompliance(culd.ComplianceValues?.Background == true),
                    ImmunizationStatusColor = GetStatusColorForCompliance(culd.ComplianceValues?.Immunization == true),
                    OverallStatusColor = GetStatusColorForCompliance(culd.ComplianceValues?.InCompliance == true)
                };

                if (_PidViewAccess)
                {
                    _cohortusercomplianceexcelitem.UserPids = _userPidRepository.GetAll()
                        .IgnoreQueryFilters()
                        .Where(p => p.IsDeleted == false && p.UserId == culd.CohortUser.UserId)
                        .Include(p => p.PidTypeFk)
                        .ToList();
                }
                CohortUserComplianceExcelList.Add(_cohortusercomplianceexcelitem);
            }
            ;

            // If no specific cohort is provided (validguid is false), generate multi-sheet report
            if (!validguid)
            {
                return _cohortUsersExcelExporter.ExportToFileFromObjectMultiSheet(CohortUserComplianceExcelList);
            }

            return _cohortUsersExcelExporter.ExportToFileFromObject(CohortUserComplianceExcelList);
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers_Edit)]
        public async Task<FileDto> GetCohortUsersImmunizationReportToExcel(GetAllCohortUsersForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var validguid = Guid.TryParse(input.CohortId, out var cohortId);
            var filteredCohortUsers = _cohortUserRepository.GetAll()
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(validguid, e => e.CohortId == cohortId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortDescriptionFilter), e => e.CohortFk != null && e.CohortFk.Description == input.CohortDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var cohortUsers = await filteredCohortUsers.ToListAsync();
            var cohortImmunizationData = new List<CohortImmunizationReportDto>();

            foreach (var cohortUser in cohortUsers)
            {
                var memberData = new CohortImmunizationReportDto
                {
                    CohortName = cohortUser.CohortFk?.Name ?? "",
                    FirstName = cohortUser.UserFk?.Name ?? "",
                    LastName = cohortUser.UserFk?.Surname ?? ""
                };

                // Get immunization requirements for this user's department/cohort
                var immunizationRequirements = await _surpathManager.GetRequirementListFor(cohortUser.CohortId ?? Guid.Empty, cohortUser.CohortFk?.TenantDepartmentId ?? Guid.Empty);

                foreach (var requirement in immunizationRequirements)
                {
                    var categories = await _surpathManager.GetRecordCategoriesForRequirementAsync(requirement.Id);

                    foreach (var category in categories)
                    {
                        // Get the user's record states for this category by querying the database directly
                        var recordStates = await _recordStateRepository.GetAll()
                            .Include(rs => rs.RecordFk)
                            .Include(rs => rs.RecordStatusFk)
                            .Where(rs => rs.UserId == cohortUser.UserId && rs.RecordCategoryId == category.Id && !rs.IsDeleted)
                            .OrderByDescending(rs => rs.CreationTime)
                            .ToListAsync();

                        var latestRecordState = recordStates.FirstOrDefault();

                        var immunizationReq = new ImmunizationRequirementDto
                        {
                            RequirementName = requirement.Name,
                            CategoryName = category.Name,
                            ComplianceStatus = "Not Compliant",
                            AdministeredDate = null,
                            ExpirationDate = null
                        };

                        if (latestRecordState != null)
                        {
                            // Determine compliance status based on record status
                            immunizationReq.ComplianceStatus = latestRecordState.RecordStatusFk?.ComplianceImpact == EnumStatusComplianceImpact.Compliant ? "Compliant" : "Not Compliant";
                            immunizationReq.StatusColor = latestRecordState.RecordStatusFk?.HtmlColor ?? "#67c777"; // Default green for compliant

                            // Get administered date (effective date) and expiration date from the record
                            if (latestRecordState.RecordFk != null)
                            {
                                immunizationReq.AdministeredDate = latestRecordState.RecordFk.EffectiveDate;
                                immunizationReq.ExpirationDate = latestRecordState.RecordFk.ExpirationDate;
                            }
                        }

                        memberData.ImmunizationRequirements.Add(immunizationReq);
                    }
                }

                cohortImmunizationData.Add(memberData);
            }

            // If no specific cohort is provided (validguid is false), generate multi-sheet report
            if (!validguid)
            {
                // Determine if we have multiple cohorts and should use multi-sheet format
                var uniqueCohorts = cohortImmunizationData.Select(c => c.CohortName).Distinct().Count();
                if (uniqueCohorts > 1)
                {
                    return _cohortUsersExcelExporter.ExportImmunizationReportToFileMultiSheet(cohortImmunizationData);
                }
            }

            return _cohortUsersExcelExporter.ExportImmunizationReportToFile(cohortImmunizationData);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_BGCExport)]
        public async Task<FileDto> GetCohortUsersToBGCExcel(GetAllCohortUsersForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var validguid = Guid.TryParse(input.CohortId, out var cohortId);
            var filteredCohortUsers = _cohortUserRepository.GetAll()
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(validguid, e => e.CohortId == cohortId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.UserFk != null && e.UserFk.Name.Contains(input.Filter) || e.UserFk.Surname.Contains(input.Filter) || e.UserFk.EmailAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortDescriptionFilter), e => e.CohortFk != null && e.CohortFk.Description == input.CohortDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredCohortUsers
                         join o1 in _cohortLookUpRepository.GetAll() on o.CohortId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _userLookUpRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCohortUserForBGCExportDto()
                         {
                             UserEditDto = new UserEditDto()
                             {
                                 Name = s2.Name ?? "",
                                 Surname = s2.Surname ?? "",
                                 EmailAddress = s2.EmailAddress ?? "",
                                 PhoneNumber = s2.PhoneNumber ?? "",
                                 Middlename = s2.MiddleName ?? "",
                                 Address = s2.Address ?? "",
                                 SuiteApt = s2.SuiteApt ?? "",
                                 City = s2.City ?? "",
                                 State = s2.State ?? "",
                                 Zip = s2.Zip ?? "",
                                 DateOfBirth = s2.DateOfBirth
                             },
                             CohortUser = new CohortUserDto
                             {
                                 Id = o.Id,
                                 UserId = o.UserId
                             },
                             CohortDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             CohortName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var cohortUserListDtos = await query.ToListAsync();

            // Get UserPids for all users
            var userIds = cohortUserListDtos.Select(c => c.CohortUser.UserId).ToList();
            var userPids = await _userPidRepository.GetAll().AsNoTracking()
                .Include(p => p.PidTypeFk)
                .Where(p => userIds.Contains((long)p.UserId) && !p.IsDeleted)
                .ToListAsync();

            var userPidDTOs = ObjectMapper.Map<List<UserPidDto>>(userPids);
            // Map the PidType navigation property
            foreach (var userPidDto in userPidDTOs)
            {
                var userPid = userPids.FirstOrDefault(p => p.Id == userPidDto.Id);
                if (userPid?.PidTypeFk != null)
                {
                    userPidDto.PidType = ObjectMapper.Map<PidTypeDto>(userPid.PidTypeFk);
                }
            }
            // Attach UserPids to each cohort user
            foreach (var cohortUser in cohortUserListDtos)
            {
                cohortUser.UserPids = userPidDTOs.Where(p => p.UserId == cohortUser.CohortUser.UserId).ToList();
            }

            return _cohortUsersBGCExcelExporter.ExportToFile(cohortUserListDtos);
        }

        public class CohortUserComplianceExcel
        {
            public string CohortName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Drug { get; set; }
            public string Background { get; set; }
            public string Immunization { get; set; }
            public string OverallCompliance { get; set; }
            public List<UserPid> UserPids { get; set; } = new List<UserPid>();
            public string DrugStatusColor { get; set; }
            public string BackgroundStatusColor { get; set; }
            public string ImmunizationStatusColor { get; set; }
            public string OverallStatusColor { get; set; }
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers)]
        public async Task<PagedResultDto<CohortUserCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _cohortLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CohortUserCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new CohortUserCohortLookupTableDto
                {
                    Id = cohort.Id.ToString(),
                    DisplayName = cohort.Description?.ToString()
                });
            }

            return new PagedResultDto<CohortUserCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CohortUsers)]
        public async Task<PagedResultDto<CohortUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userQuery = _userLookUpRepository.GetAll()

                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => (e.Name != null && e.Name.Contains(input.Filter))
                || (e.MiddleName != null && e.MiddleName.Contains(input.Filter))
                || (e.Surname != null && e.Surname.Contains(input.Filter))
                || (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
                || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
               );

            //var query = _cohortUserRepository.GetAll().IgnoreQueryFilters()
            //    .Include(e => e.UserFk)
            //    .Where(e=>e.UserFk.IsDeleted==false)
            //    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.UserFk != null && e.UserFk.Name.Contains(input.Filter));

            //var r = userQuery.ToList();
            var q = from u in userQuery
                    join cu in _cohortUserRepository.GetAll() on u.Id equals cu.UserId into cohortJoin
                    from cu in cohortJoin.DefaultIfEmpty()
                    join c in _cohortLookUpRepository.GetAll() on cu.CohortId equals c.Id into cohortInfoJoin
                    from c in cohortInfoJoin.DefaultIfEmpty()
                    join t in _tenantRepository.GetAll() on u.TenantId equals t.Id
                    select new
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        CohortUserId = cu != null ? cu.Id : Guid.Empty,
                        Cohort = c != null ? c.Name : null,
                        CohortId = c != null ? c.Id : Guid.Empty,
                        TenantId = t.Id,
                        Tenant = t.Name
                    };

            var totalCount = await q.CountAsync();

            var cohortUserList = await q
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CohortUserUserLookupTableDto>();
            foreach (var cohortUser in cohortUserList)
            {
                lookupTableDtoList.Add(new CohortUserUserLookupTableDto
                {
                    Id = cohortUser.Id,
                    DisplayName = cohortUser.Name?.ToString(),
                    Surname = cohortUser.Surname?.ToString(),
                    CohortUserId = cohortUser.CohortUserId,
                    TenantId = cohortUser.TenantId,
                    Tenant = cohortUser.Tenant,
                    Cohort = cohortUser.Cohort,
                    CohortId = cohortUser.CohortId
                });
            }

            return new PagedResultDto<CohortUserUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        private async Task AuthorizedForSelf(long id)
        {
            var hasCohortUsersPermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_CohortUsers);
            if (hasCohortUsersPermission) return;
            var hasCohortUserPermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_CohortUser);

            if (id == AbpSession.UserId && hasCohortUserPermission == true) return;

            throw new AbpAuthorizationException("You are not authorized to view this!");
        }

        private async Task<Guid?> GetCohortUserIdForUser()
        {
            var _cohortUser = _cohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.UserId == AbpSession.UserId).FirstOrDefault();
            if (_cohortUser != null)
            {
                return _cohortUser.Id;
            }

            return null;
        }

        private string GetStatusColorForCompliance(bool isCompliant)
        {
            // Get tenant-specific record status colors
            var complianceImpact = isCompliant ? EnumStatusComplianceImpact.Compliant : EnumStatusComplianceImpact.NotCompliant;

            var recordStatus = _recordStatusLookUpRepository.GetAll()
                .Where(rs => rs.ComplianceImpact == complianceImpact && !rs.IsDeleted)
                .FirstOrDefault();

            if (recordStatus != null && !string.IsNullOrEmpty(recordStatus.HtmlColor))
            {
                return recordStatus.HtmlColor;
            }

            // Fallback to default colors if no tenant-specific status found
            return isCompliant ? "#67c777" : "#ba323b";
        }
    }
}