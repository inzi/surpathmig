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
    [Table("RecordCategories")]
    [Audited]
    public class RecordCategory : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Instructions { get; set; }

        public virtual Guid? RecordRequirementId { get; set; }

        [ForeignKey("RecordRequirementId")]
        public RecordRequirement RecordRequirementFk { get; set; }

        public virtual Guid? RecordCategoryRuleId { get; set; }

        [ForeignKey("RecordCategoryRuleId")]
        public RecordCategoryRule RecordCategoryRuleFk { get; set; }

    }
}