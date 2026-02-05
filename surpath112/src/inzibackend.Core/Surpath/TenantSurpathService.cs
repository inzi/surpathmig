using inzibackend.Surpath;

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
using Abp.Organizations;

namespace inzibackend.Surpath
{
    [Table("TenantSurpathServices")]
    [Audited]
    public class TenantSurpathService : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Range(TenantSurpathServiceConsts.MinPriceValue, TenantSurpathServiceConsts.MaxPriceValue)]
        public virtual double Price { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsPricingOverrideEnabled { get; set; }

        public virtual Guid? SurpathServiceId { get; set; }

        [ForeignKey("SurpathServiceId")]
        public SurpathService SurpathServiceFk { get; set; }

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

        public virtual long? OrganizationUnitId { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit OrganizationUnitFk { get; set; }

        public virtual Guid? CohortUserId { get; set; }

        [ForeignKey("CohortUserId")]
        public CohortUser CohortUserFk { get; set; }

        public virtual bool IsInvoiced { get; set; }
    }
}