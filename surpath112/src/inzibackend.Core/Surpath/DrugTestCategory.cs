using inzibackend.Surpath;
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
    [Table("DrugTestCategories")]
    [Audited]
    public class DrugTestCategory : FullAuditedEntity<Guid>
    {

        public virtual Guid DrugId { get; set; }

        [ForeignKey("DrugId")]
        public Drug DrugFk { get; set; }

        public virtual Guid PanelId { get; set; }

        [ForeignKey("PanelId")]
        public Panel PanelFk { get; set; }

        public virtual Guid TestCategoryId { get; set; }

        [ForeignKey("TestCategoryId")]
        public TestCategory TestCategoryFk { get; set; }

    }
}