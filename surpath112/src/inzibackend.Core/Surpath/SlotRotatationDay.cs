using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("SlotRotationDays")]
    [Audited]
    public class SlotRotationDay : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public DayOfWeek Day { get; set; }
        [ForeignKey("RotationSlotId")]
        public RotationSlot RotationSlotFK { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
