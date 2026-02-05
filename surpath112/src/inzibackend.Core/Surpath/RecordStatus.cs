using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("RecordStatuses")]
    [Audited]
    public class RecordStatus : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string StatusName { get; set; }

        public virtual string HtmlColor { get; set; }

        public virtual string CSSCLass { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual bool RequireNoteOnSet { get; set; }

        public virtual bool IsSurpathServiceStatus { get; set; }

        public virtual EnumStatusComplianceImpact ComplianceImpact { get; set; }

        public virtual Guid TemplateServiceId { get; set; }

        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

    }
}