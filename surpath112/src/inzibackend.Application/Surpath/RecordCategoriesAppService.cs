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

namespace inzibackend.Surpath
{
    [AbpAuthorize]
    public class RecordCategoriesAppService : inzibackendAppServiceBase, IRecordCategoriesAppService
    {
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRecordCategoriesExcelExporter _recordCategoriesExcelExporter;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementLookUpRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleLookUpRepository;

        public RecordCategoriesAppService(IRepository<RecordCategory, Guid> recordCategoryRepository, IRecordCategoriesExcelExporter recordCategoriesExcelExporter, IRepository<RecordRequirement, Guid> lookup_recordRequirementRepository, IRepository<RecordCategoryRule, Guid> lookup_recordCategoryRuleRepository)
        {
            _recordCategoryRepository = recordCategoryRepository;
            _recordCategoriesExcelExporter = recordCategoriesExcelExporter;
            _recordRequirementLookUpRepository = lookup_recordRequirementRepository;
            _recordCategoryRuleLookUpRepository = lookup_recordCategoryRuleRepository;

        }
        [AbpAuthorize(AppPermissions.Pages_RecordCategories)]

        public async Task<PagedResultDto<GetRecordCategoryForViewDto>> GetAll(GetAllRecordCategoriesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredRecordCategories = _recordCategoryRepository.GetAll()
                        .Include(e => e.RecordRequirementFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .Where(e => e.RecordRequirementFk.IsSurpathOnly == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Instructions.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InstructionsFilter), e => e.Instructions.Contains(input.InstructionsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordRequirementNameFilter), e => e.RecordRequirementFk != null && e.RecordRequirementFk.Name == input.RecordRequirementNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter)
                        .WhereIf(input.RecordRequirementIdFilter.HasValue, e => false || e.RecordRequirementId == input.RecordRequirementIdFilter.Value);

            var pagedAndFilteredRecordCategories = filteredRecordCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var recordCategories = from o in pagedAndFilteredRecordCategories
                                   join o1 in _recordRequirementLookUpRepository.GetAll() on o.RecordRequirementId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _recordCategoryRuleLookUpRepository.GetAll() on o.RecordCategoryRuleId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.Name,
                                       o.Instructions,
                                       Id = o.Id,
                                       RecordRequirementName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       RecordCategoryRuleName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredRecordCategories.CountAsync();

            var dbList = await recordCategories.ToListAsync();
            var results = new List<GetRecordCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordCategoryForViewDto()
                {
                    RecordCategory = new RecordCategoryDto
                    {

                        Name = o.Name,
                        Instructions = o.Instructions,
                        Id = o.Id,
                    },
                    RecordRequirementName = o.RecordRequirementName,
                    RecordCategoryRuleName = o.RecordCategoryRuleName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRecordCategoryForViewDto>(
                totalCount,
                results
            );

        }
            [AbpAuthorize(AppPermissions.Pages_RecordCategories)]


            public async Task<GetRecordCategoryForViewDto> GetRecordCategoryForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategory = await _recordCategoryRepository.GetAsync(id);

            var output = new GetRecordCategoryForViewDto { RecordCategory = ObjectMapper.Map<RecordCategoryDto>(recordCategory) };

            if (output.RecordCategory.RecordRequirementId != null)
            {
                var _lookupRecordRequirement = await _recordRequirementLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordRequirementId);
                output.RecordRequirementName = _lookupRecordRequirement?.Name?.ToString();
            }

            if (output.RecordCategory.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories_Edit)]
        public async Task<GetRecordCategoryForEditOutput> GetRecordCategoryForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategory = await _recordCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecordCategoryForEditOutput { RecordCategory = ObjectMapper.Map<CreateOrEditRecordCategoryDto>(recordCategory) };

