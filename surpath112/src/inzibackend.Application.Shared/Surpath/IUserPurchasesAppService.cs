using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IUserPurchasesAppService : IApplicationService
    {
        Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAll(GetAllUserPurchasesInput input);
        
        Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAllForUserId(GetAllUserPurchasesInput input);
        
        Task<GetUserPurchaseForViewDto> GetUserPurchaseForView(Guid id);
        
        Task<GetUserPurchaseForEditOutput> GetUserPurchaseForEdit(EntityDto<Guid> input);
        
        Task CreateOrEdit(CreateOrEditUserPurchaseDto input);
        
        Task Delete(EntityDto<Guid> input);
        
        Task<FileDto> GetUserPurchasesToExcel(GetAllUserPurchasesForExcelInput input);
        
        Task<PagedResultDto<UserPurchaseUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
        
        Task<PagedResultDto<UserPurchaseSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input);
        
        Task<PagedResultDto<UserPurchaseTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForLookupTable(GetAllForLookupTableInput input);
        
        Task<PagedResultDto<UserPurchaseCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);
        
        // Methods for payment tracking
        Task<GetUserPurchaseForViewDto> GetUserPurchaseBalance(Guid id);
        
        Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAllWithBalances(GetAllUserPurchasesInput input);
        
        Task ApplyPayment(Guid userPurchaseId, double amount, string paymentMethod, string notes);
        
        Task AdjustPrice(Guid userPurchaseId, double adjustedPrice, string reason);
    }
}