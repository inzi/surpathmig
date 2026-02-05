using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("RecordRequirements")]
    [Audited]
    public class RecordRequirement : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Metadata { get; set; }

        public virtual bool IsSurpathOnly { get; set; }

        public virtual Guid? TenantDepartmentId { get; set; }

        [ForeignKey("TenantDepartmentId")]
        public TenantDepartment TenantDepartmentFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }

        public virtual Guid? SurpathServiceId { get; set; }

        //[ForeignKey("SurpathServiceId")]
        public SurpathService? SurpathServiceFk { get; set; }

        public virtual Guid? TenantSurpathServiceId { get; set; }

        //[ForeignKey("TenantSurpathServiceId")]
        public TenantSurpathService? TenantSurpathServiceFk { get; set; }

    }
}