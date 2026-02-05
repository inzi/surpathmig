using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ITenantDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantDocumentForViewDto>> GetAll(GetAllTenantDocumentsInput input);

        Task<GetTenantDocumentForViewDto> GetTenantDocumentForView(Guid id);

        Task<GetTenantDocumentForEditOutput> GetTenantDocumentForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTenantDocumentDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTenantDocumentsToExcel(GetAllTenantDocumentsForExcelInput input);

        Task<PagedResultDto<TenantDocumentTenantDocumentCategoryLookupTableDto>> GetAllTenantDocumentCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TenantDocumentRecordLookupTableDto>> GetAllRecordForLookupTable(GetAllForLookupTableInput input);

    }
}