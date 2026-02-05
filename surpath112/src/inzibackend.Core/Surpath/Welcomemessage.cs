using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("Welcomemessages")]
    [Audited]
    public class Welcomemessage : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Message { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual DateTime DisplayStart { get; set; }

        public virtual DateTime DisplayEnd { get; set; }

    }
}