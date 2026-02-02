using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IDepartmentUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetDepartmentUserForViewDto>> GetAll(GetAllDepartmentUsersInput input);

        Task<GetDepartmentUserForViewDto> GetDepartmentUserForView(Guid id);

        Task<GetDepartmentUserForEditOutput> GetDepartmentUserForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDepartmentUserDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDepartmentUsersToExcel(GetAllDepartmentUsersForExcelInput input);

        Task<PagedResultDto<DepartmentUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        //Task<PagedResultDto<DepartmentUserTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

    }
}