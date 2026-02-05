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
    [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels)]
    public class DrugPanelsAppService : inzibackendAppServiceBase, IDrugPanelsAppService
    {
        private readonly IRepository<DrugPanel, Guid> _drugPanelRepository;
        private readonly IDrugPanelsExcelExporter _drugPanelsExcelExporter;
        private readonly IRepository<Drug, Guid> _drugLookUpRepository;
        private readonly IRepository<Panel, Guid> _panelLookUpRepository;

        public DrugPanelsAppService(IRepository<DrugPanel, Guid> drugPanelRepository, IDrugPanelsExcelExporter drugPanelsExcelExporter, IRepository<Drug, Guid> lookup_drugRepository, IRepository<Panel, Guid> lookup_panelRepository)
        {
            _drugPanelRepository = drugPanelRepository;
            _drugPanelsExcelExporter = drugPanelsExcelExporter;
            _drugLookUpRepository = lookup_drugRepository;
            _panelLookUpRepository = lookup_panelRepository;

        }

        public async Task<PagedResultDto<GetDrugPanelForViewDto>> GetAll(GetAllDrugPanelsInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredDrugPanels = _drugPanelRepository.GetAll()
                            .Include(e => e.DrugFk)
                            .Include(e => e.PanelFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PanelNameFilter), e => e.PanelFk != null && e.PanelFk.Name == input.PanelNameFilter);

                var pagedAndFilteredDrugPanels = filteredDrugPanels
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var drugPanels = from o in pagedAndFilteredDrugPanels
                                 join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _panelLookUpRepository.GetAll() on o.PanelId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     Id = o.Id,
                                     DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     PanelName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

                var totalCount = await filteredDrugPanels.CountAsync();

                var dbList = await drugPanels.ToListAsync();
                var results = new List<GetDrugPanelForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetDrugPanelForViewDto()
                    {
                        DrugPanel = new DrugPanelDto
                        {

                            Id = o.Id,
                        },
                        DrugName = o.DrugName,
                        PanelName = o.PanelName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetDrugPanelForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetDrugPanelForViewDto> GetDrugPanelForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var drugPanel = await _drugPanelRepository.GetAsync(id);

                var output = new GetDrugPanelForViewDto { DrugPanel = ObjectMapper.Map<DrugPanelDto>(drugPanel) };

                if (output.DrugPanel.DrugId != null)
                {
                    var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugPanel.DrugId);
                    output.DrugName = _lookupDrug?.Name?.ToString();
                }

                if (output.DrugPanel.PanelId != null)
                {
                    var _lookupPanel = await _panelLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugPanel.PanelId);
                    output.PanelName = _lookupPanel?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels_Edit)]
        public async Task<GetDrugPanelForEditOutput> GetDrugPanelForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var drugPanel = await _drugPanelRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetDrugPanelForEditOutput { DrugPanel = ObjectMapper.Map<CreateOrEditDrugPanelDto>(drugPanel) };

                if (output.DrugPanel.DrugId != null)
                {
                    var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugPanel.DrugId);
                    output.DrugName = _lookupDrug?.Name?.ToString();
                }

                if (output.DrugPanel.PanelId != null)
                {
                    var _lookupPanel = await _panelLookUpRepository.FirstOrDefaultAsync((Guid)output.DrugPanel.PanelId);
                    output.PanelName = _lookupPanel?.Name?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditDrugPanelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels_Create)]
        protected virtual async Task Create(CreateOrEditDrugPanelDto input)
        {
            var drugPanel = ObjectMapper.Map<DrugPanel>(input);

            await _drugPanelRepository.InsertAsync(drugPanel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels_Edit)]
        protected virtual async Task Update(CreateOrEditDrugPanelDto input)
        {
            var drugPanel = await _drugPanelRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, drugPanel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _drugPanelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDrugPanelsToExcel(GetAllDrugPanelsForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredDrugPanels = _drugPanelRepository.GetAll()
                            .Include(e => e.DrugFk)
                            .Include(e => e.PanelFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PanelNameFilter), e => e.PanelFk != null && e.PanelFk.Name == input.PanelNameFilter);

                var query = (from o in filteredDrugPanels
                             join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _panelLookUpRepository.GetAll() on o.PanelId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetDrugPanelForViewDto()
                             {
                                 DrugPanel = new DrugPanelDto
                                 {
                                     Id = o.Id
                                 },
                                 DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 PanelName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             });

                var drugPanelListDtos = await query.ToListAsync();

                return _drugPanelsExcelExporter.ExportToFile(drugPanelListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels)]
        public async Task<PagedResultDto<DrugPanelDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input)
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

                var lookupTableDtoList = new List<DrugPanelDrugLookupTableDto>();
                foreach (var drug in drugList)
                {
                    lookupTableDtoList.Add(new DrugPanelDrugLookupTableDto
                    {
                        Id = drug.Id.ToString(),
                        DisplayName = drug.Name?.ToString()
                    });
                }

                return new PagedResultDto<DrugPanelDrugLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DrugPanels)]
        public async Task<PagedResultDto<DrugPanelPanelLookupTableDto>> GetAllPanelForLookupTable(GetAllForLookupTableInput input)
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

                var lookupTableDtoList = new List<DrugPanelPanelLookupTableDto>();
                foreach (var panel in panelList)
                {
                    lookupTableDtoList.Add(new DrugPanelPanelLookupTableDto
                    {
                        Id = panel.Id.ToString(),
                        DisplayName = panel.Name?.ToString()
                    });
                }

                return new PagedResultDto<DrugPanelPanelLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

    }
}