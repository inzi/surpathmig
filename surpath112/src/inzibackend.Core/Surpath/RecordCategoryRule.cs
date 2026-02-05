using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("RecordCategoryRules")]
    [Audited]
    public class RecordCategoryRule : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Notify { get; set; }

        public virtual int ExpireInDays { get; set; }

        public virtual int WarnDaysBeforeFirst { get; set; }

        public virtual bool Expires { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool IsSurpathOnly { get; set; }

        public virtual int WarnDaysBeforeSecond { get; set; }

        public virtual int WarnDaysBeforeFinal { get; set; }

        public virtual Guid TemplateRuleId { get; set; }

        public virtual string MetaData { get; set; }

        public virtual Guid? FirstWarnStatusId { get; set; }

        [ForeignKey("FirstWarnStatusId")]
        public virtual RecordStatus FirstWarnStatus { get; set; }

        public virtual Guid? SecondWarnStatusId { get; set; }

        [ForeignKey("SecondWarnStatusId")]
        public virtual RecordStatus SecondWarnStatus { get; set; }

        public virtual Guid? FinalWarnStatusId { get; set; }

        [ForeignKey("FinalWarnStatusId")]
        public virtual RecordStatus FinalWarnStatus { get; set; }

        public virtual Guid? ExpiredStatusId { get; set; }

        [ForeignKey("ExpiredStatusId")]
        public virtual RecordStatus ExpiredStatus { get; set; }
    }
}