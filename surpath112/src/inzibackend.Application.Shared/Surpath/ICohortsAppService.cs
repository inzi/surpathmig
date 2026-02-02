using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface ICohortsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCohortForViewDto>> GetAll(GetAllCohortsInput input);

        Task<GetCohortForViewDto> GetCohortForView(Guid id);

        Task<GetCohortForEditOutput> GetCohortForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCohortDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCohortsToExcel(GetAllCohortsForExcelInput input);

        Task<FileDto> GetCohortsImmunizationReportToExcel(GetAllCohortsForExcelInput input);

        //Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<GetCohortForComplianceViewDto>> GetCompliance(GetAllCohortsInput input);

        Task<List<GetCohortForComplianceViewDto>> GetComplianceList(GetAllCohortsInput input);



    }
}