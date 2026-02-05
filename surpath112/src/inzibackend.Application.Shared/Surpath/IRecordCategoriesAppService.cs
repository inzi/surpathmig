using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IRecordCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRecordCategoryForViewDto>> GetAll(GetAllRecordCategoriesInput input);

        Task<GetRecordCategoryForViewDto> GetRecordCategoryForView(Guid id);

        Task<GetRecordCategoryForEditOutput> GetRecordCategoryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordCategoryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordCategoriesToExcel(GetAllRecordCategoriesForExcelInput input);

        Task<PagedResultDto<RecordCategoryRecordRequirementLookupTableDto>> GetAllRecordRequirementForLookupTable(GetAllForLookupTableInput input);

        Task<List<RecordCategoryRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForTableDropdown();


        Task<RecordCategoryDto> GetRecordCategoryDto(Guid catid);

    }
}