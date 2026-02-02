using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Microsoft.AspNetCore.Http;


namespace inzibackend.Surpath
{
    public interface IRecordsAppService : IApplicationService
    {
        Task CreateOrEdit(CreateOrEditRecordDto input);
        Task Delete(EntityDto<Guid> input);
        Task<PagedResultDto<GetRecordForViewDto>> GetAll(GetAllRecordsInput input);
        Task<PagedResultDto<RecordTenantDocumentCategoryLookupTableDto>> GetAllTenantDocumentCategoryForLookupTable(GetAllForLookupTableInput input);
        Task<GetRecordForEditOutput> GetRecordForEdit(EntityDto<Guid> input);
        Task<GetRecordForViewDto> GetRecordForView(Guid id);
        Task<FileDto> GetRecordsToExcel(GetAllRecordsForExcelInput input);
        Task<Guid> ManualDocumentUpload(IFormFile file, string documentType);
        Task RemovefiledataFile(EntityDto<Guid> input);

    }
}