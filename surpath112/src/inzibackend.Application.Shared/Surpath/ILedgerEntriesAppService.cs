using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using inzibackend.Authorization.Users.Dto;

namespace inzibackend.Surpath
{
    public interface ILedgerEntriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLedgerEntryForViewDto>> GetAll(GetAllLedgerEntriesInput input);
        Task<PagedResultDto<GetLedgerEntryForViewDto>> GetAllForUserId(GetAllLedgerEntriesInput input);

        Task<GetLedgerEntryForViewDto> GetLedgerEntryForView(Guid id);

        Task<GetLedgerEntryForEditOutput> GetLedgerEntryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditLedgerEntryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetLedgerEntriesToExcel(GetAllLedgerEntriesForExcelInput input);

        Task<PagedResultDto<LedgerEntryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LedgerEntryTenantDocumentLookupTableDto>> GetAllTenantDocumentForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LedgerEntryCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);
        Task<UserEditDto> GetUserDetails(long userId);
    }
}