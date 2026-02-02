using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IPanelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPanelForViewDto>> GetAll(GetAllPanelsInput input);

        Task<GetPanelForViewDto> GetPanelForView(Guid id);

        Task<GetPanelForEditOutput> GetPanelForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditPanelDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetPanelsToExcel(GetAllPanelsForExcelInput input);

        Task<List<PanelTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown();

    }
}