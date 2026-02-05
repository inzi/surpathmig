using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ITenantDepartmentUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantDepartmentUserForViewDto>> GetAll(GetAllTenantDepartmentUsersInput input);

        Task<GetTenantDepartmentUserForViewDto> GetTenantDepartmentUserForView(Guid id);

        Task<GetTenantDepartmentUserForEditOutput> GetTenantDepartmentUserForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTenantDepartmentUserDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTenantDepartmentUsersToExcel(GetAllTenantDepartmentUsersForExcelInput input);

        Task<PagedResultDto<TenantDepartmentUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        //Task<PagedResultDto<TenantDepartmentUserTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input);

    }
}