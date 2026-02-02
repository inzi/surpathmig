using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.UI;
using inzibackend.Authorization;
using inzibackend.Authorization.Users;
using inzibackend.Dto;
using inzibackend.EntityFrameworkCore;
using inzibackend.Notifications;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.ComplianceManager;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath.Exporting;
using inzibackend.Url;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
    public class RecordStatesAppService : inzibackendAppServiceBase, IRecordStatesAppService
    {
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRecordStatesExcelExporter _recordStatesExcelExporter;
        private readonly IRepository<Record, Guid> _recordLookUpRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusLookUpRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;

        private readonly IAppNotifier _appNotifier;
        private readonly IDbContextProvider<inzibackendDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly ISurpathComplianceAppService _surpathComplianceAppService;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly SurpathManager _surpathManager;
        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;
        private readonly IUserEmailer _userEmailer;
        private readonly IAppUrlService _appUrlService;

        public RecordStatesAppService(
            IRepository<RecordState, Guid> recordStateRepository,
            IRecordStatesExcelExporter recordStatesExcelExporter,
            IRepository<Record, Guid> lookup_recordRepository,
            IRepository<RecordCategory, Guid> lookup_recordCategoryRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<RecordStatus, Guid> lookup_recordStatusRepository,
            IDbContextProvider<inzibackendDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager,
            IAppNotifier appNotifier,
             ISurpathComplianceAppService surpathComplianceAppService,
             IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
             IRepository<CohortUser, Guid> cohortUserRepository,
             IRepository<RecordRequirement, Guid> recordRequirementRepository,
             IRepository<RecordCategory, Guid> recordCategoryRepository,
             IRepository<RecordCategoryRule, Guid> recordCategoryRuleRepository,
             IRepository<RecordNote, Guid> recordNoteRepository,
             IRepository<SurpathService, Guid> surpathServiceRepository,
             IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
             SurpathManager surpathManager,
             ISurpathComplianceEvaluator surpathComplianceEvaluator,
             IUserEmailer userEmailer,
             IAppUrlService appUrlService)
        {
            _recordStateRepository = recordStateRepository;
            _recordStatesExcelExporter = recordStatesExcelExporter;
            _recordLookUpRepository = lookup_recordRepository;
            _recordCategoryLookUpRepository = lookup_recordCategoryRepository;
            _userLookUpRepository = lookup_userRepository;
            _recordStatusLookUpRepository = lookup_recordStatusRepository;

            _appNotifier = appNotifier;
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;

            _surpathComplianceAppService = surpathComplianceAppService;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _cohortUserRepository = cohortUserRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _recordCategoryRepository = recordCategoryRepository;
            _recordCategoryRuleRepository = recordCategoryRuleRepository;
            _recordNoteRepository = recordNoteRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _surpathManager = surpathManager;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;

            _userEmailer = userEmailer;
            _appUrlService = appUrlService;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task<exdtos.PagedResultDto<GetRecordStateForViewDto>> GetAll(GetAllRecordStatesInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var stateFilter = input.StateFilter.HasValue
                        ? (EnumRecordState)input.StateFilter
                        : default;

            var filteredRecordStates = _recordStateRepository.GetAll()
                        .Include(e => e.RecordFk)
                        .Include(e => e.RecordCategoryFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordStatusFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .Where(e => e.RecordCategoryFk.RecordRequirementFk.IsSurpathOnly == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes == input.NotesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordfilenameFilter), e => e.RecordFk != null && e.RecordFk.filename == input.RecordfilenameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryNameFilter), e => e.RecordCategoryFk != null && e.RecordCategoryFk.Name == input.RecordCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStatusStatusNameFilter), e => e.RecordStatusFk != null && e.RecordStatusFk.StatusName == input.RecordStatusStatusNameFilter);

            input.SortingUser = input.SortingUser ?? "asc";
            //var _alwaysby = "userFk.name ";
            var _alwaysby = "userFk.id ";
            var _alwaysbyval = $"{_alwaysby} {input.SortingUser}";
            input.Sorting = input.Sorting ?? _alwaysbyval;
            if (input.Sorting.StartsWith(_alwaysby))
            {
                _alwaysbyval = $"{_alwaysby}{input.Sorting.Split(' ').Last()}";
            }

            if (!input.Sorting.StartsWith(_alwaysby))
            {
                input.Sorting = _alwaysbyval + "," + input.Sorting;
            }
            input.SortingUser = _alwaysbyval.Split(' ').Last();

            var pagedAndFilteredRecordStates = filteredRecordStates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var recordStates = from o in pagedAndFilteredRecordStates
                               join o1 in _recordLookUpRepository.GetAll() on o.RecordId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _recordCategoryLookUpRepository.GetAll() on o.RecordCategoryId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               join o4 in _recordStatusLookUpRepository.GetAll() on o.RecordStatusId equals o4.Id into j4
                               from s4 in j4.DefaultIfEmpty()

                               select new
                               {
                                   o.State,
                                   o.Notes,
                                   Id = o.Id,
                                   Recordfilename = s1 == null || s1.filename == null ? "" : s1.filename.ToString(),
                                   RecordCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                   FullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserId = s3 == null || s3.Id == null ? -1 : s3.Id,
                                   UserTenantId = s3 == null || s3.Id == null ? -1 : s3.TenantId,
                                   RecordStatusStatusName = s4 == null || s4.StatusName == null ? "" : s4.StatusName.ToString(),
                                   RecordStatusHighlight = s4 == null || s4.HtmlColor == null ? "" : s4.HtmlColor,
                                   RecordStatusCSSClass = s4 == null || s4.CSSCLass == null ? "" : s4.CSSCLass,
                                   RecordStatusId = s4 == null || s4.Id == Guid.Empty ? Guid.Empty : s4.Id
                               };

            var totalCount = await filteredRecordStates.CountAsync();

            var dbList = await recordStates.ToListAsync();
            var results = new List<GetRecordStateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordStateForViewDto()
                {
                    RecordState = new RecordStateDto
                    {
                        State = o.State,
                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    RecordStatus = new RecordStatusDto
                    {
                        Id = o.RecordStatusId,
                        StatusName = o.RecordStatusStatusName,
                        HtmlColor = o.RecordStatusHighlight,
                        CSSCLass = o.RecordStatusCSSClass,
                    },
                    Recordfilename = o.Recordfilename,
                    RecordCategoryName = o.RecordCategoryName,
                    UserName = o.FullName,
                    FullName = o.FullName,
                    UserId = o.UserId,
                    RecordStatusStatusName = o.RecordStatusStatusName,
                    CohortUserId = await _surpathManager.GetCohortUserIdForUser((long)o.UserId, (int)o.UserTenantId)
                };

                results.Add(res);
            }

            return new exdtos.PagedResultDto<GetRecordStateForViewDto>(
                totalCount,
                results,
                input.SortingUser
            );
        }

        public async Task<GetRecordStateForViewDto> GetRecordStateForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordState = await _recordStateRepository.GetAsync(id);

            var output = new GetRecordStateForViewDto { RecordState = ObjectMapper.Map<RecordStateDto>(recordState) };

            if (output.RecordState.RecordId != null)
            {
                var _lookupRecord = await _recordLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordId);
                output.Recordfilename = _lookupRecord?.filename?.ToString();
            }

            if (output.RecordState.RecordCategoryId != null)
            {
                var _lookupRecordCategory = await _recordCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordCategoryId);
                output.RecordCategoryName = _lookupRecordCategory?.Name?.ToString();
            }

            if (output.RecordState.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordState.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.RecordState.RecordStatusId != null)
            {
                var _lookupRecordStatus = await _recordStatusLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordStatusId);
                output.RecordStatusStatusName = _lookupRecordStatus?.StatusName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Edit)]
        public async Task<GetRecordStateForEditOutput> GetRecordStateForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordState = _recordStateRepository.GetAll().Include(e => e.RecordCategoryFk).ThenInclude(e => e.RecordRequirementFk).Where(e => e.Id == input.Id).FirstOrDefault();

            var output = new GetRecordStateForEditOutput { RecordState = ObjectMapper.Map<CreateOrEditRecordStateDto>(recordState) };

            if (output.RecordState.RecordId != null)
            {
                var _lookupRecord = await _recordLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordId);
                output.Recordfilename = _lookupRecord?.filename?.ToString();
            }

            if (output.RecordState.RecordCategoryId != null)
            {
                var _lookupRecordCategory = await _recordCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordCategoryId);
                output.RecordCategoryName = _lookupRecordCategory?.Name?.ToString();
            }

            if (output.RecordState.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.RecordState.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.RecordState.RecordStatusId != null)
            {
                var _lookupRecordStatus = await _recordStatusLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordState.RecordStatusId);
                output.RecordStatusStatusName = _lookupRecordStatus?.StatusName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Edit)]
        public async Task<GetRecordStateForEditOutput> GetRecordStateForReview(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            //var recordState = await _recordStateRepository.FirstOrDefaultAsync(input.Id);
            var recordState = _recordStateRepository
                .GetAll().IgnoreQueryFilters()
                .Where(e => e.Id == input.Id && e.IsDeleted == false)
                .Include(e => e.RecordFk)
                .Include(e => e.RecordStatusFk)
                .Include(e => e.UserFk)
                .Include(e => e.RecordCategoryFk)
                .ThenInclude(e => e.RecordRequirementFk)
                .Include(e => e.RecordCategoryFk)
                .ThenInclude(e => e.RecordCategoryRuleFk)
                .ToList()
                .OrderByDescending(e => e.CreationTime)
                .FirstOrDefault();
            if (recordState == null)
            {
                throw new UserFriendlyException("Record not found! Please ensure it has not been delete or try again.");
            }
            var _IsSurpathOnly = recordState.RecordCategoryFk.RecordRequirementFk.IsSurpathOnly;
            var output = new GetRecordStateForEditOutput { RecordState = ObjectMapper.Map<CreateOrEditRecordStateDto>(recordState) };

            if (output.RecordState.RecordId != null)
            {
                output.Recordfilename = recordState.RecordFk.filename?.ToString(); //  _lookupRecord?.filename?.ToString();

                output.RecordState.RecordDto = ObjectMapper.Map<RecordDto>(recordState.RecordFk);
            }

            if (recordState.RecordCategoryFk != null)
            {
                output.RecordCategoryName = recordState.RecordCategoryFk.Name?.ToString();
                output.RecordState.RecordCategoryDto = ObjectMapper.Map<RecordCategoryDto>(recordState.RecordCategoryFk);
                if (recordState.RecordCategoryFk.RecordRequirementFk != null)
                {
                    output.RecordRequirementName = recordState.RecordCategoryFk.RecordRequirementFk.Name?.ToString();
                }
                if (recordState.RecordCategoryFk.RecordCategoryRuleFk != null)
                {
                    output.RecordState.RecordCategoryDto.RecordCategoryRule = ObjectMapper.Map<RecordCategoryRuleDto>(recordState.RecordCategoryFk.RecordCategoryRuleFk);
                }
            }

            if (output.RecordState.UserId != null)
            {
                output.UserName = recordState.UserFk.Name?.ToString();
            }

            if (output.RecordState.RecordStatusId != null)
            {
                output.RecordStatusStatusName = recordState.RecordStatusFk.StatusName?.ToString();
            }
            output.TenantId = recordState.TenantId;

            output.IsSurpathOnly = _IsSurpathOnly;
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Edit)]
        public async Task CreateOrEdit(CreateOrEditRecordStateDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Create)]
        protected virtual async Task Create(CreateOrEditRecordStateDto input)
        {
            input = await _surpathComplianceAppService.CreateNewRecordState(input);

            CurrentUnitOfWork.SaveChanges();
            await ArchiveOldRecordStatesWithSameCategory(input);
            await StateGhangeNotify(input);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Edit)]
        protected virtual async Task Update(CreateOrEditRecordStateDto input)
        {
            var recordState = await _recordStateRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordState);
            CurrentUnitOfWork.SaveChanges();

            // if we have effective / expiry dates, we need to update the record
            if (input.RecordDto.Id != Guid.Empty && input.RecordDto.EffectiveDate != null && input.RecordDto.ExpirationDate != null)
            {
                // get the record
                var record = await _recordLookUpRepository.FirstOrDefaultAsync((Guid)input.RecordDto.Id);
                // update the record
                if (record.EffectiveDate != input.RecordDto.EffectiveDate || record.ExpirationDate != input.RecordDto.ExpirationDate)
                {
                    record.EffectiveDate = input.RecordDto.EffectiveDate;
                    record.ExpirationDate = input.RecordDto.ExpirationDate;
                }
                CurrentUnitOfWork.SaveChanges();
            }

            await StateGhangeNotify(input);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _recordStateRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task<FileDto> GetRecordStatesToExcel(GetAllRecordStatesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var stateFilter = input.StateFilter.HasValue
                        ? (EnumRecordState)input.StateFilter
                        : default;

            var filteredRecordStates = _recordStateRepository.GetAll()
                        .Include(e => e.RecordFk)
                        .Include(e => e.RecordCategoryFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordStatusFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes == input.NotesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordfilenameFilter), e => e.RecordFk != null && e.RecordFk.filename == input.RecordfilenameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryNameFilter), e => e.RecordCategoryFk != null && e.RecordCategoryFk.Name == input.RecordCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStatusStatusNameFilter), e => e.RecordStatusFk != null && e.RecordStatusFk.StatusName == input.RecordStatusStatusNameFilter);

            var query = (from o in filteredRecordStates
                         join o1 in _recordLookUpRepository.GetAll() on o.RecordId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _recordCategoryLookUpRepository.GetAll() on o.RecordCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _recordStatusLookUpRepository.GetAll() on o.RecordStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetRecordStateForViewDto()
                         {
                             RecordState = new RecordStateDto
                             {
                                 State = o.State,
                                 Notes = o.Notes,
                                 Id = o.Id
                             },
                             Recordfilename = s1 == null || s1.filename == null ? "" : s1.filename.ToString(),
                             RecordCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RecordStatusStatusName = s4 == null || s4.StatusName == null ? "" : s4.StatusName.ToString()
                         });

            var recordStateListDtos = await query.ToListAsync();

            return _recordStatesExcelExporter.ExportToFile(recordStateListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task<exdtos.PagedResultDto<RecordStateRecordLookupTableDto>> GetAllRecordForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.filename != null && e.filename.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RecordStateRecordLookupTableDto>();
            foreach (var record in recordList)
            {
                lookupTableDtoList.Add(new RecordStateRecordLookupTableDto
                {
                    Id = record.Id.ToString(),
                    DisplayName = record.filename?.ToString()
                });
            }

            return new exdtos.PagedResultDto<RecordStateRecordLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task<exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>> GetAllRecordCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordCategoryLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            if (input.Shuffle)
            {
                Random rand = new Random();
                recordCategoryList = recordCategoryList.OrderBy(c => rand.Next()).ToList();
            }

            var lookupTableDtoList = new List<RecordStateRecordCategoryLookupTableDto>();
            foreach (var recordCategory in recordCategoryList)
            {
                lookupTableDtoList.Add(new RecordStateRecordCategoryLookupTableDto
                {
                    Id = recordCategory.Id.ToString(),
                    DisplayName = recordCategory.Name?.ToString(),
                    DisplayInstructions = recordCategory.Instructions?.ToString(),
                });
            }

            return new exdtos.PagedResultDto<RecordStateRecordCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task<exdtos.PagedResultDto<RecordStateUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _userLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter))
                || (e.MiddleName != null && e.MiddleName.Contains(input.Filter))
                || (e.Surname != null && e.Surname.Contains(input.Filter))
                || (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
                || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RecordStateUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new RecordStateUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new exdtos.PagedResultDto<RecordStateUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task ArchiveOldRecordStatesWithSameCategory(CreateOrEditRecordStateDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                //if (AbpSession.TenantId == null)
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                // Switching to host is necessary for single tenant mode.
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    // IMPORTANT: We should NOT delete old record states
                    // They should remain in the system as historical records visible in archives
                    // When a new document is uploaded, mark all previous versions as IsArchived=true
                    // to ensure only the new document is used for compliance calculations

                    var _rsId = (Guid)input.Id;
                    var _catId = (Guid)input.RecordCategoryId;

                    if (_rsId != Guid.Empty && _catId != Guid.Empty)
                    {
                        // Find all old record states for this user/category combination
                        var oldRecordStates = await _recordStateRepository.GetAll()
                            .Where(r => r.UserId == input.UserId && r.RecordCategoryId == _catId && r.Id != _rsId)
                            .ToListAsync();

                        if (oldRecordStates.Any())
                        {
                            // Mark all old records as archived
                            foreach (var oldRecord in oldRecordStates)
                            {
                                oldRecord.IsArchived = true;
                                // ArchivedTime and ArchivedByUserId will be set automatically by DbContext
                                await _recordStateRepository.UpdateAsync(oldRecord);
                            }

                            Logger.Info($"Marked {oldRecordStates.Count} old record states as IsArchived=true for user {input.UserId} " +
                                       $"in category {_catId}. New current record state: {_rsId}");
                        }

                        // DO NOT DELETE - Let the archives show the full history
                        // _oldRsIds.ForEach(async r => await _recordStateRepository.DeleteAsync(r));
                    }
                }
                uow.Complete();
            }
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task StateGhangeNotify(CreateOrEditRecordStateDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                //if (AbpSession.TenantId == null)
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                // Switching to host is necessary for single tenant mode.
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    //var _context = await _dbContextProvider.GetDbContextAsync();

                    var _info = from u in _userLookUpRepository.GetAll().Where(u => u.Id == (long)input.UserId)
                                join chu in _cohortUserRepository.GetAll() on u.Id equals chu.UserId
                                join rs in _recordStateRepository.GetAll().Where(r => r.Id == input.Id) on u.Id equals rs.UserId
                                join st in _recordStatusLookUpRepository.GetAll() on rs.RecordStatusId equals st.Id
                                join rc in _recordCategoryRepository.GetAll() on rs.RecordCategoryId equals rc.Id
                                join req in _recordRequirementRepository.GetAll() on rc.RecordRequirementId equals req.Id
                                join rule in _recordCategoryRuleRepository.GetAll() on rc.RecordCategoryRuleId equals rule.Id
                                join r in _recordLookUpRepository.GetAll() on rs.RecordId equals r.Id
                                select new
                                {
                                    user = u,
                                    cohortUser = chu,
                                    recordState = rs,
                                    recordStatus = st,
                                    recordCategory = rc,
                                    recordCategoryRule = rule,
                                    recordRequirement = req,
                                    record = r
                                };
                    var thisInfo = _info.FirstOrDefault();

                    if (thisInfo != null)
                    {
                        if (thisInfo.recordCategoryRule.Notify == true)
                        {
                            await _appNotifier.RecordStateChanged(
                                   thisInfo.user.ToUserIdentifier(),
                                   new LocalizableString(
                                       AppNotificationNames.RecordStateChanged,
                                       inzibackendConsts.LocalizationSourceName
                                   ),
                                   null,
                                   Abp.Notifications.NotificationSeverity.Warn);
                        }
                        if (thisInfo.recordState.State == EnumRecordState.Rejected)
                        {
                            if (thisInfo.cohortUser != null)
                            {
                                var link = _appUrlService.CreateViewCohortUserUrlFormat(thisInfo.user.TenantId);
                                link = link.Replace("{cohortUserId}", thisInfo.cohortUser.Id.ToString());
                                await _userEmailer.SendEmailForComplianceRelatedNotification(thisInfo.user, link);
                            }
                        }
                    }
                }
                uow.Complete();
            }
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
        public async Task<List<RecordStateRecordStatusLookupTableDto>> GetAllRecordStatusForTableDropdown(int? TenantId = null)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _recordStatusLookUpRepository.GetAll()
                .WhereIf(AbpSession.TenantId == null && TenantId.HasValue, e => e.TenantId == TenantId)
                .WhereIf(AbpSession.TenantId != null, e => e.IsSurpathServiceStatus == false)
                .Select(recordStatus => new RecordStateRecordStatusLookupTableDto
                {
                    Id = recordStatus.Id.ToString(),
                    DisplayName = recordStatus == null || recordStatus.StatusName == null ? "" : recordStatus.StatusName.ToString(),
                    IsDefault = recordStatus == null ? false : recordStatus.IsDefault,
                    RequireNoteOnSet = recordStatus == null ? false : recordStatus.RequireNoteOnSet,
                    IsSurpathServiceStatus = recordStatus == null ? false : recordStatus.IsSurpathServiceStatus
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
        public async Task<List<RecordStateRecordStatusLookupTableDto>> GetAllServiceRecordStatusForTableDropdown()
        {
            try
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var _retval = await _recordStatusLookUpRepository.GetAll()
                    //.WhereIf(AbpSession.TenantId == null && TenantId.HasValue, e => e.TenantId == TenantId)
                    .Where(e => e.TenantId == null)
                    .Where(e => e.IsSurpathServiceStatus == true)
                    .Select(recordStatus => new RecordStateRecordStatusLookupTableDto
                    {
                        Id = recordStatus == null ? "" : recordStatus.Id.ToString(),
                        DisplayName = recordStatus == null || recordStatus.StatusName == null ? "" : recordStatus.StatusName.ToString(),
                        IsDefault = recordStatus == null ? false : recordStatus.IsDefault,
                        RequireNoteOnSet = recordStatus == null ? false : recordStatus.RequireNoteOnSet,
                        IsSurpathServiceStatus = recordStatus == null ? false : recordStatus.IsSurpathServiceStatus
                    }).ToListAsync();
                _retval = _retval.Where(r => string.IsNullOrEmpty(r.Id) == false).ToList();
                return _retval;
            }
            catch (Exception)
            {
                throw new UserFriendlyException("Check services configuration for client!");
            }
        }

        // Task<exdtos.PagedResultDto<GetRecordStateForViewDto>>
        [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
        public async Task<exdtos.PagedResultDto<GetRecordStateForViewDto>> GetAllForUserId(GetAllRecordStatesInput input)
        {
            // Must have permissions or be this logged in user
            // TODO check permissions and logged in user id, if not permission or user, we have to throw

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var stateFilter = input.StateFilter.HasValue
                        ? (EnumRecordState)input.StateFilter
                        : default;
            var r = _recordStateRepository.GetAll().Where(r => r.UserId == input.UserId);

            var filteredRecordStates = _recordStateRepository.GetAll()
                        .Include(e => e.RecordFk)
                        .Include(e => e.RecordCategoryFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordStatusFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .Where(e => e.UserId == input.UserId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes == input.NotesFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordfilenameFilter), e => e.RecordFk != null && e.RecordFk.filename == input.RecordfilenameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryNameFilter), e => e.RecordCategoryFk != null && e.RecordCategoryFk.Name == input.RecordCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordStatusStatusNameFilter), e => e.RecordStatusFk != null && e.RecordStatusFk.StatusName == input.RecordStatusStatusNameFilter);

            input.SortingUser = input.SortingUser ?? "asc";
            var _alwaysby = "userFk.name ";
            var _alwaysbyval = $"{_alwaysby} {input.SortingUser}";
            input.Sorting = input.Sorting ?? _alwaysbyval;
            if (input.Sorting.StartsWith(_alwaysby))
            {
                _alwaysbyval = $"{_alwaysby}{input.Sorting.Split(' ').Last()}";
            }

            if (!input.Sorting.StartsWith(_alwaysby))
            {
                input.Sorting = _alwaysbyval + "," + input.Sorting;
            }
            input.SortingUser = _alwaysbyval.Split(' ').Last();

            var pagedAndFilteredRecordStates = filteredRecordStates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var recordStates = from o in pagedAndFilteredRecordStates
                               join o1 in _recordLookUpRepository.GetAll() on o.RecordId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _recordCategoryLookUpRepository.GetAll() on o.RecordCategoryId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               join o4 in _recordStatusLookUpRepository.GetAll() on o.RecordStatusId equals o4.Id into j4
                               from s4 in j4.DefaultIfEmpty()

                               select new
                               {
                                   o.State,
                                   o.Notes,
                                   Id = o.Id,
                                   Recordfilename = s1 == null || s1.filename == null ? "" : s1.filename.ToString(),
                                   RecordCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                   FullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                   UserId = s3 == null || s3.Id == null ? -1 : s3.Id,
                                   RecordStatusStatusName = s4 == null || s4.StatusName == null ? "" : s4.StatusName.ToString(),
                                   RecordStatusHighlight = s4 == null || s4.HtmlColor == null ? "" : s4.HtmlColor,
                                   RecordStatusCSSClass = s4 == null || s4.CSSCLass == null ? "" : s4.CSSCLass,
                                   RecordStatusId = s4 == null || s4.Id == Guid.Empty ? Guid.Empty : s4.Id,
                                   RecordCategoryId = s2 == null ? Guid.Empty : s2.Id,
                                   RecordId = s1 == null || s1.filename == null ? Guid.Empty : s1.Id,
                                   Record = s1
                               };

            var totalCount = await filteredRecordStates.CountAsync();

            var dbList = await recordStates.ToListAsync();
            var results = new List<GetRecordStateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordStateForViewDto()
                {
                    RecordState = new RecordStateDto
                    {
                        State = o.State,
                        Notes = o.Notes,
                        Id = o.Id,
                        RecordCategoryId = o.RecordCategoryId,
                        RecordId = o.RecordId,
                        RecordStatusId = o.RecordStatusId,
                        UserId = o.UserId,
                        RecordDto = new RecordDto()
                        {
                            BinaryObjId = o.Record.BinaryObjId,
                            TenantDocumentCategoryId = o.Record.TenantDocumentCategoryId
                        }
                    },
                    RecordStatus = new RecordStatusDto
                    {
                        Id = o.RecordStatusId,
                        StatusName = o.RecordStatusStatusName,
                        HtmlColor = o.RecordStatusHighlight,
                        CSSCLass = o.RecordStatusCSSClass,
                    },
                    Recordfilename = o.Recordfilename,
                    RecordCategoryName = o.RecordCategoryName,
                    UserName = o.FullName,
                    FullName = o.FullName,
                    UserId = o.UserId,
                    RecordStatusStatusName = o.RecordStatusStatusName
                };

                results.Add(res);
            }

            return new exdtos.PagedResultDto<GetRecordStateForViewDto>(
                totalCount,
                results,
                input.SortingUser
            );
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
        public async Task<exdtos.PagedResultDto<GetRecordStateCompliancetForViewDto>> GetRecordStateComplianceForUserId(GetAllRecordStatesInput input)
        {
            var _results = await _surpathComplianceEvaluator.GetComplianceStatesForUser(input.UserId);

            return new exdtos.PagedResultDto<GetRecordStateCompliancetForViewDto>(
                _results.Count(),
                _results,
                input.SortingUser
            );
        }

        public async Task<int?> GetTenantIdForRecordState(EntityDto<Guid> input)
        {
            int? tenantid = null;

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordState = _recordStateRepository.GetAll().Include(e => e.RecordCategoryFk).ThenInclude(e => e.RecordRequirementFk).Where(e => e.Id == input.Id).FirstOrDefault();
            if (recordState != null) tenantid = recordState.TenantId;
            return tenantid;
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates, AppPermissions.Pages_CohortUser)]
        public async Task<List<GetArchivedDocumentsDto>> GetArchivedDocumentsForUser(GetArchivedDocumentsInput input)
        {
            await AuthorizedForSelf(input.UserId);

            // Disable tenant filter to ensure we can see all records
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            // Also disable soft-delete filter to see archived records
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);

            // Get all record states for the user, including historical ones
            var recordStatesQuery = _recordStateRepository.GetAll()
                .Include(e => e.RecordFk)
                .Include(e => e.RecordCategoryFk)
                    .ThenInclude(e => e.RecordRequirementFk)
                .Include(e => e.RecordStatusFk)
                .Where(e => e.UserId == input.UserId && e.IsArchived == true && e.IsDeleted == false);

            // Apply date filters if provided
            if (input.StartDate.HasValue)
            {
                recordStatesQuery = recordStatesQuery.Where(e => e.CreationTime >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                recordStatesQuery = recordStatesQuery.Where(e => e.CreationTime <= input.EndDate.Value);
            }

            // Apply category filter if provided
            if (input.RecordCategoryId.HasValue)
            {
                recordStatesQuery = recordStatesQuery.Where(e => e.RecordCategoryId == input.RecordCategoryId.Value);
            }

            var recordStates = await recordStatesQuery
                .OrderByDescending(e => e.CreationTime)
                .ToListAsync();

            // Log for debugging
            Logger.Info($"GetArchivedDocumentsForUser: Found {recordStates.Count} archived records for user {input.UserId}");

            // Get current user requirements to identify which categories are still active
            var currentUserRequirements = await _surpathComplianceEvaluator.GetComplianceStatesForUser(input.UserId);

            // Get active category IDs for the user (categories that have current documents)
            var activeCategoryIds = currentUserRequirements
                .Where(c => c.RecordCategory?.Id != null)
                .Select(c => c.RecordCategory.Id)
                .ToHashSet();

            // Group by record category
            var groupedRecords = recordStates
                .GroupBy(r => new
                {
                    RequirementId = r.RecordCategoryFk?.RecordRequirementFk?.Id,
                    CategoryId = r.RecordCategoryFk?.Id,
                    RequirementName = r.RecordCategoryFk?.RecordRequirementFk?.Name,
                    CategoryName = r.RecordCategoryFk?.Name,
                    CategoryDeleted = r.RecordCategoryFk?.IsDeleted ?? false,
                    RequirementDeleted = r.RecordCategoryFk?.RecordRequirementFk?.IsDeleted ?? false
                })
                .Where(g => g.Key.CategoryId != null); // Include even if requirement is null (orphaned)

            var result = new List<GetArchivedDocumentsDto>();

            foreach (var group in groupedRecords)
            {
                var archivedDocuments = new List<ArchivedDocumentItemDto>();
                var categoryId = group.Key.CategoryId.Value;
                var isActiveCategory = activeCategoryIds.Contains(categoryId);

                foreach (var recordState in group.OrderByDescending(r => r.CreationTime))
                {
                    // Get creator user info
                    var creatorUser = recordState.CreatorUserId.HasValue
                        ? await _userLookUpRepository.GetAll()
                            .Where(u => u.Id == recordState.CreatorUserId.Value)
                            .Select(u => new { u.Name, u.Surname })
                            .FirstOrDefaultAsync()
                        : null;

                    archivedDocuments.Add(new ArchivedDocumentItemDto
                    {
                        Id = recordState.Id,
                        RecordStateId = recordState.Id,
                        RecordId = recordState.RecordId,
                        FileName = recordState.RecordFk?.filename ?? "Unknown",
                        CreationTime = recordState.CreationTime,
                        State = recordState.State,
                        Notes = recordState.Notes,
                        BinaryObjId = recordState.RecordFk?.BinaryObjId,
                        IsArchived = recordState.IsArchived,
                        CreatedByUserName = creatorUser != null ? $"{creatorUser.Name} {creatorUser.Surname}" : "System",
                        CreatedByUserId = recordState.CreatorUserId,
                        RecordStatus = recordState.RecordStatusFk != null ? ObjectMapper.Map<RecordStatusDto>(recordState.RecordStatusFk) : null
                    });
                }

                // Always include categories that have any documents
                // Archives should show ALL historical documents, including previous versions
                if (archivedDocuments.Any())
                {
                    var categoryName = group.Key.CategoryName ?? "Unknown Category";
                    var requirementName = group.Key.RequirementName ?? "Unknown Requirement";

                    // Add indicators for deleted or inactive requirements
                    if (group.Key.RequirementDeleted || group.Key.CategoryDeleted)
                    {
                        categoryName = $"[DELETED] {categoryName}";
                    }
                    else if (!isActiveCategory)
                    {
                        categoryName = $"[INACTIVE] {categoryName}";
                    }
                    // No special indicator for active categories - they're the normal case

                    result.Add(new GetArchivedDocumentsDto
                    {
                        RecordRequirement = group.First().RecordCategoryFk?.RecordRequirementFk != null
                            ? ObjectMapper.Map<RecordRequirementDto>(group.First().RecordCategoryFk.RecordRequirementFk)
                            : null,
                        RecordCategory = ObjectMapper.Map<RecordCategoryDto>(group.First().RecordCategoryFk),
                        CategoryName = $"{requirementName} - {categoryName}",
                        TotalDocuments = archivedDocuments.Count,
                        ArchivedDocuments = archivedDocuments
                    });
                }
            }

            return result.OrderBy(r => r.CategoryName).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStates)]
        public async Task ReassociateDocument(ReassociateDocumentInput input)
        {
            // Get the record state
            var recordState = await _recordStateRepository.GetAsync(input.RecordStateId);
            if (recordState == null)
            {
                throw new UserFriendlyException("Record state not found");
            }

            // Verify the user has permission to modify this record
            await AuthorizedForSelf(recordState.UserId.Value);

            // Get the new category to validate it exists
            var newCategory = await _recordCategoryRepository.GetAsync(input.NewRecordCategoryId);
            if (newCategory == null)
            {
                throw new UserFriendlyException("Target category not found");
            }

            // Store the old category info for audit
            var oldCategoryId = recordState.RecordCategoryId;
            var oldCategory = oldCategoryId.HasValue
                ? await _recordCategoryRepository.GetAsync(oldCategoryId.Value)
                : null;

            // Check if there's already a current (non-archived) document for the target category
            var existingCurrentDoc = await _recordStateRepository.GetAll()
                .Where(r => r.UserId == recordState.UserId &&
                           r.RecordCategoryId == input.NewRecordCategoryId &&
                           r.IsArchived == false)
                .FirstOrDefaultAsync();

            // If there's no current document for the new category, un-archive this one
            if (existingCurrentDoc == null)
            {
                recordState.IsArchived = false;
                // ArchivedTime and ArchivedByUserId will be cleared automatically by DbContext
            }
            else
            {
                // Otherwise, keep it as archived
                recordState.IsArchived = true;
            }

            // Update the category
            recordState.RecordCategoryId = input.NewRecordCategoryId;
            recordState.LastModificationTime = DateTime.UtcNow;
            recordState.LastModifierUserId = AbpSession.UserId;

            // Add audit note
            var auditNote = new RecordNote
            {
                RecordStateId = recordState.Id,
                Note = $"📋 DOCUMENT REASSOCIATED 📋\n" +
                       $"Document moved from '{oldCategory?.Name ?? "No Category"}' to '{newCategory.Name}'\n" +
                       $"Reassociated by user {AbpSession.UserId} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.\n" +
                       $"IsArchived set to: {recordState.IsArchived}\n" +
                       $"Reason: {input.Notes ?? "No reason provided"}",
                Created = DateTime.UtcNow,
                AuthorizedOnly = false,
                HostOnly = false,
                SendNotification = false,
                UserId = AbpSession.UserId,
                TenantId = AbpSession.TenantId
            };

            // Save changes
            await _recordStateRepository.UpdateAsync(recordState);
            await _recordNoteRepository.InsertAsync(auditNote);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Notify the user about the change
            await _appNotifier.SendMessageAsync(
                new UserIdentifier(AbpSession.TenantId, recordState.UserId.Value),
                $"Your document has been reassociated to category: {newCategory.Name}",
                Abp.Notifications.NotificationSeverity.Info);
        }

        private async Task AuthorizedForSelf(long id)
        {
            var hasCohortUsersPermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_CohortUsers);
            if (hasCohortUsersPermission) return;
            var hasCohortUserPermission = await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_CohortUser);

            if (id == AbpSession.UserId && hasCohortUserPermission == true) return;

            throw new AbpAuthorizationException("You are not authorized to view this!");
        }
    }
}