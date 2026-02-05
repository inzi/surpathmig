using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("TenantDepartmentUsers")]
    [Audited]
    public class TenantDepartmentUser : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual Guid TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

    }
}