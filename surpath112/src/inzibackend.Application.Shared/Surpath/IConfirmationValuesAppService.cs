using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IConfirmationValuesAppService : IApplicationService
    {
        Task<PagedResultDto<GetConfirmationValueForViewDto>> GetAll(GetAllConfirmationValuesInput input);

        Task<GetConfirmationValueForViewDto> GetConfirmationValueForView(Guid id);

        Task<GetConfirmationValueForEditOutput> GetConfirmationValueForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditConfirmationValueDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetConfirmationValuesToExcel(GetAllConfirmationValuesForExcelInput input);

        Task<PagedResultDto<ConfirmationValueDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input);

        Task<List<ConfirmationValueTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown();

    }
}