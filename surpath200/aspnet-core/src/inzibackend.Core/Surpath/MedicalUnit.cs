using inzibackend;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("MedicalUnits")]
    [Audited]
    public class MedicalUnit : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string PrimaryContact { get; set; }

        [Required]
        public virtual string PrimaryContactPhone { get; set; }

        public virtual string PrimaryContactEmail { get; set; }

        [Required]
        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        [Required]
        public virtual string City { get; set; }

        public virtual enumUSStates State { get; set; }

        [Required]
        public virtual string ZipCode { get; set; }

        public virtual int HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

    }
}