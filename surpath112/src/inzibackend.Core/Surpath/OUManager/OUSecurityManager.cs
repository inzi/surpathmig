using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Organizations;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using inzibackend.Surpath.OUs;
using Microsoft.AspNetCore.Identity;

namespace inzibackend.Authorization.Organizations
{
    public class OUSecurityManager : DomainService, IOUSecurityManager
    {

        private readonly IRepository<TenantDepartmentOrganizationUnit, Guid> _tenantDepartmentOURepository;

        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<TenantDepartmentOrganizationUnit, Guid> _organizationUnitTenantDepartmentRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;

        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public OUSecurityManager(
            IRepository<Cohort, Guid> cohortRepository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<TenantDepartmentOrganizationUnit, Guid> TenantDepartmentOURepository,
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<TenantDepartmentOrganizationUnit, Guid> organizationUnitTenantDepartmentRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _roleManager = roleManager;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _organizationUnitTenantDepartmentRepository = organizationUnitTenantDepartmentRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _tenantDepartmentOURepository = TenantDepartmentOURepository;
            _userManager = userManager;
            _cohortRepository = cohortRepository;
        }

        /// Add TenantDepartment to OU
        public async Task AddTenantDepartmentToOUAsync(Guid tenantDepartmentId, long organizationUnitId)
        {
            var tenantDepartmentOU = new TenantDepartmentOrganizationUnit
            {
                TenantDepartmentId = tenantDepartmentId,
                OrganizationUnitId = organizationUnitId
            };
            await _tenantDepartmentOURepository.InsertAsync(tenantDepartmentOU);
        }

        /// Remove TenantDepartment from OU
        public async Task RemoveTenantDepartmentFromOUAsync(Guid tenantDepartmentId, long organizationUnitId)
        {
            var tenantDepartmentOU = await _tenantDepartmentOURepository.FirstOrDefaultAsync(tdo => tdo.TenantDepartmentId == tenantDepartmentId && tdo.OrganizationUnitId == organizationUnitId);
            if (tenantDepartmentOU != null)
            {
                await _tenantDepartmentOURepository.DeleteAsync(tenantDepartmentOU);
            }
        }

        /// Get if TenantDepartment is in OU
        public async Task<bool> IsTenantDepartmentInOUAsync(Guid tenantDepartmentId, long organizationUnitId)
        {
            return await _tenantDepartmentOURepository.FirstOrDefaultAsync(tdo => tdo.TenantDepartmentId == tenantDepartmentId && tdo.OrganizationUnitId == organizationUnitId) != null;
        }

        /// Get OU of TenantDepartment
        public async Task<long> GetOUOfTenantDepartmentAsync(Guid tenantDepartmentId)
        {
            var tenantDepartmentOU = await _tenantDepartmentOURepository.FirstOrDefaultAsync(tdo => tdo.TenantDepartmentId == tenantDepartmentId);
            return tenantDepartmentOU.OrganizationUnitId;
        }

        public IQueryable<TenantDepartment> ApplyTenantDepartmentVisibilityFilter(IQueryable<TenantDepartment> query, long userId)
        {
            var userOUs = _userOrganizationUnitRepository
                .GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationUnitId)
            .ToList();
            if (!userOUs.Any())
            {
                return query;
            }
            var visibleDepartmentIds = _tenantDepartmentOURepository
                .GetAll()
                .Where(x => userOUs.Contains(x.OrganizationUnitId))
                .Select(x => x.TenantDepartmentId)
                .ToList();

            var visibleDepartments = query.Where(x => visibleDepartmentIds.Contains(x.Id));

            return visibleDepartments;
        }
        public IQueryable<TenantDepartmentUser> ApplyTenantDepartmentUserVisibilityFilter(IQueryable<TenantDepartmentUser> query, long userId)
        {
            var userOUs = _userOrganizationUnitRepository
                .GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationUnitId)
                .ToList();
            if (!userOUs.Any())
            {
                return query;
            }
            var visibleDepartmentIds = _tenantDepartmentOURepository
                .GetAll()
                .Where(x => userOUs.Contains(x.OrganizationUnitId))
                .Select(x => (Guid?)x.TenantDepartmentId)
                .ToList();

            return query.Where(x => visibleDepartmentIds.Contains(x.TenantDepartmentId));
        }

        //public IQueryable<TenantDepartmentUser> ApplyTenantDepartmentUserVisibilityFilter(IQueryable<TenantDepartmentUser> query, long userId)
        //{
        //    return ApplyVisibilityFilter(query, userId);
        //}

        public IQueryable<Cohort> ApplyTenantDepartmentCohortVisibilityFilter(IQueryable<Cohort> query, long userId)
        {

            var userOUs = _userOrganizationUnitRepository
                .GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationUnitId)
                .ToList();
            if (!userOUs.Any())
            {
                return query;
            }
            var visibleDepartmentIds = _tenantDepartmentOURepository
                .GetAll()
                .Where(x => userOUs.Contains(x.OrganizationUnitId))
                .Select(x => x.TenantDepartmentId)
                .ToList();

            var cohortQuery = _cohortRepository.GetAll()
                .Where(c => visibleDepartmentIds.Contains(c.TenantDepartmentId.Value));

            var filteredCohortUsers = from cu in query
                                      join c in cohortQuery on cu.Id equals c.Id
                                      select cu;

            return filteredCohortUsers;
        }
        public IQueryable<CohortUser> ApplyTenantDepartmentCohortUserVisibilityFilter(IQueryable<CohortUser> query, long userId)
        {
            var userOUs = _userOrganizationUnitRepository
                .GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationUnitId)
                .ToList();
            if (!userOUs.Any())
            {
                return query;
            }
            var visibleDepartmentIds = _tenantDepartmentOURepository
                .GetAll()
                .Where(x => userOUs.Contains(x.OrganizationUnitId))
                .Select(x => x.TenantDepartmentId)
                .ToList();

            var cohortQuery = _cohortRepository.GetAll()
                .Where(c => visibleDepartmentIds.Contains(c.TenantDepartmentId.Value));

            var filteredCohortUsers = from cu in query
                                      join c in cohortQuery on cu.CohortId equals c.Id
                                      select cu;

            return filteredCohortUsers;

        }
                
    }
}
