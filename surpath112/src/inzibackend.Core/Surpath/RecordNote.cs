using inzibackend.Surpath;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("RecordNotes")]
    [Audited]
    public class RecordNote : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Note { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual bool AuthorizedOnly { get; set; }

        public virtual bool HostOnly { get; set; }

        public virtual bool SendNotification { get; set; }

        public virtual Guid? RecordStateId { get; set; }

        [ForeignKey("RecordStateId")]
        public RecordState RecordStateFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual long? NotifyUserId { get; set; }

        [ForeignKey("NotifyUserId")]
        public User NotifyUserFk { get; set; }

    }
}