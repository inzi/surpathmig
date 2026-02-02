using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ISurpathServicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSurpathServiceForViewDto>> GetAll(GetAllSurpathServicesInput input);

        Task<GetSurpathServiceForViewDto> GetSurpathServiceForView(Guid id);

        Task<GetSurpathServiceForEditOutput> GetSurpathServiceForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditSurpathServiceDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetSurpathServicesToExcel(GetAllSurpathServicesForExcelInput input);

        //Task<PagedResultDto<SurpathServiceTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<SurpathServiceCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<SurpathServiceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<SurpathServiceRecordCategoryRuleLookupTableDto>> GetAllRecordCategoryRuleForLookupTable(GetAllForLookupTableInput input);

    }
}