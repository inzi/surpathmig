using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("Drugs")]
    [Audited]
    public class Drug : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(DrugConsts.MaxNameLength, MinimumLength = DrugConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(DrugConsts.MaxCodeLength, MinimumLength = DrugConsts.MinCodeLength)]
        public virtual string Code { get; set; }

    }
}