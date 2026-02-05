using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("Panels")]
    [Audited]
    public class Panel : FullAuditedEntity<Guid>
    {

        [Required]
        [StringLength(PanelConsts.MaxNameLength, MinimumLength = PanelConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Range(PanelConsts.MinCostValue, PanelConsts.MaxCostValue)]
        public virtual double Cost { get; set; }

        [StringLength(PanelConsts.MaxDescriptionLength, MinimumLength = PanelConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual Guid? TestCategoryId { get; set; }

        [ForeignKey("TestCategoryId")]
        public TestCategory TestCategoryFk { get; set; }

    }
}