using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IUserPidsAppService : IApplicationService
    {
        Task<PagedResultDto<GetUserPidForViewDto>> GetAll(GetAllUserPidsInput input);

        Task<GetUserPidForViewDto> GetUserPidForView(Guid id);

        Task<GetUserPidForEditOutput> GetUserPidForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditUserPidDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetUserPidsToExcel(GetAllUserPidsForExcelInput input);

        Task<List<UserPidPidTypeLookupTableDto>> GetAllPidTypeForTableDropdown();

        Task<PagedResultDto<UserPidUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}