using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using Abp.Organizations;

namespace inzibackend.Surpath
{
    [Table("TenantDepartments")]
    [Audited]
    public class TenantDepartment : FullAuditedEntity<Guid>, IMayHaveTenant, IMustHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual bool Active { get; set; }

        public virtual EnumClientMROTypes MROType { get; set; }

        public virtual string Description { get; set; }

        public virtual long OrganizationUnitId { get; set; }

    }
}