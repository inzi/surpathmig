using inzibackend.Surpath;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace inzibackend.Authorization.Organizations
{
    public interface IOUSecurityManager
    {
        Task AddTenantDepartmentToOUAsync(Guid tenantDepartmentId, long organizationUnitId);
        Task<long> GetOUOfTenantDepartmentAsync(Guid tenantDepartmentId);
        Task<bool> IsTenantDepartmentInOUAsync(Guid tenantDepartmentId, long organizationUnitId);
        Task RemoveTenantDepartmentFromOUAsync(Guid tenantDepartmentId, long organizationUnitId);
        IQueryable<TenantDepartment> ApplyTenantDepartmentVisibilityFilter(IQueryable<TenantDepartment> query, long userId);
        IQueryable<TenantDepartmentUser> ApplyTenantDepartmentUserVisibilityFilter(IQueryable<TenantDepartmentUser> query, long userId);
        IQueryable<Cohort> ApplyTenantDepartmentCohortVisibilityFilter(IQueryable<Cohort> query, long userId);
        IQueryable<CohortUser> ApplyTenantDepartmentCohortUserVisibilityFilter(IQueryable<CohortUser> query, long userId);


    }
}