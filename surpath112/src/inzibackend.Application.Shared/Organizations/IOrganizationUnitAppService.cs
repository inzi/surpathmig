using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Organizations.Dto;

namespace inzibackend.Organizations
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits();

        Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input);

        Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input);

        Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input);

        Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input);

        Task DeleteOrganizationUnit(EntityDto<long> input);

        Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input);

        Task RemoveRoleFromOrganizationUnit(RoleToOrganizationUnitInput input);

        Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input);

        Task AddRolesToOrganizationUnit(RolesToOrganizationUnitInput input);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input);

        Task<PagedResultDto<NameValueDto>> FindRoles(FindOrganizationUnitRolesInput input);

        Task RemoveDepartmentFromOrganizationUnit(TenantDepartmentToOrganizationUnitInput input);

        Task AddDepartmentsToOrganizationUnit(TenantDepartmentsToOrganizationUnitInput input);

        Task<bool> IsUserInOrganizationUnitAndDepartmentAsync(long userId, long organizationUnitId, Guid departmentId);
    }
}
