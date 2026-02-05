using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;


namespace inzibackend.Surpath
{
    public interface ITenantSurpathServicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantSurpathServiceForViewDto>> GetAll(GetAllTenantSurpathServicesInput input);

        Task<GetTenantSurpathServiceForViewDto> GetTenantSurpathServiceForView(Guid id);

        Task<GetTenantSurpathServiceForEditOutput> GetTenantSurpathServiceForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTenantSurpathServiceDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTenantSurpathServicesToExcel(GetAllTenantSurpathServicesForExcelInput input);

        Task<PagedResultDto<TenantSurpathServiceSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input);

        //Task<PagedResultDto<TenantSurpathServiceTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TenantSurpathServiceCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TenantSurpathServiceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TenantSurpathServiceRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForLookupTable(GetAllForLookupTableInput input);
        //Task<PagedResultDto<TenantSurpathServiceRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForLookupTable(GetAllForLookupTableInput input);
        Task<List<TenantSurpathServiceDto>> GetAllTenantSurpathServiceDtoForTenant(EntityDto input);
        
        Task<bool> CloneAsync(EntityDto<Guid> input);

        Task<PagedResultDto<GetSurpathServiceForViewDto>> GetAllTenantServices(GetAllSurpathServicesInput input);

        Task AssignToOrganizationUnit(Guid id, long organizationUnitId);

        Task AssignToCohort(Guid id, Guid cohortId);

        Task AssignToTenantDepartment(Guid id, Guid departmentId);

        Task<bool> IsUserPaid(long userId);

        Task AssignToTenant(Guid id);

        Task AssignToCohortUser(Guid id, Guid cohortUserId);

        // Hierarchical pricing management methods
        Task<HierarchicalPricingDto> GetHierarchicalPricing(GetHierarchicalPricingInput input);
        
        Task<HierarchicalPricingNodeDto> GetHierarchicalPricingV2(GetHierarchicalPricingInputV2 input);

        Task BatchUpdatePrices(BatchUpdatePriceDto input);

        Task SetAllServicesPrice(SetAllServicesPriceDto input);

        Task<List<SurpathServiceDto>> GetAvailableSurpathServices();

        Task ToggleEnabled(ToggleEnabledDto input);
        
        Task UpdateServicePrice(UpdateServicePriceDto input);
    }
}