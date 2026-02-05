using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IDrugTestCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDrugTestCategoryForViewDto>> GetAll(GetAllDrugTestCategoriesInput input);

        Task<GetDrugTestCategoryForViewDto> GetDrugTestCategoryForView(Guid id);

        Task<GetDrugTestCategoryForEditOutput> GetDrugTestCategoryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDrugTestCategoryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDrugTestCategoriesToExcel(GetAllDrugTestCategoriesForExcelInput input);

        Task<PagedResultDto<DrugTestCategoryDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DrugTestCategoryPanelLookupTableDto>> GetAllPanelForLookupTable(GetAllForLookupTableInput input);

        Task<List<DrugTestCategoryTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown();

    }
}