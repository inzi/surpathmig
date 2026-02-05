using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("CodeTypes")]
    [Audited]
    public class CodeType : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CodeTypeConsts.MaxNameLength, MinimumLength = CodeTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}