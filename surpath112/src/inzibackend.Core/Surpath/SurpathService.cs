using inzibackend.Surpath;
using inzibackend.Surpath;
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
    [Table("SurpathServices")]
    [Audited]
    public class SurpathService : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Range(SurpathServiceConsts.MinPriceValue, SurpathServiceConsts.MaxPriceValue)]
        public virtual double Price { get; set; }

        [Range(SurpathServiceConsts.MinDiscountValue, SurpathServiceConsts.MaxDiscountValue)]
        public virtual decimal Discount { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsEnabledByDefault { get; set; }

        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual Guid? RecordCategoryRuleId { get; set; }

        [ForeignKey("RecordCategoryRuleId")]
        public RecordCategoryRule RecordCategoryRuleFk { get; set; }

        //public string FeatureName { get; set; }
        public string FeatureIdentifier { get; set; }

    }
}