            if (output.RecordCategory.RecordRequirementId != null)
            {
                var _lookupRecordRequirement = await _recordRequirementLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordRequirementId);
                output.RecordRequirementName = _lookupRecordRequirement?.Name?.ToString();
            }

            if (output.RecordCategory.RecordCategoryRuleId != null)
            {
                var _lookupRecordCategoryRule = await _recordCategoryRuleLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordCategory.RecordCategoryRuleId);
                output.RecordCategoryRuleName = _lookupRecordCategoryRule?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRecordCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RecordCategories_Create)]
        protected virtual async Task Create(CreateOrEditRecordCategoryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategory = ObjectMapper.Map<RecordCategory>(input);

            if (AbpSession.TenantId != null)
            {
                recordCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordCategoryRepository.InsertAsync(recordCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories_Edit)]
        protected virtual async Task Update(CreateOrEditRecordCategoryDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var recordCategory = await _recordCategoryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _recordCategoryRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetRecordCategoriesToExcel(GetAllRecordCategoriesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredRecordCategories = _recordCategoryRepository.GetAll()
                        .Include(e => e.RecordRequirementFk)
                        .Include(e => e.RecordCategoryRuleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Instructions.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InstructionsFilter), e => e.Instructions.Contains(input.InstructionsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordRequirementNameFilter), e => e.RecordRequirementFk != null && e.RecordRequirementFk.Name == input.RecordRequirementNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RecordCategoryRuleNameFilter), e => e.RecordCategoryRuleFk != null && e.RecordCategoryRuleFk.Name == input.RecordCategoryRuleNameFilter);

            var query = (from o in filteredRecordCategories
                         join o1 in _recordRequirementLookUpRepository.GetAll() on o.RecordRequirementId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _recordCategoryRuleLookUpRepository.GetAll() on o.RecordCategoryRuleId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRecordCategoryForViewDto()
                         {
                             RecordCategory = new RecordCategoryDto
                             {
                                 Name = o.Name,
                                 Instructions = o.Instructions,
                                 Id = o.Id
                             },
                             RecordRequirementName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             RecordCategoryRuleName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var recordCategoryListDtos = await query.ToListAsync();

            return _recordCategoriesExcelExporter.ExportToFile(recordCategoryListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories)]
        public async Task<PagedResultDto<RecordCategoryRecordRequirementLookupTableDto>> GetAllRecordRequirementForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _recordRequirementLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var recordRequirementList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RecordCategoryRecordRequirementLookupTableDto>();
            foreach (var recordRequirement in recordRequirementList)
            {
                lookupTableDtoList.Add(new RecordCategoryRecordRequirementLookupTableDto
                {
                    Id = recordRequirement.Id.ToString(),
                    DisplayName = recordRequirement.Name?.ToString()
                });
            }

            return new PagedResultDto<RecordCategoryRecordRequirementLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }
        [AbpAuthorize(AppPermissions.Pages_RecordCategories)]
        public async Task<List<RecordCategoryRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _recordCategoryRuleLookUpRepository.GetAll()
                .Select(recordCategoryRule => new RecordCategoryRecordCategoryRuleLookupTableDto
                {
                    Id = recordCategoryRule.Id.ToString(),
                    DisplayName = recordCategoryRule == null || recordCategoryRule.Name == null ? "" : recordCategoryRule.Name.ToString()
                }).ToListAsync();

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategories, AppPermissions.Pages_CohortUser, AppPermissions.Pages_CohortUsers)]

        public async Task<RecordCategoryDto> GetRecordCategoryDto(Guid catid)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            //var r = _recordCategoryRepository.Get(catid);
            var r = _recordCategoryRepository.GetAll().IgnoreQueryFilters().Include(e => e.RecordRequirementFk).Where(e => e.Id == catid && e.IsDeleted == false).FirstOrDefault();
            if (r == null) return null;
            var d = ObjectMapper.Map<RecordCategoryDto>(r);
            d.IsSurpathService = r.RecordRequirementFk.IsSurpathOnly;
            d.TenantId = r.TenantId;
            return d;
        }
    }
}