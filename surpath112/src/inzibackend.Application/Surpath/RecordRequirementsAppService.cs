using inzibackend.Surpath;
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
using NPOI.OpenXmlFormats;
using inzibackend.MultiTenancy;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements)]
    public class RecordRequirementsAppService : inzibackendAppServiceBase, IRecordRequirementsAppService
    {
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRecordRequirementsExcelExporter _recordRequirementsExcelExporter;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IRepository<Cohort, Guid> _cohortLookUpRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceLookUpRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;

        private readonly IRepository<Tenant> _tenantRepository;


        public RecordRequirementsAppService(
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRecordRequirementsExcelExporter recordRequirementsExcelExporter,
            IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository,
            IRepository<Cohort, Guid> lookup_cohortRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<TenantSurpathService, Guid> lookup_tenantSurpathServiceRepository,
            IRepository<RecordCategory, Guid> recordCategoryRepository)
        {
            _recordRequirementRepository = recordRequirementRepository;
            _recordRequirementsExcelExporter = recordRequirementsExcelExporter;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;
            _cohortLookUpRepository = lookup_cohortRepository;
            _tenantRepository = tenantRepository;
            //_lookup_surpathServiceRepository = lookup_surpathServiceRepository;
            _tenantSurpathServiceLookUpRepository = lookup_tenantSurpathServiceRepository;
            _recordCategoryRepository = recordCategoryRepository;
        }

        public async Task<PagedResultDto<GetRecordRequirementForViewDto>> GetAll(GetAllRecordRequirementsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredRecordRequirements = _recordRequirementRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.SurpathServiceFk)
                        .Where(e => e.SurpathServiceId == null)
                        //.Include(e => e.TenantSurpathServiceFk)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Metadata.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetadataFilter), e => e.Metadata.Contains(input.MetadataFilter))
                        .WhereIf(input.IsSurpathOnlyFilter.HasValue && input.IsSurpathOnlyFilter > -1, e => (input.IsSurpathOnlyFilter == 1 && e.IsSurpathOnly) || (input.IsSurpathOnlyFilter == 0 && !e.IsSurpathOnly))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter);



            var recordRequirements = from o in filteredRecordRequirements
                                     join o1 in _tenantDepartmentLookUpRepository.GetAll().AsNoTracking() on o.TenantDepartmentId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _cohortLookUpRepository.GetAll().AsNoTracking() on o.CohortId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _tenantSurpathServiceLookUpRepository.GetAll().AsNoTracking().Include(r => r.SurpathServiceFk) on o.SurpathServiceId equals o3.SurpathServiceId into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     join tenantTable in _tenantRepository.GetAll().AsNoTracking().Where(_t => _t.Id == AbpSession.TenantId || AbpSession.TenantId == null) on o    .TenantId equals tenantTable.Id into tJoin
                                     from t in tJoin.DefaultIfEmpty()
                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         o.Metadata,
                                         o.IsSurpathOnly,
                                         Id = o.Id,
                                         TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                         CohortName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                         SurpathServiceName = s3 == null || s3.SurpathServiceFk.Name == null ? "" : s3.SurpathServiceFk.Name.ToString()
                                         //TenantSurpathServiceName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                         ,
                                         TenantName = t == null || t.Name == null ? "was null" : t.Name.ToString()
                                     };

            var pagedAndFilteredRecordRequirements = recordRequirements
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await pagedAndFilteredRecordRequirements.CountAsync();

            var dbList = await pagedAndFilteredRecordRequirements.ToListAsync();
            var results = new List<GetRecordRequirementForViewDto>();

            var _cats = await _recordCategoryRepository.GetAll().IgnoreQueryFilters().AsNoTracking().Where(x => x.IsDeleted == false).Select(x => new { x.Id, x.RecordRequirementId, x.RecordCategoryRuleId }).ToListAsync();

            foreach (var o in dbList)
            {

                var res = new GetRecordRequirementForViewDto()
                {
                    RecordRequirement = new RecordRequirementDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Metadata = o.Metadata,
                        IsSurpathOnly = o.IsSurpathOnly,
                        Id = o.Id,
                        CategoryDTOs = _cats.Where(x => x.RecordRequirementId == o.Id).Select(x => new RecordCategoryDto { Id = x.Id, RecordRequirementId = x.RecordRequirementId, RecordCategoryRuleId = x.RecordCategoryRuleId }).ToList()
                    },
                    TenantDepartmentName = o.TenantDepartmentName,
                    CohortName = o.CohortName,
                    SurpathServiceName = o.SurpathServiceName,
                    TenantName = o.TenantName

                    //,TenantSurpathServiceName = o.TenantSurpathServiceName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRecordRequirementForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRecordRequirementForViewDto> GetRecordRequirementForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordRequirement = await _recordRequirementRepository.GetAsync(id);

            var output = new GetRecordRequirementForViewDto { RecordRequirement = ObjectMapper.Map<RecordRequirementDto>(recordRequirement) };

            if (output.RecordRequirement.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.RecordRequirement.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.RecordRequirement.SurpathServiceId != null)
            {
                var _lookupSurpathService = _tenantSurpathServiceLookUpRepository.GetAll().Include(s => s.SurpathServiceFk).Where(s => s.Id == (Guid)output.RecordRequirement.SurpathServiceId).FirstOrDefault();
                output.SurpathServiceName = _lookupSurpathService?.SurpathServiceFk.Name?.ToString();
            }

            if (output.RecordRequirement.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Edit)]
        public async Task<GetRecordRequirementForEditOutput> GetRecordRequirementForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordRequirement = await _recordRequirementRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecordRequirementForEditOutput { RecordRequirement = ObjectMapper.Map<CreateOrEditRecordRequirementDto>(recordRequirement) };

            if (output.RecordRequirement.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            if (output.RecordRequirement.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            if (output.RecordRequirement.SurpathServiceId != null)
            {
                var _lookupSurpathService = _tenantSurpathServiceLookUpRepository.GetAll().Include(r => r.SurpathServiceFk).Where(r => r.Id == (Guid)output.RecordRequirement.SurpathServiceId).FirstOrDefault();
                output.SurpathServiceName = _lookupSurpathService?.SurpathServiceFk.Name?.ToString();

            }

            if (output.RecordRequirement.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordRequirement.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRecordRequirementDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Create)]
        protected virtual async Task Create(CreateOrEditRecordRequirementDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordRequirement = ObjectMapper.Map<RecordRequirement>(input);

            if (AbpSession.TenantId != null)
            {
                recordRequirement.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordRequirementRepository.InsertAsync(recordRequirement);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Edit)]
        protected virtual async Task Update(CreateOrEditRecordRequirementDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var recordRequirement = await _recordRequirementRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordRequirement);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _recordRequirementRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetRecordRequirementsToExcel(GetAllRecordRequirementsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredRecordRequirements = _recordRequirementRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                        .Include(e => e.CohortFk)
                        .Include(e => e.SurpathServiceFk)
                        .Include(e => e.TenantSurpathServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Metadata.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetadataFilter), e => e.Metadata.Contains(input.MetadataFilter))
                        .WhereIf(input.IsSurpathOnlyFilter.HasValue && input.IsSurpathOnlyFilter > -1, e => (input.IsSurpathOnlyFilter == 1 && e.IsSurpathOnly) || (input.IsSurpathOnlyFilter == 0 && !e.IsSurpathOnly))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter);

            var query = (from o in filteredRecordRequirements
                         join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _cohortLookUpRepository.GetAll() on o.CohortId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _tenantSurpathServiceLookUpRepository.GetAll().Include(r => r.SurpathServiceFk) on o.SurpathServiceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetRecordRequirementForViewDto()
                         {
                             RecordRequirement = new RecordRequirementDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Metadata = o.Metadata,
                                 IsSurpathOnly = o.IsSurpathOnly,
                                 Id = o.Id
                             },
                             TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CohortName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             SurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                             //,TenantSurpathServiceName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var recordRequirementListDtos = await query.ToListAsync();

            return _recordRequirementsExcelExporter.ExportToFile(recordRequirementListDtos);

        }


        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements)]
        public async Task<PagedResultDto<RecordRequirementCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _cohortLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RecordRequirementCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new RecordRequirementCohortLookupTableDto
                {
                    Id = cohort.Id.ToString(),
                    DisplayName = cohort.Name?.ToString()
                });
            }

            return new PagedResultDto<RecordRequirementCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }
        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements)]
        public async Task<List<RecordRequirementSurpathServiceLookupTableDto>> GetAllSurpathServiceForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _tenantSurpathServiceLookUpRepository.GetAll()
            .Select(surpathService => new RecordRequirementSurpathServiceLookupTableDto
            {
                Id = surpathService.Id.ToString(),
                DisplayName = surpathService == null || surpathService.Name == null ? "" : surpathService.Name.ToString()
            }).ToListAsync();

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements)]
        public async Task<List<RecordRequirementTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _tenantSurpathServiceLookUpRepository.GetAll()
                .Select(tenantSurpathService => new RecordRequirementTenantSurpathServiceLookupTableDto
                {
                    Id = tenantSurpathService.Id.ToString(),
                    DisplayName = tenantSurpathService == null || tenantSurpathService.Name == null ? "" : tenantSurpathService.Name.ToString()
                }).ToListAsync();

        }

    }
}