using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ITenantDocumentCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantDocumentCategoryForViewDto>> GetAll(GetAllTenantDocumentCategoriesInput input);

        Task<GetTenantDocumentCategoryForViewDto> GetTenantDocumentCategoryForView(Guid id);

        Task<GetTenantDocumentCategoryForEditOutput> GetTenantDocumentCategoryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTenantDocumentCategoryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTenantDocumentCategoriesToExcel(GetAllTenantDocumentCategoriesForExcelInput input);

        Task<PagedResultDto<TenantDocumentCategoryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}