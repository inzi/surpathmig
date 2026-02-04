using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IRecordRequirementsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRecordRequirementForViewDto>> GetAll(GetAllRecordRequirementsInput input);

        Task<GetRecordRequirementForViewDto> GetRecordRequirementForView(Guid id);

        Task<GetRecordRequirementForEditOutput> GetRecordRequirementForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordRequirementDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordRequirementsToExcel(GetAllRecordRequirementsForExcelInput input);

        //Task<PagedResultDto<RecordRequirementTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<RecordRequirementCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);

        Task<List<RecordRequirementSurpathServiceLookupTableDto>> GetAllSurpathServiceForTableDropdown();

        Task<List<RecordRequirementTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForTableDropdown();

    }
}
