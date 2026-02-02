using inzibackend.Surpath;
using inzibackend.Surpath;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;

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
using inzibackend.MultiTenancy.Dto;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_SurpathServices)]
    public class SurpathServicesAppService : inzibackendAppServiceBase, ISurpathServicesAppService
    {
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly ISurpathServicesExcelExporter _surpathServicesExcelExporter;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IRepository<Cohort, Guid> _cohortLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleLoookUpRepository;

        public SurpathServicesAppService(IRepository<SurpathService, Guid> surpathServiceRepository, ISurpathServicesExcelExporter surpathServicesExcelExporter, IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository, IRepository<Cohort, Guid> lookup_cohortRepository, IRepository<User, long> lookup_userRepository, IRepository<RecordCategoryRule, Guid> lookup_recordCategoryRuleRepository)
        {
            _surpathServiceRepository = surpathServiceRepository;
            _surpathServicesExcelExporter = surpathServicesExcelExporter;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;
            _cohortLookUpRepository = lookup_cohortRepository;
            _userLookUpRepository = lookup_userRepository;
            _recordCategoryRuleLoookUpRepository = lookup_recordCategoryRuleRepository;

        }

        public async Task<PagedResultDto<GetSurpathServiceForViewDto>> GetAll(GetAllSurpathServicesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredSurpathServices = _surpathServiceRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .WhereIf(input.TenantId !=null, e => e.TenantId == input.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.Discount >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.Discount <= input.MaxDiscountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsEnabledByDefaultFilter.HasValue && input.IsEnabledByDefaultFilter > -1, e => (input.IsEnabledByDefaultFilter == 1 && e.IsEnabledByDefault) || (input.IsEnabledByDefaultFilter == 0 && !e.IsEnabledByDefault))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter);

            var pagedAndFilteredSurpathServices = filteredSurpathServices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var surpathServices = from o in pagedAndFilteredSurpathServices
                                  join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _cohortLookUpRepository.GetAll() on o.CohortId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  join o4 in _recordCategoryRuleLoookUpRepository.GetAll() on o.RecordCategoryRuleId equals o4.Id into j4
                                  from s4 in j4.DefaultIfEmpty()

                                  select new
                                  {

                                      o.Name,
                                      o.Price,
                                      o.Discount,
                                      o.Description,
                                      o.IsEnabledByDefault,
                                      Id = o.Id,
                                      TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      CohortName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                      RecordCategoryRuleName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                  };

            var totalCount = await filteredSurpathServices.CountAsync();

            var dbList = await surpathServices.ToListAsync();
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
                    RecordCategoryRuleName = o.RecordCategoryRuleName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSurpathServiceForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetSurpathServiceForViewDto> GetSurpathServiceForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var surpathService = await _surpathServiceRepository.GetAsync(id);

            var output = new GetSurpathServiceForViewDto { SurpathService = ObjectMapper.Map<SurpathServiceDto>(surpathService) };

            if (output.SurpathService.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.SurpathService.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.SurpathService.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.SurpathService.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.SurpathService.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLoookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SurpathServices_Edit)]
        public async Task<GetSurpathServiceForEditOutput> GetSurpathServiceForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var surpathService = await _surpathServiceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSurpathServiceForEditOutput { SurpathService = ObjectMapper.Map<CreateOrEditSurpathServiceDto>(surpathService) };

            if (output.SurpathService.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.SurpathService.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.SurpathService.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.SurpathService.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.SurpathService.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLoookUpRepository.FirstOrDefaultAsync((Guid)output.SurpathService.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSurpathServiceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SurpathServices_Create)]
        protected virtual async Task Create(CreateOrEditSurpathServiceDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var surpathService = ObjectMapper.Map<SurpathService>(input);

            if (AbpSession.TenantId != null)
            {
                surpathService.TenantId = (int?)AbpSession.TenantId;
            }

            await _surpathServiceRepository.InsertAsync(surpathService);

        }

        [AbpAuthorize(AppPermissions.Pages_SurpathServices_Edit)]
        protected virtual async Task Update(CreateOrEditSurpathServiceDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var surpathService = await _surpathServiceRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, surpathService);

        }

        [AbpAuthorize(AppPermissions.Pages_SurpathServices_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _surpathServiceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSurpathServicesToExcel(GetAllSurpathServicesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredSurpathServices = _surpathServiceRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.Discount >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.Discount <= input.MaxDiscountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsEnabledByDefaultFilter.HasValue && input.IsEnabledByDefaultFilter > -1, e => (input.IsEnabledByDefaultFilter == 1 && e.IsEnabledByDefault) || (input.IsEnabledByDefaultFilter == 0 && !e.IsEnabledByDefault))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter);

            var query = (from o in filteredSurpathServices
                         join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _cohortLookUpRepository.GetAll() on o.CohortId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _userLookUpRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _recordCategoryRuleLoookUpRepository.GetAll() on o.RecordCategoryRuleId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetSurpathServiceForViewDto()
                         {
                             SurpathService = new SurpathServiceDto
                             {
                                 Name = o.Name,
                                 Price = o.Price,
                                 Discount = o.Discount,
                                 Description = o.Description,
                                 IsEnabledByDefault = o.IsEnabledByDefault,
                                 Id = o.Id
                             },
                             TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CohortName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RecordCategoryRuleName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var surpathServiceListDtos = await query.ToListAsync();

            return _surpathServicesExcelExporter.ExportToFile(surpathServiceListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_SurpathServices)]
        public async Task<PagedResultDto<SurpathServiceCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            var _isHost = AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host;

            if (_isHost) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _cohortLookUpRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),e => e.Name != null && e.Name.Contains(input.Filter))
                .WhereIf(input.TenantId > 0, e => e.TenantId == input.TenantId);

            Dictionary<int, Tuple<int, string, string>> _tenantList = new Dictionary<int, Tuple<int, string, string>>();

            if (_isHost)
            {
                _tenantList = await TenantManager.GetTenancyInfoList();
            }

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SurpathServiceCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new SurpathServiceCohortLookupTableDto
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

            return new PagedResultDto<SurpathServiceCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_SurpathServices)]
        public async Task<PagedResultDto<SurpathServiceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
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

            var lookupTableDtoList = new List<SurpathServiceUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new SurpathServiceUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<SurpathServiceUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_SurpathServices)]
        public async Task<PagedResultDto<SurpathServiceRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordCategoryRuleLoookUpRepository.GetAll()
                .Where(e => e.IsSurpathOnly == true)
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordCategoryRuleList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SurpathServiceRecordCategoryRuleLookupTableDto>();
            foreach (var recordCategoryRule in recordCategoryRuleList)
            {
                lookupTableDtoList.Add(new SurpathServiceRecordCategoryRuleLookupTableDto
                {
                    Id = recordCategoryRule.Id.ToString(),
                    DisplayName = recordCategoryRule.Name?.ToString()
                });
            }

            return new PagedResultDto<SurpathServiceRecordCategoryRuleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}