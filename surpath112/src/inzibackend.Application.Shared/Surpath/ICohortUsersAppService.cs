using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ICohortUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCohortUserForViewDto>> GetAll(GetAllCohortUsersInput input);

        Task<GetCohortUserForViewDto> GetCohortUserForView(Guid? id);

        Task<GetCohortUserForEditOutput> GetCohortUserForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCohortUserDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCohortUsersToExcel(GetAllCohortUsersForExcelInput input);

        Task<FileDto> GetCohortUsersImmunizationReportToExcel(GetAllCohortUsersForExcelInput input);

        Task<PagedResultDto<CohortUserCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CohortUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}