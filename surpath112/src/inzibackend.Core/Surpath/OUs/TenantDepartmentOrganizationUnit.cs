using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace inzibackend.Surpath.OUs
{
    [Table("TenantDepartmentOrganizationUnits")]
    [Audited]
    public class TenantDepartmentOrganizationUnit : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public Guid TenantDepartmentId{ get; set; }

        public long OrganizationUnitId { get; set; }

        public TenantDepartmentOrganizationUnit()
        {
        }
    }
}
