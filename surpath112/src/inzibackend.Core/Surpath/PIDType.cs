using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("PidTypes")]
    [Audited]
    public class PidType : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool MaskPid { get; set; }

        public virtual string PidRegex { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime ModifiedOn { get; set; }

        public virtual long CreatedBy { get; set; }

        public virtual long LastModifiedBy { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string PidInputMask { get; set; }

        public virtual bool Required { get; set; }

    }
}