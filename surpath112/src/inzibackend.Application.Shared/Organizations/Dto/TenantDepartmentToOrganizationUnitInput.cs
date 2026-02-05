using System;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Organizations.Dto
{
  
    public class TenantDepartmentToOrganizationUnitInput
    {
        public Guid TenantDepartmentId { get; set; }

        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}