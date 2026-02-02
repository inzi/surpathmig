using System;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Organizations.Dto
{
    public class TenantDepartmentsToOrganizationUnitInput
    {
        public Guid[] TenantDepartmentIds { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}