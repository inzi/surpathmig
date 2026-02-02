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
using Abp.Domain.Uow;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using Abp.Organizations;
using Abp.Authorization.Users;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
    public class TenantSurpathServicesAppService : inzibackendAppServiceBase, ITenantSurpathServicesAppService
    {
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly ITenantSurpathServicesExcelExporter _tenantSurpathServicesExcelExporter;
        private readonly IRepository<SurpathService, Guid> _surpathServiceLookUpRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IRepository<Cohort, Guid> _cohortLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleLookUpRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;

        public TenantSurpathServicesAppService(
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            ITenantSurpathServicesExcelExporter tenantSurpathServicesExcelExporter,
            IRepository<SurpathService, Guid> surpathServiceLookUpRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentLookUpRepository,
            IRepository<Cohort, Guid> cohortLookUpRepository,
            IRepository<User, long> userLookUpRepository,
            IRepository<RecordCategoryRule, Guid> recordCategoryRuleLookUpRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            HierarchicalPricingManager hierarchicalPricingManager)
        {
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _tenantSurpathServicesExcelExporter = tenantSurpathServicesExcelExporter;
            _surpathServiceLookUpRepository = surpathServiceLookUpRepository;
            _tenantDepartmentLookUpRepository = tenantDepartmentLookUpRepository;
            _cohortLookUpRepository = cohortLookUpRepository;
            _userLookUpRepository = userLookUpRepository;
            _recordCategoryRuleLookUpRepository = recordCategoryRuleLookUpRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _cohortUserRepository = cohortUserRepository;
            _hierarchicalPricingManager = hierarchicalPricingManager;
        }

        public async Task<PagedResultDto<GetTenantSurpathServiceForViewDto>> GetAll(GetAllTenantSurpathServicesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredTenantSurpathServices = _tenantSurpathServiceRepository.GetAll()
                        .Include(e => e.SurpathServiceFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsEnabledFilter.HasValue && input.IsEnabledFilter > -1, e => (input.IsEnabledFilter == 1 && e.IsPricingOverrideEnabled) || (input.IsEnabledFilter == 0 && !e.IsPricingOverrideEnabled))
                            .WhereIf(input.TenantId.HasValue, e => e.TenantId == input.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter);

            var pagedAndFilteredTenantSurpathServices = filteredTenantSurpathServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantNames = await TenantManager.GetTenantNames();
            var _ServiceNames = new Dictionary<Guid, string>();

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                _ServiceNames = _surpathServiceLookUpRepository.GetAll().Select(x => new { x.Id, x.Name }).ToDictionary(x => x.Id, x => x.Name);
            }

            var tenantSurpathServices = from o in pagedAndFilteredTenantSurpathServices
                                        join o1 in _surpathServiceLookUpRepository.GetAll().Where(s => s.TenantId == null) on o.SurpathServiceId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()

                                        join o3 in _cohortLookUpRepository.GetAll() on o.CohortId equals o3.Id into j3
                                        from s3 in j3.DefaultIfEmpty()

                                        join o4 in _userLookUpRepository.GetAll() on o.UserId equals o4.Id into j4
                                        from s4 in j4.DefaultIfEmpty()

                                        join o5 in _recordCategoryRuleLookUpRepository.GetAll() on o.RecordCategoryRuleId equals o5.Id into j5
                                        from s5 in j5.DefaultIfEmpty()

                                        select new
                                        {
                                            o.Name,
                                            o.Price,
                                            o.Description,
                                            o.IsPricingOverrideEnabled,
                                            Id = o.Id,
                                            SurpathServiceId = o.SurpathServiceId,
                                            SurpathServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                            RecordCategoryRuleName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                                            TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                            CohortName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                            UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                                            TenantId = o.TenantId
                                        };

            var totalCount = await filteredTenantSurpathServices.CountAsync();

            var dbList = await tenantSurpathServices.ToListAsync();
            var results = new List<GetTenantSurpathServiceForViewDto>();
            dbList = dbList.Where(o => o.TenantId != null).ToList();
            foreach (var o in dbList)
            {
                var _tenantName = AbpSession.TenantId == null ? tenantNames[(int)o.TenantId] : "";
                var _serviceName = o.SurpathServiceId == null ? "" : _ServiceNames[(Guid)o.SurpathServiceId];

                var res = new GetTenantSurpathServiceForViewDto()
                {
                    TenantSurpathService = new TenantSurpathServiceDto
                    {
                        Name = o.Name,
                        Price = o.Price,
                        Description = o.Description,
                        IsPricingOverrideEnabled = o.IsPricingOverrideEnabled,
                        Id = o.Id,
                    },
                    TenantDepartmentName = o.TenantDepartmentName,
                    CohortName = o.CohortName,
                    UserName = o.UserName,
                    SurpathServiceName = _serviceName,
                    TenantName = _tenantName,
                    RecordCategoryRuleName = o.RecordCategoryRuleName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantSurpathServiceForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<PagedResultDto<GetSurpathServiceForViewDto>> GetAllTenantServices(GetAllSurpathServicesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredTenantSurpathServices = _tenantSurpathServiceRepository.GetAll()
                .Include(e => e.SurpathServiceFk)
                .Include(e => e.TenantDepartmentFk)
                .Include(e => e.CohortFk)
                .Include(e => e.UserFk)
                .Include(e => e.RecordCategoryRuleFk)
                .WhereIf(input.TenantId != null, e => e.TenantId == input.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SurpathServiceFk.Name.Contains(input.Filter) || e.SurpathServiceFk.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.SurpathServiceFk.Name.Contains(input.NameFilter))
                .WhereIf(input.MinPriceFilter != null, e => e.SurpathServiceFk.Price >= input.MinPriceFilter)
                .WhereIf(input.MaxPriceFilter != null, e => e.SurpathServiceFk.Price <= input.MaxPriceFilter)
                .WhereIf(input.MinDiscountFilter != null, e => e.SurpathServiceFk.Discount >= input.MinDiscountFilter)
                .WhereIf(input.MaxDiscountFilter != null, e => e.SurpathServiceFk.Discount <= input.MaxDiscountFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.SurpathServiceFk.Description.Contains(input.DescriptionFilter))
                .WhereIf(input.IsEnabledByDefaultFilter.HasValue && input.IsEnabledByDefaultFilter > -1, e => (input.IsEnabledByDefaultFilter == 1 && e.SurpathServiceFk.IsEnabledByDefault) || (input.IsEnabledByDefaultFilter == 0 && !e.SurpathServiceFk.IsEnabledByDefault))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter)
                ;

            var pagedAndFilteredTenantSurpathServices = filteredTenantSurpathServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantSurpathServices = from o in pagedAndFilteredTenantSurpathServices
                                        join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        join o2 in _cohortLookUpRepository.GetAll() on o.CohortId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()

                                        join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                                        from s3 in j3.DefaultIfEmpty()

                                        join o4 in _recordCategoryRuleLookUpRepository.GetAll() on o.RecordCategoryRuleId equals o4.Id into j4
                                        from s4 in j4.DefaultIfEmpty()

                                        select new
                                        {
                                            //spf = o.SurpathServiceFk,
                                            TenantId = o.TenantId,
                                            Name = o.SurpathServiceFk != null ? o.SurpathServiceFk.Name : string.Empty,
                                            Price = o.SurpathServiceFk != null ? o.SurpathServiceFk.Price : 0,
                                            Discount = o.SurpathServiceFk != null ? o.SurpathServiceFk.Discount : 0,
                                            Description = o.SurpathServiceFk != null ? o.SurpathServiceFk.Description : string.Empty,
                                            IsEnabledByDefault = o.SurpathServiceFk != null ? o.SurpathServiceFk.IsEnabledByDefault : false,
                                            IsEnabled = o.IsPricingOverrideEnabled,
                                            o.Id,
                                            TenantDepartmentName = s1 != null ? s1.Name : string.Empty,
                                            CohortName = s2 != null ? s2.Name : string.Empty,
                                            UserName = s3 != null ? s3.Name : string.Empty,
                                            RecordCategoryRuleName = s4 != null ? s4.Name : string.Empty,
                                            TenantSurpathServiceName = o.Name,
                                            TenantSurpathServiceDescription = o.Description
                                        };

            //select new
            //{
            //    Name = o.SurpathServiceFk != null ? o.SurpathServiceFk.Name : string.Empty,
            //    Price = o.SurpathServiceFk != null ? o.SurpathServiceFk.Price : (double?)null,
            //    Discount = o.SurpathServiceFk != null ? o.SurpathServiceFk.Discount : (decimal?)null,
            //    Description = o.SurpathServiceFk != null ? o.SurpathServiceFk.Description : string.Empty,
            //    IsEnabledByDefault = o.SurpathServiceFk != null ? o.SurpathServiceFk.IsEnabledByDefault : (bool?)null,
            //    o.Id,
            //    TenantDepartmentName = s1 != null ? s1.Name : string.Empty,
            //    CohortName = s2 != null ? s2.Name : string.Empty,
            //    UserName = s3 != null ? s3.Name : string.Empty,
            //    RecordCategoryRuleName = s4 != null ? s4.Name : string.Empty
            //};

            var totalCount = await filteredTenantSurpathServices.CountAsync();

            var dbList = await tenantSurpathServices.ToListAsync();
            var results = new List<GetSurpathServiceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSurpathServiceForViewDto()
                {
                    SurpathService = new SurpathServiceDto
                    {
                        Name = o.Name,
                        Price = o.Price,
                        Discount = o.Discount,
                        Description = o.Description,
                        IsEnabledByDefault = o.IsEnabledByDefault,
                        Id = o.Id,
                    },
                    TenantDepartmentName = o.TenantDepartmentName,
                    CohortName = o.CohortName,
                    UserName = o.UserName,
                    RecordCategoryRuleName = o.RecordCategoryRuleName,
                    TenantId = o.TenantId,
                    IsEnabled = o.IsEnabled,
                    Name = o.TenantSurpathServiceName,
                    Description = o.TenantSurpathServiceDescription
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSurpathServiceForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetTenantSurpathServiceForViewDto> GetTenantSurpathServiceForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);

            var output = new GetTenantSurpathServiceForViewDto { TenantSurpathService = ObjectMapper.Map<TenantSurpathServiceDto>(tenantSurpathService) };

            if (output.TenantSurpathService.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.TenantSurpathService.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.TenantSurpathService.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.TenantSurpathService.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantSurpathService.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TenantSurpathService.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task<GetTenantSurpathServiceForEditOutput> GetTenantSurpathServiceForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantSurpathService = await _tenantSurpathServiceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantSurpathServiceForEditOutput { TenantSurpathService = ObjectMapper.Map<CreateOrEditTenantSurpathServiceDto>(tenantSurpathService) };

            if (output.TenantSurpathService.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.TenantSurpathService.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.TenantSurpathService.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.TenantSurpathService.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantSurpathService.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TenantSurpathService.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantSurpathService.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantSurpathServiceDto input)
        {
            // Set tenant context if provided in the input (for host users)
            if (input.TenantId.HasValue && AbpSession.TenantId == null)
            {
                //using (CurrentUnitOfWork.SetTenantId(input.TenantId.Value))
                //{
                await CreateOrEditInternal(input);
                //}
            }
            else
            {
                await CreateOrEditInternal(input);
            }
        }

        private async Task CreateOrEditInternal(CreateOrEditTenantSurpathServiceDto input)
        {
            // we should check to see if this already exists as a requirement, and if so, undelete and update the previous record
            var _t = await _recordRequirementRepository.GetAll().IgnoreQueryFilters().ToListAsync();
            var _existing = await _recordRequirementRepository.GetAll().IgnoreQueryFilters().Where(x => x.SurpathServiceId == input.SurpathServiceId && x.TenantSurpathServiceFk.IsDeleted == false).Include(x => x.TenantSurpathServiceFk).FirstOrDefaultAsync();

            if (_existing != null && _existing.TenantSurpathServiceFk.IsDeleted == true)
            {
                input.Id = _existing.TenantSurpathServiceFk.Id;
            }

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Create)]
        protected virtual async Task Create(CreateOrEditTenantSurpathServiceDto input)
        {
            var tenantSurpathService = ObjectMapper.Map<TenantSurpathService>(input);

            //// Determine the tenant ID
            //if (AbpSession.TenantId != null)
            //{
            //    // Tenant user - use session tenant ID
            //    tenantSurpathService.TenantId = (int?)AbpSession.TenantId;
            //}
            //else if (input.TenantId.HasValue)
            //{
            //    // Host user - use the provided tenant ID
            //    tenantSurpathService.TenantId = input.TenantId.Value;
            //}
            //else
            //{
            //    // Host user without explicit tenant ID - try to get from related entities
            //    if (input.TenantDepartmentId.HasValue)
            //    {
            //        var dept = await _tenantDepartmentLookUpRepository.GetAsync(input.TenantDepartmentId.Value);
            //        tenantSurpathService.TenantId = dept.TenantId;
            //    }
            //    else if (input.CohortId.HasValue)
            //    {
            //        var cohort = await _cohortLookUpRepository.GetAsync(input.CohortId.Value);
            //        tenantSurpathService.TenantId = cohort.TenantId;
            //    }
            //    else if (input.UserId.HasValue)
            //    {
            //        var user = await _userLookUpRepository.GetAsync(input.UserId.Value);
            //        tenantSurpathService.TenantId = user.TenantId;
            //    }
            //    else
            //    {
            //        throw new UserFriendlyException("Unable to determine tenant for pricing creation.");
            //    }
            //}

            // Inherit RecordCategoryRuleId from parent in hierarchy if not explicitly set
            if (!input.RecordCategoryRuleId.HasValue && input.SurpathServiceId.HasValue)
            {
                // First try to get from parent in hierarchy
                Guid? parentRecordCategoryRuleId = null;

                // Determine the tenant ID for the query
                int? tenantId = tenantSurpathService.TenantId ?? AbpSession.TenantId;

                if (tenantId.HasValue)
                {
                    // Build query to find parent service based on hierarchy
                    var parentQuery = _tenantSurpathServiceRepository.GetAll()
                        .Where(ts => ts.TenantId == tenantId.Value && ts.SurpathServiceId == input.SurpathServiceId.Value);

                    TenantSurpathService parentService = null;

                    // If creating a user-level override, look for cohort-level parent
                    if (input.UserId.HasValue && input.CohortId.HasValue)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.CohortId == input.CohortId.Value && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }
                    // If no cohort-level parent and we have a department, look for department-level parent
                    if (parentService == null && input.TenantDepartmentId.HasValue)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.TenantDepartmentId == input.TenantDepartmentId.Value && ts.CohortId == null && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }
                    // If no department-level parent, look for tenant-level parent
                    if (parentService == null)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }

                    if (parentService != null && parentService.RecordCategoryRuleId.HasValue)
                    {
                        parentRecordCategoryRuleId = parentService.RecordCategoryRuleId;
                    }
                }

                // If we found a parent's requirement, use it
                if (parentRecordCategoryRuleId.HasValue)
                {
                    tenantSurpathService.RecordCategoryRuleId = parentRecordCategoryRuleId;
                }
                else
                {
                    // Otherwise fall back to the base SurpathService's requirement
                    var baseSurpathService = await _surpathServiceLookUpRepository.GetAsync(input.SurpathServiceId.Value);
                    if (baseSurpathService?.RecordCategoryRuleId != null)
                    {
                        tenantSurpathService.RecordCategoryRuleId = baseSurpathService.RecordCategoryRuleId;
                    }
                }
            }

            tenantSurpathService.CreationTime = DateTime.UtcNow;
            tenantSurpathService.CreatorUserId = AbpSession.UserId;
            await _tenantSurpathServiceRepository.InsertAsync(tenantSurpathService);

            //// Set the tenant context for the operation
            //using (CurrentUnitOfWork.SetTenantId(tenantSurpathService.TenantId))
            //{
            //}
        }

        //private async Task CreateRequirementIfNotExist(CreateOrEditTenantSurpathServiceDto input)
        //{
        //    var _SurpathServiceId = input.SurpathServiceId;

        //    // we have to create a requirement and a category if they don't exist

        //    var _requirement = _recordRequirementRepository.GetAll().Where(r=>r.SurpathServiceId == input.SurpathServiceId).FirstOrDefault();
        //    if (_requirement != null) return;

        //    var newReq = new RecordRequirement()
        //    {
        //    };

        //}

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        protected virtual async Task Update(CreateOrEditTenantSurpathServiceDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var tenantSurpathService = await _tenantSurpathServiceRepository.FirstOrDefaultAsync((Guid)input.Id);
            tenantSurpathService.LastModifierUserId = AbpSession.UserId;
            tenantSurpathService.LastModificationTime = DateTime.UtcNow;
            ObjectMapper.Map(input, tenantSurpathService);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Delete)]
        public async Task Disable(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _tss = _tenantSurpathServiceRepository.Get(input.Id);
            _tss.IsPricingOverrideEnabled = false;
            await _tenantSurpathServiceRepository.UpdateAsync(_tss);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _tss = _tenantSurpathServiceRepository.Get(input.Id);
            if (_tss != null)
            {
                await ReAssociateToDefaultService(_tss);
            }
            await _tenantSurpathServiceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantSurpathServicesToExcel(GetAllTenantSurpathServicesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredTenantSurpathServices = _tenantSurpathServiceRepository.GetAll()
                        .Include(e => e.SurpathServiceFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsEnabledFilter.HasValue && input.IsEnabledFilter > -1, e => (input.IsEnabledFilter == 1 && e.IsPricingOverrideEnabled) || (input.IsEnabledFilter == 0 && !e.IsPricingOverrideEnabled))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter);

            var query = (from o in filteredTenantSurpathServices
                         join o1 in _surpathServiceLookUpRepository.GetAll() on o.SurpathServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _cohortLookUpRepository.GetAll() on o.CohortId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _userLookUpRepository.GetAll() on o.UserId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _recordCategoryRuleLookUpRepository.GetAll() on o.RecordCategoryRuleId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetTenantSurpathServiceForViewDto()
                         {
                             TenantSurpathService = new TenantSurpathServiceDto
                             {
                                 Name = o.Name,
                                 Price = o.Price,
                                 Description = o.Description,
                                 IsPricingOverrideEnabled = o.IsPricingOverrideEnabled,
                                 Id = o.Id
                             },
                             SurpathServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CohortName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             RecordCategoryRuleName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var tenantSurpathServiceListDtos = await query.ToListAsync();

            return _tenantSurpathServicesExcelExporter.ExportToFile(tenantSurpathServiceListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<PagedResultDto<TenantSurpathServiceSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _surpathServiceLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var surpathServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TenantSurpathServiceSurpathServiceLookupTableDto>();
            foreach (var surpathService in surpathServiceList)
            {
                lookupTableDtoList.Add(new TenantSurpathServiceSurpathServiceLookupTableDto
                {
                    Id = surpathService.Id.ToString(),
                    DisplayName = surpathService.Name?.ToString()
                });
            }

            return new PagedResultDto<TenantSurpathServiceSurpathServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<PagedResultDto<TenantSurpathServiceRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordCategoryRuleLookUpRepository.GetAll()
                .Where(r => r.IsSurpathOnly == true)
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordCategoryRuleList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TenantSurpathServiceRecordCategoryRuleLookupTableDto>();
            foreach (var recordCategoryRule in recordCategoryRuleList)
            {
                lookupTableDtoList.Add(new TenantSurpathServiceRecordCategoryRuleLookupTableDto
                {
                    Id = recordCategoryRule.Id.ToString(),
                    DisplayName = recordCategoryRule.Name?.ToString()
                });
            }

            return new PagedResultDto<TenantSurpathServiceRecordCategoryRuleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<TenantSurpathServiceDto>> GetAllTenantSurpathServiceDtoForTenant(EntityDto input)
        {
            var allservices = await _surpathServiceLookUpRepository.GetAllListAsync();

            var tenantservices = await _tenantSurpathServiceRepository.GetAllListAsync();

            var _tenantservices = new List<TenantSurpathService>();
            foreach (var service in allservices)
            {
                if (!tenantservices.Any(s => s.SurpathServiceId == service.Id))
                {
                    _tenantservices.Add(new TenantSurpathService()
                    {
                        TenantId = input.Id,
                        SurpathServiceId = service.Id,
                        IsPricingOverrideEnabled = true
                    });
                }
            }

            var results = new List<TenantSurpathServiceDto>();
            foreach (var tenantservice in _tenantservices)
            {
                results.Add(new TenantSurpathServiceDto()
                {
                    Id = tenantservice.Id,
                    IsPricingOverrideEnabled = tenantservice.IsPricingOverrideEnabled,
                    SurpathServiceId = tenantservice.SurpathServiceId,
                    SurpathServiceName = allservices.Where(s => s.Id == tenantservice.SurpathServiceId).First().Name
                });
            }
            return results;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<PagedResultDto<TenantSurpathServiceCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            var _isHost = AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host;

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _cohortLookUpRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name != null && e.Name.Contains(input.Filter))
                .WhereIf(input.TenantId > 0, e => e.TenantId == input.TenantId);

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            Dictionary<int, Tuple<int, string, string>> _tenantList = new Dictionary<int, Tuple<int, string, string>>();

            if (_isHost)
            {
                _tenantList = await TenantManager.GetTenancyInfoList();
            }

            var lookupTableDtoList = new List<TenantSurpathServiceCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new TenantSurpathServiceCohortLookupTableDto
                {
                    Id = cohort.Id.ToString(),
                    DisplayName = cohort.Name?.ToString(),
                    TenantInfoDto = (_isHost && cohort.TenantId != null) ? new TenantInfoDto()
                    {
                        Id = _tenantList[(int)cohort.TenantId].Item1,
                        Name = _tenantList[(int)cohort.TenantId].Item2,
                        TenancyName = _tenantList[(int)cohort.TenantId].Item3
                    } : null
                });
            }

            return new PagedResultDto<TenantSurpathServiceCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<PagedResultDto<TenantSurpathServiceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _userLookUpRepository.GetAll().AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                      e => (e.Name != null && e.Name.Contains(input.Filter))
                || (e.MiddleName != null && e.MiddleName.Contains(input.Filter))
                || (e.Surname != null && e.Surname.Contains(input.Filter))
                || (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
                || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
               )
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenantId.ToString()), e => e.TenantId == input.TenantId);

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TenantSurpathServiceUserLookupTableDto>();
            foreach (var user in userList)
            {
                var _displayName = user.Name?.ToString() + ' ' + user.Surname?.ToString();

                lookupTableDtoList.Add(new TenantSurpathServiceUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = _displayName
                });
            }

            return new PagedResultDto<TenantSurpathServiceUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        private async Task ReAssociateToDefaultService(TenantSurpathService tss)
        {
            // clearing tenantsurpathserviceid will dissassociate the requirement from the custom pricing
            var _tenantreq = _recordRequirementRepository.GetAll().Where(r => r.SurpathServiceId == tss.SurpathServiceId).Include(r => r.SurpathServiceFk).ThenInclude(r => r.RecordCategoryRuleFk).FirstOrDefault();
            _tenantreq.TenantSurpathServiceId = Guid.Empty;
            await _recordRequirementRepository.UpdateAsync(_tenantreq);
        }

        public async Task<bool> CloneAsync(EntityDto<Guid> input)
        {
            try
            {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                // Get the original tenant surpath service
                var originalService = await _tenantSurpathServiceRepository.GetAsync(input.Id);

                if (originalService == null)
                    throw new UserFriendlyException("Surpath service not found");

                var _TenantSurpathService = ObjectMapper.Map<TenantSurpathServiceDto>(originalService);
                _TenantSurpathService.Id = Guid.NewGuid();

                var _newTenantSurpathService = ObjectMapper.Map<TenantSurpathService>(_TenantSurpathService);
                _newTenantSurpathService.TenantId = originalService.TenantId;
                _newTenantSurpathService.Name = _newTenantSurpathService.Name += " Clone";
                _newTenantSurpathService.Description = _newTenantSurpathService.Description += " Clone";
                _newTenantSurpathService.IsPricingOverrideEnabled = false;

                // Save the cloned service
                await _tenantSurpathServiceRepository.InsertAsync(_newTenantSurpathService);

                // Trigger necessary events or notifications if needed
                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error cloning TenantSurpathService", ex);
                return false;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_ManageOU)]
        public async Task AssignToOrganizationUnit(Guid id, long organizationUnitId)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);
            var organizationUnit = await _organizationUnitRepository.GetAsync(organizationUnitId);

            if (tenantSurpathService == null || organizationUnit == null)
            {
                throw new UserFriendlyException(L("InvalidAssignment"));
            }

            tenantSurpathService.OrganizationUnitId = organizationUnitId;
            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_ManageCohort)]
        public async Task AssignToCohort(Guid id, Guid cohortId)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);
            var cohort = await _cohortLookUpRepository.GetAsync(cohortId);

            if (tenantSurpathService == null || cohort == null)
            {
                throw new UserFriendlyException(L("InvalidAssignment"));
            }

            tenantSurpathService.CohortId = cohortId;
            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_ManageDepartment)]
        public async Task AssignToTenantDepartment(Guid id, Guid departmentId)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);
            var department = await _tenantDepartmentLookUpRepository.GetAsync(departmentId);

            if (tenantSurpathService == null || department == null)
            {
                throw new UserFriendlyException(L("InvalidAssignment"));
            }

            tenantSurpathService.TenantDepartmentId = departmentId;
            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_AssignToTenant)]
        public async Task AssignToTenant(Guid id)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);

            if (tenantSurpathService == null)
            {
                throw new UserFriendlyException(L("InvalidAssignment"));
            }

            // Clear other assignments
            tenantSurpathService.OrganizationUnitId = null;
            tenantSurpathService.CohortId = null;
            tenantSurpathService.TenantDepartmentId = null;
            tenantSurpathService.UserId = null;
            tenantSurpathService.CohortUserId = null;

            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        public async Task<bool> IsUserPaid(long userId)
        {
            var user = await _userLookUpRepository.GetAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Check tenant level services
            var tenantServices = await _tenantSurpathServiceRepository
                .GetAll()
                .Where(s => s.TenantId == AbpSession.TenantId &&
                            s.IsPricingOverrideEnabled &&
                            s.OrganizationUnitId == null &&
                            s.CohortId == null &&
                            s.TenantDepartmentId == null &&
                            s.UserId == null)
                .ToListAsync();

            if (tenantServices.Any())
            {
                return true;
            }

            // Check organization unit level services
            var userOUs = await _userOrganizationUnitRepository
                .GetAll()
                .Where(uou => uou.UserId == userId)
                .Select(uou => uou.OrganizationUnitId)
                .ToListAsync();

            var ouServices = await _tenantSurpathServiceRepository
                .GetAll()
                .Where(s => s.OrganizationUnitId.HasValue &&
                            userOUs.Contains(s.OrganizationUnitId.Value) &&
                            s.IsPricingOverrideEnabled)
                .ToListAsync();

            if (ouServices.Any())
            {
                return true;
            }

            // Check cohort level services
            var userCohorts = await _cohortUserRepository
                .GetAll()
                .Where(cu => cu.UserId == userId)
                .Select(cu => cu.CohortId)
                .ToListAsync();

            var cohortServices = await _tenantSurpathServiceRepository
                .GetAll()
                .Where(s => s.CohortId.HasValue &&
                            userCohorts.Contains(s.CohortId.Value) &&
                            s.IsPricingOverrideEnabled)
                .ToListAsync();

            if (cohortServices.Any())
            {
                return true;
            }

            // Check department level services
            var userDepartments = await _tenantDepartmentUserRepository
                .GetAll()
                .Where(du => du.UserId == userId)
                .Select(du => du.TenantDepartmentId)
                .ToListAsync();

            var departmentServices = await _tenantSurpathServiceRepository
                .GetAll()
                .Where(s => s.TenantDepartmentId.HasValue &&
                            userDepartments.Contains(s.TenantDepartmentId.Value) &&
                            s.IsPricingOverrideEnabled)
                .ToListAsync();

            if (departmentServices.Any())
            {
                return true;
            }

            // Check user level services
            var userServices = await _tenantSurpathServiceRepository
                .GetAll()
                .Where(s => s.UserId == userId && s.IsPricingOverrideEnabled)
                .ToListAsync();

            return userServices.Any();
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_AssignToTenant)]
        public async Task AssignToCohortUser(Guid id, Guid cohortUserId)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(id);
            var cohortUser = await _cohortUserRepository.GetAsync(cohortUserId);

            if (cohortUser == null)
                throw new UserFriendlyException(L("CohortUserNotFound"));

            if (!await IsUserPaid(cohortUser.UserId))
                throw new UserFriendlyException(L("UserNotPaid"));

            tenantSurpathService.CohortUserId = cohortUserId;
            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        // Hierarchical pricing management methods
        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<HierarchicalPricingDto> GetHierarchicalPricing(GetHierarchicalPricingInput input)
        {
            var result = await _hierarchicalPricingManager.GetHierarchicalPricingAsync(input.TenantId, input.SurpathServiceId);
            return ObjectMapper.Map<HierarchicalPricingDto>(result);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<HierarchicalPricingNodeDto> GetHierarchicalPricingV2(GetHierarchicalPricingInputV2 input)
        {
            var result = await _hierarchicalPricingManager.GetHierarchicalPricingV2Async(input.TenantId, input.SurpathServiceId, input.IncludeDisabled);
            return MapToHierarchicalPricingNodeDto(result);
        }

        private HierarchicalPricingNodeDto MapToHierarchicalPricingNodeDto(HierarchicalPricingNode node)
        {
            if (node == null) return null;

            var dto = new HierarchicalPricingNodeDto
            {
                Id = node.Id,
                NodeType = node.NodeType,
                Name = node.Name,
                Description = node.Description,
                Surname = node.Surname,
                UserName = node.UserName,
                EmailAddress = node.EmailAddress,
                  Services = node.Services?.Select(s => new TenantSurpathServiceDto
                  {
                      Id = s.Id,
                      Name = s.Name,
                      Price = s.Price,
                      Description = s.Description,
                      IsPricingOverrideEnabled = s.IsEnabled,
                      IsInvoiced = s.IsInvoiced,
                      SurpathServiceId = s.SurpathServiceId,
                      TenantDepartmentId = s.TenantDepartmentId,
                      CohortId = s.CohortId,
                      UserId = s.UserId,
                      BasePrice = s.BasePrice,
                      AmountDue = s.IsEnabled && !s.IsInvoiced ? s.Price : 0
                  }).ToList() ?? new List<TenantSurpathServiceDto>(),
                Children = node.Children?.Select(MapToHierarchicalPricingNodeDto).ToList() ?? new List<HierarchicalPricingNodeDto>()
            };

            return dto;
        }

        // Original implementation kept for reference - TO BE REMOVED
        private async Task<HierarchicalPricingDto> GetHierarchicalPricingOld(GetHierarchicalPricingInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var result = new HierarchicalPricingDto();

                // Get all surpath services
                var surpathServices = await _surpathServiceLookUpRepository.GetAll()
                    .Where(s => !input.SurpathServiceId.HasValue || s.Id == input.SurpathServiceId.Value)
                    .ToListAsync();

                // Get tenant info
                var tenant = await TenantManager.GetByIdAsync(input.TenantId);
                result.Tenant = new TenantPricingDto
                {
                    Id = tenant.Id,
                    Name = tenant.Name
                };

                // Get all tenant surpath services for the tenant
                var tenantServices = await _tenantSurpathServiceRepository.GetAll()
                    .Where(ts => ts.TenantId == input.TenantId)
                    //.Where(ts => !input.SurpathServiceId.HasValue || ts.SurpathServiceId == input.SurpathServiceId.Value)
                    .ToListAsync();

                // Get tenant-level services (no specific assignment)
                var tenantLevelServices = tenantServices
                    .Where(ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                    .ToList();

                // Build tenant services list
                foreach (var service in surpathServices)
                {
                    var tenantService = tenantLevelServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                    result.Tenant.Services.Add(new ServicePriceDto
                    {
                        ServiceId = service.Id,
                        ServiceName = service.Name,
                        ServiceDescription = service.Description,
                        BasePrice = service.Price,
                        OverridePrice = tenantService?.Price,
                        EffectivePrice = tenantService?.Price ?? service.Price,
                        IsInherited = tenantService == null,
                        TenantSurpathServiceId = tenantService?.Id,
                        IsEnabled = tenantService?.IsPricingOverrideEnabled ?? service.IsEnabledByDefault
                    });
                }

                // Get all departments for the tenant
                var departments = await _tenantDepartmentLookUpRepository.GetAll()
                    .Where(d => d.TenantId == input.TenantId && d.Active)
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                // Process each department
                foreach (var dept in departments)
                {
                    var deptPricing = ObjectMapper.Map<DepartmentPricingDto>(dept);

                    // Get department-level services
                    var deptServices = tenantServices
                        .Where(ts => ts.TenantDepartmentId == dept.Id && ts.CohortId == null && ts.UserId == null)
                        .ToList();

                    // Build department services list
                    foreach (var service in surpathServices)
                    {
                        var deptService = deptServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                        var tenantPrice = result.Tenant.Services.FirstOrDefault(s => s.ServiceId == service.Id);

                        deptPricing.Services.Add(new ServicePriceDto
                        {
                            ServiceId = service.Id,
                            ServiceName = service.Name,
                            ServiceDescription = service.Description,
                            BasePrice = service.Price,
                            OverridePrice = deptService?.Price,
                            EffectivePrice = deptService?.Price ?? tenantPrice?.EffectivePrice ?? service.Price,
                            IsInherited = deptService == null,
                            TenantSurpathServiceId = deptService?.Id,
                            IsEnabled = deptService?.IsPricingOverrideEnabled ?? tenantPrice?.IsEnabled ?? service.IsEnabledByDefault
                        });
                    }

                    // Get cohorts for the department
                    var cohorts = await _cohortLookUpRepository.GetAll()
                        .Where(c => c.TenantDepartmentId == dept.Id)
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                    // Process each cohort
                    foreach (var cohort in cohorts)
                    {
                        var cohortPricing = ObjectMapper.Map<CohortPricingDto>(cohort);

                        // Get cohort-level services
                        var cohortServices = tenantServices
                            .Where(ts => ts.CohortId == cohort.Id && ts.UserId == null)
                            .ToList();

                        // Build cohort services list
                        foreach (var service in surpathServices)
                        {
                            var cohortService = cohortServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                            var deptPrice = deptPricing.Services.FirstOrDefault(s => s.ServiceId == service.Id);

                            cohortPricing.Services.Add(new ServicePriceDto
                            {
                                ServiceId = service.Id,
                                ServiceName = service.Name,
                                ServiceDescription = service.Description,
                                BasePrice = service.Price,
                                OverridePrice = cohortService?.Price,
                                EffectivePrice = cohortService?.Price ?? deptPrice?.EffectivePrice ?? service.Price,
                                IsInherited = cohortService == null,
                                TenantSurpathServiceId = cohortService?.Id,
                                IsEnabled = cohortService?.IsPricingOverrideEnabled ?? deptPrice?.IsEnabled ?? service.IsEnabledByDefault
                            });
                        }

                        // Get users in the cohort
                        var cohortUsers = await _cohortUserRepository.GetAll()
                            .Include(cu => cu.UserFk)
                            .Where(cu => cu.CohortId == cohort.Id)
                            .OrderBy(cu => cu.UserFk.Name)
                            .ThenBy(cu => cu.UserFk.Surname)
                            .ToListAsync();

                        // Process each user
                        foreach (var cohortUser in cohortUsers)
                        {
                            var userPricing = new UserPricingDto
                            {
                                Id = cohortUser.UserId,
                                Name = cohortUser.UserFk.Name,
                                Surname = cohortUser.UserFk.Surname,
                                UserName = cohortUser.UserFk.UserName,
                                EmailAddress = cohortUser.UserFk.EmailAddress
                            };

                            // Get user-level services
                            var userServices = tenantServices
                                .Where(ts => ts.UserId == cohortUser.UserId)
                                .ToList();

                            // Build user services list
                            foreach (var service in surpathServices)
                            {
                                var userService = userServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                                var cohortPrice = cohortPricing.Services.FirstOrDefault(s => s.ServiceId == service.Id);

                                userPricing.Services.Add(new ServicePriceDto
                                {
                                    ServiceId = service.Id,
                                    ServiceName = service.Name,
                                    ServiceDescription = service.Description,
                                    BasePrice = service.Price,
                                    OverridePrice = userService?.Price,
                                    EffectivePrice = userService?.Price ?? cohortPrice?.EffectivePrice ?? service.Price,
                                    IsInherited = userService == null,
                                    TenantSurpathServiceId = userService?.Id,
                                    IsEnabled = userService?.IsPricingOverrideEnabled ?? cohortPrice?.IsEnabled ?? service.IsEnabledByDefault
                                });
                            }

                            cohortPricing.Users.Add(userPricing);
                        }

                        deptPricing.Cohorts.Add(cohortPricing);
                    }

                    result.Departments.Add(deptPricing);
                }

                // Get all cohorts that don't belong to any department (standalone cohorts)
                var standaloneCohorts = await _cohortLookUpRepository.GetAll()
                    .Where(c => c.TenantId == input.TenantId && c.TenantDepartmentId == null)
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                // Process standalone cohorts (same logic as department cohorts)
                foreach (var cohort in standaloneCohorts)
                {
                    var cohortPricing = ObjectMapper.Map<CohortPricingDto>(cohort);

                    // Get cohort-level services
                    var cohortServices = tenantServices
                        .Where(ts => ts.CohortId == cohort.Id && ts.UserId == null)
                        .ToList();

                    // Build cohort services list
                    foreach (var service in surpathServices)
                    {
                        var cohortService = cohortServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                        var tenantPrice = result.Tenant.Services.FirstOrDefault(s => s.ServiceId == service.Id);

                        cohortPricing.Services.Add(new ServicePriceDto
                        {
                            ServiceId = service.Id,
                            ServiceName = service.Name,
                            ServiceDescription = service.Description,
                            BasePrice = service.Price,
                            OverridePrice = cohortService?.Price,
                            EffectivePrice = cohortService?.Price ?? tenantPrice?.EffectivePrice ?? service.Price,
                            IsInherited = cohortService == null,
                            TenantSurpathServiceId = cohortService?.Id,
                            IsEnabled = cohortService?.IsPricingOverrideEnabled ?? tenantPrice?.IsEnabled ?? service.IsEnabledByDefault
                        });
                    }

                    // Get users in the cohort
                    var cohortUsers = await _cohortUserRepository.GetAll()
                        .Include(cu => cu.UserFk)
                        .Where(cu => cu.CohortId == cohort.Id)
                        .OrderBy(cu => cu.UserFk.Name)
                        .ThenBy(cu => cu.UserFk.Surname)
                        .ToListAsync();

                    // Process each user
                    foreach (var cohortUser in cohortUsers)
                    {
                        var userPricing = new UserPricingDto
                        {
                            Id = cohortUser.UserId,
                            Name = cohortUser.UserFk.Name,
                            Surname = cohortUser.UserFk.Surname,
                            UserName = cohortUser.UserFk.UserName,
                            EmailAddress = cohortUser.UserFk.EmailAddress
                        };

                        // Get user-level services
                        var userServices = tenantServices
                            .Where(ts => ts.UserId == cohortUser.UserId)
                            .ToList();

                        // Build user services list
                        foreach (var service in surpathServices)
                        {
                            var userService = userServices.FirstOrDefault(ts => ts.SurpathServiceId == service.Id);
                            var cohortPrice = cohortPricing.Services.FirstOrDefault(s => s.ServiceId == service.Id);

                            userPricing.Services.Add(new ServicePriceDto
                            {
                                ServiceId = service.Id,
                                ServiceName = service.Name,
                                ServiceDescription = service.Description,
                                BasePrice = service.Price,
                                OverridePrice = userService?.Price,
                                EffectivePrice = userService?.Price ?? cohortPrice?.EffectivePrice ?? service.Price,
                                IsInherited = userService == null,
                                TenantSurpathServiceId = userService?.Id,
                                IsEnabled = userService?.IsPricingOverrideEnabled ?? cohortPrice?.IsEnabled ?? service.IsEnabledByDefault
                            });
                        }

                        cohortPricing.Users.Add(userPricing);
                    }

                    // Add standalone cohorts directly to result (not under departments)
                    result.Cohorts.Add(cohortPricing);
                }

                return result;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task BatchUpdatePrices(BatchUpdatePriceDto input)
        {
            foreach (var update in input.Updates)
            {
                if (update.Id.HasValue)
                {
                    // Update existing record
                    var existing = await _tenantSurpathServiceRepository.GetAsync(update.Id.Value);

                    if (update.Price.HasValue)
                    {
                        existing.Price = update.Price.Value;
                        existing.IsPricingOverrideEnabled = update.IsEnabled;
                        await _tenantSurpathServiceRepository.UpdateAsync(existing);
                    }
                    else
                    {
                        // If price is null, delete the override to inherit from parent
                        await _tenantSurpathServiceRepository.DeleteAsync(existing);
                    }
                }
                else if (update.Price.HasValue)
                {
                    // Create new record only if price is provided
                    var surpathService = await _surpathServiceLookUpRepository.GetAsync(update.SurpathServiceId);

                    var newService = new TenantSurpathService
                    {
                        TenantId = AbpSession.TenantId ?? throw new UserFriendlyException(L("TenantIdRequired")),
                        SurpathServiceId = update.SurpathServiceId,
                        Name = surpathService.Name,
                        Price = update.Price.Value,
                        Description = surpathService.Description,
                        IsPricingOverrideEnabled = update.IsEnabled,
                        TenantDepartmentId = update.TenantDepartmentId,
                        CohortId = update.CohortId,
                        UserId = update.UserId
                    };

                    // Inherit RecordCategoryRuleId from parent in hierarchy
                    var parentQuery = _tenantSurpathServiceRepository.GetAll()
                        .Where(ts => ts.TenantId == newService.TenantId && ts.SurpathServiceId == update.SurpathServiceId);

                    TenantSurpathService parentService = null;

                    // If creating a user-level override, look for cohort-level parent
                    if (update.UserId.HasValue && update.CohortId.HasValue)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.CohortId == update.CohortId.Value && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }
                    // If no cohort-level parent and we have a department, look for department-level parent
                    if (parentService == null && update.TenantDepartmentId.HasValue)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.TenantDepartmentId == update.TenantDepartmentId.Value && ts.CohortId == null && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }
                    // If no department-level parent, look for tenant-level parent
                    if (parentService == null)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }

                    if (parentService != null && parentService.RecordCategoryRuleId.HasValue)
                    {
                        newService.RecordCategoryRuleId = parentService.RecordCategoryRuleId;
                    }
                    else if (surpathService.RecordCategoryRuleId.HasValue)
                    {
                        // Fall back to base service's requirement
                        newService.RecordCategoryRuleId = surpathService.RecordCategoryRuleId;
                    }

                    await _tenantSurpathServiceRepository.InsertAsync(newService);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task SetAllServicesPrice(SetAllServicesPriceDto input)
        {
            // Get all surpath services
            var surpathServices = await _surpathServiceLookUpRepository.GetAllListAsync();

            // Parse target IDs outside of LINQ expression
            Guid? departmentId = null;
            Guid? cohortId = null;
            long? userId = null;

            if (input.TargetType == "department" && Guid.TryParse(input.TargetId, out var deptId))
                departmentId = deptId;
            else if (input.TargetType == "cohort" && Guid.TryParse(input.TargetId, out var cohId))
                cohortId = cohId;
            else if (input.TargetType == "user" && long.TryParse(input.TargetId, out var uid))
                userId = uid;

            // Delete existing overrides at the specified level
            var existingServices = await _tenantSurpathServiceRepository.GetAll()
                .Where(ts => ts.TenantId == input.TenantId)
                .WhereIf(input.TargetType == "tenant",
                    ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                .WhereIf(input.TargetType == "department" && departmentId.HasValue,
                    ts => ts.TenantDepartmentId == departmentId.Value)
                .WhereIf(input.TargetType == "cohort" && cohortId.HasValue,
                    ts => ts.CohortId == cohortId.Value)
                .WhereIf(input.TargetType == "user" && userId.HasValue,
                    ts => ts.UserId == userId.Value)
                .ToListAsync();

            // Delete all existing services at this level
            foreach (var existing in existingServices)
            {
                await _tenantSurpathServiceRepository.DeleteAsync(existing);
            }

            // Create new services with the specified price
            foreach (var surpathService in surpathServices)
            {
                var newService = new TenantSurpathService
                {
                    TenantId = input.TenantId,
                    SurpathServiceId = surpathService.Id,
                    Name = surpathService.Name,
                    Price = input.Price,
                    Description = surpathService.Description,
                    IsPricingOverrideEnabled = true
                };

                // Set the appropriate level
                switch (input.TargetType)
                {
                    case "department":
                        newService.TenantDepartmentId = Guid.Parse(input.TargetId);
                        break;

                    case "cohort":
                        newService.CohortId = Guid.Parse(input.TargetId);
                        break;

                    case "user":
                        newService.UserId = long.Parse(input.TargetId);
                        break;
                }

                // Inherit RecordCategoryRuleId from parent in hierarchy
                var parentQuery = _tenantSurpathServiceRepository.GetAll()
                    .Where(ts => ts.TenantId == input.TenantId && ts.SurpathServiceId == surpathService.Id);

                TenantSurpathService parentService = null;

                // Look for parent based on target type
                if (input.TargetType == "user" && cohortId.HasValue)
                {
                    // For user level, look for cohort parent
                    parentService = await parentQuery
                        .Where(ts => ts.CohortId == cohortId.Value && ts.UserId == null)
                        .FirstOrDefaultAsync();
                }
                else if ((input.TargetType == "cohort" || input.TargetType == "user") && departmentId.HasValue)
                {
                    // For cohort or user level, look for department parent if no cohort parent
                    if (parentService == null)
                    {
                        parentService = await parentQuery
                            .Where(ts => ts.TenantDepartmentId == departmentId.Value && ts.CohortId == null && ts.UserId == null)
                            .FirstOrDefaultAsync();
                    }
                }

                // If no specific parent found, look for tenant-level parent
                if (parentService == null && input.TargetType != "tenant")
                {
                    parentService = await parentQuery
                        .Where(ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                        .FirstOrDefaultAsync();
                }

                if (parentService != null && parentService.RecordCategoryRuleId.HasValue)
                {
                    newService.RecordCategoryRuleId = parentService.RecordCategoryRuleId;
                }
                else if (surpathService.RecordCategoryRuleId.HasValue)
                {
                    // Fall back to base service's requirement
                    newService.RecordCategoryRuleId = surpathService.RecordCategoryRuleId;
                }

                await _tenantSurpathServiceRepository.InsertAsync(newService);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<List<SurpathServiceDto>> GetAvailableSurpathServices()
        {
            var services = await _surpathServiceLookUpRepository.GetAll()
                .OrderBy(s => s.Name)
                .ToListAsync();

            return ObjectMapper.Map<List<SurpathServiceDto>>(services);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task ToggleEnabled(ToggleEnabledDto input)
        {
            var tenantSurpathService = await _tenantSurpathServiceRepository.GetAsync(input.Id);
            tenantSurpathService.IsPricingOverrideEnabled = input.IsEnabled;
            await _tenantSurpathServiceRepository.UpdateAsync(tenantSurpathService);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task UpdateServicePrice(UpdateServicePriceDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // Parse target IDs based on level
            Guid? departmentId = null;
            Guid? cohortId = null;
            long? userId = null;

            switch (input.Level.ToLower())
            {
                case "department":
                    if (Guid.TryParse(input.TargetId, out var deptId))
                        departmentId = deptId;
                    break;

                case "cohort":
                    if (Guid.TryParse(input.TargetId, out var cohId))
                        cohortId = cohId;
                    break;

                case "user":
                    if (long.TryParse(input.TargetId, out var uid))
                        userId = uid;
                    break;
            }

            // Find existing record
            var existingService = await _tenantSurpathServiceRepository.GetAll()
                .Where(ts => ts.TenantId == input.TenantId &&
                           ts.SurpathServiceId == input.SurpathServiceId)
                .WhereIf(input.Level == "tenant",
                    ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                .WhereIf(input.Level == "department" && departmentId.HasValue,
                    ts => ts.TenantDepartmentId == departmentId.Value && ts.CohortId == null && ts.UserId == null)
                .WhereIf(input.Level == "cohort" && cohortId.HasValue,
                    ts => ts.CohortId == cohortId.Value && ts.UserId == null)
                .WhereIf(input.Level == "user" && userId.HasValue,
                    ts => ts.UserId == userId.Value)
                .FirstOrDefaultAsync();

            if (existingService != null)
            {
                // Update existing record
                if (input.Price.HasValue)
                {
                    existingService.Price = input.Price.Value;
                    existingService.IsInvoiced = input.IsInvoiced;
                    existingService.IsPricingOverrideEnabled = true;
                    existingService.LastModifierUserId = AbpSession.UserId;
                    existingService.LastModificationTime = DateTime.UtcNow;
                    await _tenantSurpathServiceRepository.UpdateAsync(existingService);
                }
                else
                {
                    // If price is null, delete the override to inherit from parent
                    await _tenantSurpathServiceRepository.DeleteAsync(existingService);
                }
            }
            else if (input.Price.HasValue)
            {
                // Create new record
                var surpathService = await _surpathServiceLookUpRepository.GetAsync(input.SurpathServiceId);

                var newService = new TenantSurpathService
                {
                    TenantId = input.TenantId,
                    SurpathServiceId = input.SurpathServiceId,
                    Name = surpathService.Name,
                    Price = input.Price.Value,
                    Description = surpathService.Description,
                    IsPricingOverrideEnabled = true,
                    IsInvoiced = input.IsInvoiced,
                    TenantDepartmentId = departmentId,
                    CohortId = cohortId,
                    UserId = userId,
                    CreationTime = DateTime.UtcNow,
                    CreatorUserId = AbpSession.UserId
                };

                // Inherit RecordCategoryRuleId from parent in hierarchy
                await InheritRecordCategoryRule(newService, surpathService);

                await _tenantSurpathServiceRepository.InsertAsync(newService);
            }
        }

        private async Task InheritRecordCategoryRule(TenantSurpathService newService, SurpathService baseSurpathService)
        {
            var parentQuery = _tenantSurpathServiceRepository.GetAll()
                .Where(ts => ts.TenantId == newService.TenantId && ts.SurpathServiceId == newService.SurpathServiceId);

            TenantSurpathService parentService = null;

            // If creating a user-level override, look for cohort-level parent
            if (newService.UserId.HasValue && newService.CohortId.HasValue)
            {
                parentService = await parentQuery
                    .Where(ts => ts.CohortId == newService.CohortId.Value && ts.UserId == null)
                    .FirstOrDefaultAsync();
            }
            // If no cohort-level parent and we have a department, look for department-level parent
            if (parentService == null && newService.TenantDepartmentId.HasValue)
            {
                parentService = await parentQuery
                    .Where(ts => ts.TenantDepartmentId == newService.TenantDepartmentId.Value && ts.CohortId == null && ts.UserId == null)
                    .FirstOrDefaultAsync();
            }
            // If no department-level parent, look for tenant-level parent
            if (parentService == null)
            {
                parentService = await parentQuery
                    .Where(ts => ts.TenantDepartmentId == null && ts.CohortId == null && ts.UserId == null)
                    .FirstOrDefaultAsync();
            }

            if (parentService != null && parentService.RecordCategoryRuleId.HasValue)
            {
                newService.RecordCategoryRuleId = parentService.RecordCategoryRuleId;
            }
            else if (baseSurpathService.RecordCategoryRuleId.HasValue)
            {
                // Fall back to base service's requirement
                newService.RecordCategoryRuleId = baseSurpathService.RecordCategoryRuleId;
            }
        }
    }
}
