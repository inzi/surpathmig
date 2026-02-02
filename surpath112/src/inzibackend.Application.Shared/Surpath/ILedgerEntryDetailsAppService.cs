using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ILedgerEntryDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLedgerEntryDetailForViewDto>> GetAll(GetAllLedgerEntryDetailsInput input);

        Task<GetLedgerEntryDetailForViewDto> GetLedgerEntryDetailForView(Guid id);

        Task<GetLedgerEntryDetailForEditOutput> GetLedgerEntryDetailForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditLedgerEntryDetailDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetLedgerEntryDetailsToExcel(GetAllLedgerEntryDetailsForExcelInput input);

        Task<PagedResultDto<LedgerEntryDetailLedgerEntryLookupTableDto>> GetAllLedgerEntryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LedgerEntryDetailSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LedgerEntryDetailTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForLookupTable(GetAllForLookupTableInput input);

    }
}