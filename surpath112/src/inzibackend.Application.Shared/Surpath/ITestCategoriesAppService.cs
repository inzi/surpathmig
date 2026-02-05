using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ITestCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTestCategoryForViewDto>> GetAll(GetAllTestCategoriesInput input);

        Task<GetTestCategoryForViewDto> GetTestCategoryForView(Guid id);

        Task<GetTestCategoryForEditOutput> GetTestCategoryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTestCategoryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTestCategoriesToExcel(GetAllTestCategoriesForExcelInput input);

    }
}