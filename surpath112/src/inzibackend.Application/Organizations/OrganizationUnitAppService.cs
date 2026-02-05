using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using inzibackend.Authorization;
using inzibackend.Organizations.Dto;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using inzibackend.Authorization.Roles;
using inzibackend.Surpath.Dtos;
using inzibackend.Surpath.OUs;
using inzibackend.Surpath;
using inzibackend.Authorization.Organizations;

namespace inzibackend.Organizations
{
    [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitAppService : inzibackendAppServiceBase, IOrganizationUnitAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<TenantDepartmentOrganizationUnit, Guid> _organizationUnitTenantDepartmentRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;

        private readonly RoleManager _roleManager;

        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            RoleManager roleManager,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<TenantDepartmentOrganizationUnit, Guid> organizationUnitTenantDepartmentRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository

            )
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _roleManager = roleManager;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _organizationUnitTenantDepartmentRepository = organizationUnitTenantDepartmentRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;

        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var organizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var organizationUnitMemberCounts = await _userOrganizationUnitRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedUsers => new
                {
                    organizationUnitId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            var organizationUnitRoleCounts = await _organizationUnitRoleRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedRoles => new
                {
                    organizationUnitId = groupedRoles.Key,
                    count = groupedRoles.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            var organizationUnitDepartmentCounts = await _organizationUnitTenantDepartmentRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupDepts => new
                {
                    organizationUnitId = groupDepts.Key,
                    count = groupDepts.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            return new ListResultDto<OrganizationUnitDto>(
                organizationUnits.Select(ou =>
                {
                    var organizationUnitDto = ObjectMapper.Map<OrganizationUnitDto>(ou);
                    organizationUnitDto.MemberCount = organizationUnitMemberCounts.ContainsKey(ou.Id) ? organizationUnitMemberCounts[ou.Id] : 0;
                    organizationUnitDto.RoleCount = organizationUnitRoleCounts.ContainsKey(ou.Id) ? organizationUnitRoleCounts[ou.Id] : 0;
                    organizationUnitDto.DepartmentCount = organizationUnitDepartmentCounts.ContainsKey(ou.Id) ? organizationUnitDepartmentCounts[ou.Id] : 0;
                    return organizationUnitDto;
                }).ToList());
        }

        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            var query = from ouUser in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouUser.OrganizationUnitId equals ou.Id
                        join user in UserManager.Users on ouUser.UserId equals user.Id
                        where ouUser.OrganizationUnitId == input.Id
                        select new
                        {
                            ouUser,
                            user
                        };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitUserDto = ObjectMapper.Map<OrganizationUnitUserListDto>(item.user);
                    organizationUnitUserDto.AddedTime = item.ouUser.CreationTime;
                    return organizationUnitUserDto;
                }).ToList());
        }

        public async Task<PagedResultDto<OrganizationUnitRoleListDto>> GetOrganizationUnitRoles(GetOrganizationUnitRolesInput input)
        {
            var query = from ouRole in _organizationUnitRoleRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouRole.OrganizationUnitId equals ou.Id
                        join role in _roleManager.Roles on ouRole.RoleId equals role.Id
                        where ouRole.OrganizationUnitId == input.Id
                        select new
                        {
                            ouRole,
                            role
                        };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitRoleListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitRoleDto = ObjectMapper.Map<OrganizationUnitRoleListDto>(item.role);
                    organizationUnitRoleDto.AddedTime = item.ouRole.CreationTime;
                    return organizationUnitRoleDto;
                }).ToList());
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);

            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            await _userOrganizationUnitRepository.DeleteAsync(x => x.OrganizationUnitId == input.Id);
            await _organizationUnitRoleRepository.DeleteAsync(x => x.OrganizationUnitId == input.Id);
            await _organizationUnitManager.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task RemoveRoleFromOrganizationUnit(RoleToOrganizationUnitInput input)
        {
            await _roleManager.RemoveFromOrganizationUnitAsync(input.RoleId, input.OrganizationUnitId);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            foreach (var userId in input.UserIds)
            {
                await UserManager.AddToOrganizationUnitAsync(userId, input.OrganizationUnitId);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task AddRolesToOrganizationUnit(RolesToOrganizationUnitInput input)
        {
            foreach (var roleId in input.RoleIds)
            {
                await _roleManager.AddToOrganizationUnitAsync(roleId, input.OrganizationUnitId, AbpSession.TenantId);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input)
        {
            var userIdsInOrganizationUnit = _userOrganizationUnitRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.UserId);

            var query = UserManager.Users
                .Where(u => !userIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(u =>
                    new NameValueDto(
                        u.FullName + " (" + u.EmailAddress + ")",
                        u.Id.ToString()
                    )
                ).ToList()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task<PagedResultDto<NameValueDto>> FindRoles(FindOrganizationUnitRolesInput input)
        {
            var roleIdsInOrganizationUnit = _organizationUnitRoleRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.RoleId);

            var query = _roleManager.Roles
                .Where(u => !roleIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.DisplayName.Contains(input.Filter) ||
                        u.Name.Contains(input.Filter)
                );

            var roleCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.DisplayName)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                roleCount,
                users.Select(u =>
                    new NameValueDto(
                        u.DisplayName,
                        u.Id.ToString()
                    )
                ).ToList()
            );
        }

        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        public async Task<PagedResultDto<OrganizationUnitDepartmentListDto>> GetOrganizationUnitDepartments(GetOrganizationUnitTenantDepartmentInput input)
        {
            //var q = from ouDepartment in _organizationUnitTenantDepartmentRepository.GetAll()
            //        select ouDepartment;

            //var r = q.ToList();



            var query = from ouDepartment in _organizationUnitTenantDepartmentRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouDepartment.OrganizationUnitId equals ou.Id
                        join department in _tenantDepartmentRepository.GetAll() on ouDepartment.TenantDepartmentId equals department.Id
                        where ouDepartment.OrganizationUnitId == input.Id
                        select new
                        {
                            ouDepartment,
                            department
                        };

            var totalCount = await query.CountAsync();
            //var items = await query.PageBy(input).ToListAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitDepartmentListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitDepartmentDto = new OrganizationUnitDepartmentListDto() { Name = item.department.Name, Description = item.department.Description, Id = item.department.Id};
                    organizationUnitDepartmentDto.AddedTime = item.ouDepartment.CreationTime;
                    return organizationUnitDepartmentDto;
                }).ToList());
        }


        public async Task<PagedResultDto<NameValueDto>> FindTenantDepartments(FindOrganizationUnitTenantDepartmentInput input)
        {
            var tenantDeptartmentIdsInOrganizationUnit = _organizationUnitTenantDepartmentRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.TenantDepartmentId);

            var query = _tenantDepartmentRepository.GetAll()
                .Where(u => !tenantDeptartmentIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    td =>
                        td.Name.Contains(input.Filter) ||
                        td.Description.Contains(input.Filter)
                );

            var deptCount = await query.CountAsync();
            var depts = await query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Description)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                deptCount,
                depts.Select(td =>
                    new NameValueDto(
                        td.Name,
                        td.Id.ToString()
                    )
                ).ToList()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageDepartments)]
        public async Task AddDepartmentsToOrganizationUnit(TenantDepartmentsToOrganizationUnitInput input)
        {
            foreach (var departmentId in input.TenantDepartmentIds)
            {
                var exists = await _organizationUnitTenantDepartmentRepository.FirstOrDefaultAsync(
                    x => x.OrganizationUnitId == input.OrganizationUnitId &&
                         x.TenantDepartmentId == departmentId
                );

                if (exists == null)
                {

                    await _organizationUnitTenantDepartmentRepository.InsertAsync(
                        new TenantDepartmentOrganizationUnit
                        {
                            TenantId = AbpSession.TenantId,
                            TenantDepartmentId = departmentId,
                            OrganizationUnitId = input.OrganizationUnitId
                        }
                    );
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageDepartments)]
        public async Task RemoveDepartmentFromOrganizationUnit(TenantDepartmentToOrganizationUnitInput input)
        {
            var ouDepartment = await _organizationUnitTenantDepartmentRepository.FirstOrDefaultAsync(
                x => x.OrganizationUnitId == input.OrganizationUnitId &&
                     x.TenantDepartmentId == input.TenantDepartmentId
            );

            if (ouDepartment != null)
            {

                await _organizationUnitTenantDepartmentRepository.DeleteAsync(ouDepartment);
            }
        }

        public async Task<bool> IsUserInOrganizationUnitAndDepartmentAsync(long userId, long organizationUnitId, Guid departmentId)
        {
            // Check if the user is in the specified department
            var userDepartment = await _tenantDepartmentUserRepository.FirstOrDefaultAsync(tdu =>
                tdu.UserId == userId && tdu.TenantDepartmentId == departmentId);

            if (userDepartment == null)
            {
                return false;
            }

            // Check if the user is in the specified organization unit
            var userOrganizationUnit = await _userOrganizationUnitRepository.FirstOrDefaultAsync(uou =>
                uou.UserId == userId && uou.OrganizationUnitId == organizationUnitId);

            return userOrganizationUnit != null;
        }
        
    }
}

