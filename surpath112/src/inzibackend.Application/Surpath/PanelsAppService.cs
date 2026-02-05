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
    [AbpAuthorize(AppPermissions.Pages_Administration_Panels)]
    public class PanelsAppService : inzibackendAppServiceBase, IPanelsAppService
    {
        private readonly IRepository<Panel, Guid> _panelRepository;
        private readonly IPanelsExcelExporter _panelsExcelExporter;
        private readonly IRepository<TestCategory, Guid> _testCategoryLookUpRepository;

        public PanelsAppService(IRepository<Panel, Guid> panelRepository, IPanelsExcelExporter panelsExcelExporter, IRepository<TestCategory, Guid> lookup_testCategoryRepository)
        {
            _panelRepository = panelRepository;
            _panelsExcelExporter = panelsExcelExporter;
            _testCategoryLookUpRepository = lookup_testCategoryRepository;

        }

        public async Task<PagedResultDto<GetPanelForViewDto>> GetAll(GetAllPanelsInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredPanels = _panelRepository.GetAll()
                            .Include(e => e.TestCategoryFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(input.MinCostFilter != null, e => e.Cost >= input.MinCostFilter)
                            .WhereIf(input.MaxCostFilter != null, e => e.Cost <= input.MaxCostFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

                var pagedAndFilteredPanels = filteredPanels
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var panels = from o in pagedAndFilteredPanels
                             join o1 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.Cost,
                                 o.Description,
                                 Id = o.Id,
                                 TestCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

                var totalCount = await filteredPanels.CountAsync();

                var dbList = await panels.ToListAsync();
                var results = new List<GetPanelForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetPanelForViewDto()
                    {
                        Panel = new PanelDto
                        {

                            Name = o.Name,
                            Cost = o.Cost,
                            Description = o.Description,
                            Id = o.Id,
                        },
                        TestCategoryName = o.TestCategoryName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetPanelForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetPanelForViewDto> GetPanelForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var panel = await _panelRepository.GetAsync(id);

                var output = new GetPanelForViewDto { Panel = ObjectMapper.Map<PanelDto>(panel) };

                if (output.Panel.TestCategoryId != null)
                {
                    var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.Panel.TestCategoryId);
                    output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Panels_Edit)]
        public async Task<GetPanelForEditOutput> GetPanelForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var panel = await _panelRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetPanelForEditOutput { Panel = ObjectMapper.Map<CreateOrEditPanelDto>(panel) };

                if (output.Panel.TestCategoryId != null)
                {
                    var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.Panel.TestCategoryId);
                    output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditPanelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Panels_Create)]
        protected virtual async Task Create(CreateOrEditPanelDto input)
        {
            var panel = ObjectMapper.Map<Panel>(input);

            await _panelRepository.InsertAsync(panel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Panels_Edit)]
        protected virtual async Task Update(CreateOrEditPanelDto input)
        {
            var panel = await _panelRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, panel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Panels_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _panelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPanelsToExcel(GetAllPanelsForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredPanels = _panelRepository.GetAll()
                            .Include(e => e.TestCategoryFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(input.MinCostFilter != null, e => e.Cost >= input.MinCostFilter)
                            .WhereIf(input.MaxCostFilter != null, e => e.Cost <= input.MaxCostFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

                var query = (from o in filteredPanels
                             join o1 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new GetPanelForViewDto()
                             {
                                 Panel = new PanelDto
                                 {
                                     Name = o.Name,
                                     Cost = o.Cost,
                                     Description = o.Description,
                                     Id = o.Id
                                 },
                                 TestCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             });

                var panelListDtos = await query.ToListAsync();

                return _panelsExcelExporter.ExportToFile(panelListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Panels)]
        public async Task<List<PanelTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown()
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _testCategoryLookUpRepository.GetAll()
                    .Select(testCategory => new PanelTestCategoryLookupTableDto
                    {
                        Id = testCategory.Id.ToString(),
                        DisplayName = testCategory == null || testCategory.Name == null ? "" : testCategory.Name.ToString()
                    }).ToListAsync();
        }

    }
}