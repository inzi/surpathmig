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
    [Table("DrugPanels")]
    [Audited]
    public class DrugPanel : FullAuditedEntity<Guid>
    {

        public virtual Guid DrugId { get; set; }

        [ForeignKey("DrugId")]
        public Drug DrugFk { get; set; }

        public virtual Guid PanelId { get; set; }

        [ForeignKey("PanelId")]
        public Panel PanelFk { get; set; }

    }
}