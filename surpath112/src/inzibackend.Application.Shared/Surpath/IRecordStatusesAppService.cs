using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IRecordStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRecordStatusForViewDto>> GetAll(GetAllRecordStatusesInput input);

        Task<GetRecordStatusForViewDto> GetRecordStatusForView(Guid id);

        Task<GetRecordStatusForEditOutput> GetRecordStatusForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditRecordStatusDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetRecordStatusesToExcel(GetAllRecordStatusesForExcelInput input);

        //Task<PagedResultDto<RecordStatusTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

        Task EnsureSurpathServiceStatuses(int tenantId);

        Task<List<RecordStatusDto>> GetAllForDropdown();
    }
}