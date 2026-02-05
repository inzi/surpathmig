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

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories)]
    public class DrugTestCategoriesAppService : inzibackendAppServiceBase, IDrugTestCategoriesAppService
    {
        private readonly IRepository<DrugTestCategory, Guid> _drugTestCategoryRepository;
        private readonly IDrugTestCategoriesExcelExporter _drugTestCategoriesExcelExporter;
        private readonly IRepository<Drug, Guid> _drugLookUpRepository;
        private readonly IRepository<Panel, Guid> _panelLookUpRepository;
        private readonly IRepository<TestCategory, Guid> _testCategoryLookUpRepository;

        public DrugTestCategoriesAppService(IRepository<DrugTestCategory, Guid> drugTestCategoryRepository, IDrugTestCategoriesExcelExporter drugTestCategoriesExcelExporter, IRepository<Drug, Guid> lookup_drugRepository, IRepository<Panel, Guid> lookup_panelRepository, IRepository<TestCategory, Guid> lookup_testCategoryRepository)
        {
            _drugTestCategoryRepository = drugTestCategoryRepository;
            _drugTestCategoriesExcelExporter = drugTestCategoriesExcelExporter;
            _drugLookUpRepository = lookup_drugRepository;
            _panelLookUpRepository = lookup_panelRepository;
            _testCategoryLookUpRepository = lookup_testCategoryRepository;

        }

        public async Task<PagedResultDto<GetDrugTestCategoryForViewDto>> GetAll(GetAllDrugTestCategoriesInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredDrugTestCategories = _drugTestCategoryRepository.GetAll()
                            .Include(e => e.DrugFk)
                            .Include(e => e.PanelFk)
                            .Include(e => e.TestCategoryFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PanelNameFilter), e => e.PanelFk != null && e.PanelFk.Name == input.PanelNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

                var pagedAndFilteredDrugTestCategories = filteredDrugTestCategories
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var drugTestCategories = from o in pagedAndFilteredDrugTestCategories
                                         join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _panelLookUpRepository.GetAll() on o.PanelId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         join o3 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o3.Id into j3
                                         from s3 in j3.DefaultIfEmpty()

                                         select new
                                         {

                                             Id = o.Id,
                                             DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             PanelName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                             TestCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                         };

                var totalCount = await filteredDrugTestCategories.CountAsync();

                var dbList = await drugTestCategories.ToListAsync();
                var results = new List<GetDrugTestCategoryForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetDrugTestCategoryForViewDto()
                    {
                        DrugTestCategory = new DrugTestCategoryDto
                        {

                            Id = o.Id,
                        },
                        DrugName = o.DrugName,
                        PanelName = o.PanelName,
                        TestCategoryName = o.TestCategoryName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetDrugTestCategoryForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetDrugTestCategoryForViewDto> GetDrugTestCategoryForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var drugTestCategory = await _drugTestCategoryRepository.GetAsync(id);

                var output = new GetDrugTestCategoryForViewDto { DrugTestCategory = ObjectMapper.Map<DrugTestCategoryDto>(drugTestCategory) };

                if (output.DrugTestCategory.DrugId != null)
                {
                    var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.DrugId);
                    output.DrugName = _lookupDrug?.Name?.ToString();
                }

                if (output.DrugTestCategory.PanelId != null)
                {
                    var _lookupPanel = await _panelLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.PanelId);
                    output.PanelName = _lookupPanel?.Name?.ToString();
                }

                if (output.DrugTestCategory.TestCategoryId != null)
                {
                    var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.TestCategoryId);
                    output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Edit)]
        public async Task<GetDrugTestCategoryForEditOutput> GetDrugTestCategoryForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var drugTestCategory = await _drugTestCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDrugTestCategoryForEditOutput { DrugTestCategory = ObjectMapper.Map<CreateOrEditDrugTestCategoryDto>(drugTestCategory) };

            if (output.DrugTestCategory.DrugId != null)
            {
                var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.DrugId);
                output.DrugName = _lookupDrug?.Name?.ToString();
            }

            if (output.DrugTestCategory.PanelId != null)
            {
                var _lookupPanel = await _panelLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.PanelId);
                output.PanelName = _lookupPanel?.Name?.ToString();
            }

            if (output.DrugTestCategory.TestCategoryId != null)
            {
                var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugTestCategory.TestCategoryId);
                output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDrugTestCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Create)]
        protected virtual async Task Create(CreateOrEditDrugTestCategoryDto input)
        {
            var drugTestCategory = ObjectMapper.Map<DrugTestCategory>(input);

            await _drugTestCategoryRepository.InsertAsync(drugTestCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Edit)]
        protected virtual async Task Update(CreateOrEditDrugTestCategoryDto input)
        {
            var drugTestCategory = await _drugTestCategoryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, drugTestCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _drugTestCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDrugTestCategoriesToExcel(GetAllDrugTestCategoriesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredDrugTestCategories = _drugTestCategoryRepository.GetAll()
                        .Include(e => e.DrugFk)
                        .Include(e => e.PanelFk)
                        .Include(e => e.TestCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PanelNameFilter), e => e.PanelFk != null && e.PanelFk.Name == input.PanelNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

            var query = (from o in filteredDrugTestCategories
                         join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _panelLookUpRepository.GetAll() on o.PanelId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetDrugTestCategoryForViewDto()
                         {
                             DrugTestCategory = new DrugTestCategoryDto
                             {
                                 Id = o.Id
                             },
                             DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             PanelName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             TestCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var drugTestCategoryListDtos = await query.ToListAsync();

            return _drugTestCategoriesExcelExporter.ExportToFile(drugTestCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories)]
        public async Task<PagedResultDto<DrugTestCategoryDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _drugLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var drugList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DrugTestCategoryDrugLookupTableDto>();
            foreach (var drug in drugList)
            {
                lookupTableDtoList.Add(new DrugTestCategoryDrugLookupTableDto
                {
                    Id = drug.Id.ToString(),
                    DisplayName = drug.Name?.ToString()
                });
            }

            return new PagedResultDto<DrugTestCategoryDrugLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories)]
        public async Task<PagedResultDto<DrugTestCategoryPanelLookupTableDto>> GetAllPanelForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _panelLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var panelList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DrugTestCategoryPanelLookupTableDto>();
            foreach (var panel in panelList)
            {
                lookupTableDtoList.Add(new DrugTestCategoryPanelLookupTableDto
                {
                    Id = panel.Id.ToString(),
                    DisplayName = panel.Name?.ToString()
                });
            }

            return new PagedResultDto<DrugTestCategoryPanelLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_DrugTestCategories)]
        public async Task<List<DrugTestCategoryTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _testCategoryLookUpRepository.GetAll()
                .Select(testCategory => new DrugTestCategoryTestCategoryLookupTableDto
                {
                    Id = testCategory.Id.ToString(),
                    DisplayName = testCategory == null || testCategory.Name == null ? "" : testCategory.Name.ToString()
                }).ToListAsync();
        }

    }
}