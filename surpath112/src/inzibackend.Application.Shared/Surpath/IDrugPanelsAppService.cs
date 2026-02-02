using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IDrugPanelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDrugPanelForViewDto>> GetAll(GetAllDrugPanelsInput input);

        Task<GetDrugPanelForViewDto> GetDrugPanelForView(Guid id);

        Task<GetDrugPanelForEditOutput> GetDrugPanelForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDrugPanelDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDrugPanelsToExcel(GetAllDrugPanelsForExcelInput input);

        Task<PagedResultDto<DrugPanelDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DrugPanelPanelLookupTableDto>> GetAllPanelForLookupTable(GetAllForLookupTableInput input);

    }
}