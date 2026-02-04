using inzibackend.Surpath;
using inzibackend.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("UserPids")]
    [Audited]
    public class UserPid : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Pid { get; set; }

        public virtual bool Validated { get; set; }

        public virtual Guid PidTypeId { get; set; }

        [ForeignKey("PidTypeId")]
        public PidType PidTypeFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}