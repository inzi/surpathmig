using inzibackend.Surpath;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("DeptCodes")]
    [Audited]
    public class DeptCode : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(DeptCodeConsts.MaxCodeLength, MinimumLength = DeptCodeConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        public virtual Guid CodeTypeId { get; set; }

        [ForeignKey("CodeTypeId")]
        public CodeType CodeTypeFk { get; set; }

        public virtual Guid TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

    }
}