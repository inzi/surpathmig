using inzibackend.Surpath;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using inzibackend.Surpath.Statics;
using System.Linq;

namespace inzibackend.Surpath
{
    [Table("RotationSlots")]
    [Audited]
    public class RotationSlot : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string SlotId { get; set; }

        //[Range(RotationSlotConsts.MinAvailableSlotsValue, RotationSlotConsts.MaxAvailableSlotsValue)]
        public virtual int AvailableSlots { get; set; }

        public virtual DateTime ShiftStartDate { get; set; }

        public virtual DateTime ShiftEndDate { get; set; }

        public virtual DateTime ShiftStartTime { get; set; }

        public virtual DateTime ShiftEndTime { get; set; }

        public virtual decimal ShiftHours { get; set; }

        public virtual bool NotifyHospital { get; set; }

        public virtual DateTime? HospitalNotifiedDateTime { get; set; }

        public virtual EnumSlotShiftType ShiftType { get; set; }

        public virtual DateTime BidStartDateTime { get; set; }

        public virtual DateTime BidEndDateTime { get; set; }

        public virtual int HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

        public virtual int MedicalUnitId { get; set; }

        [ForeignKey("MedicalUnitId")]
        public MedicalUnit MedicalUnitFk { get; set; }
        public ICollection<SlotAvailableDay> SlotAvailableDays { get; set; } = new List<SlotAvailableDay>();
        public ICollection<SlotRotationDay> SlotRotationDays { get; set; } /// = new List<SlotRotationDay>();

        public void UpdateAvailableDays(List<SlotAvailableDay> slotAvailableDays)
        {
            foreach (var _day in SlotAvailableDays)
            {
                if (slotAvailableDays.Any(d => d.Day == _day.Day))
                {
                    var _slot_day = slotAvailableDays.FirstOrDefault(d => d.Day == _day.Day);
                    _day.Day = _slot_day.Day;
                    _day.IsSelected = _slot_day.IsSelected;
                }
            }
        }
        public void UpdateRotationDays(List<SlotRotationDay> slotRotationDays)
        {
            foreach (var _day in SlotRotationDays)
            {
                if (slotRotationDays.Any(d => d.Day == _day.Day))
                {
                    var _slot_day = slotRotationDays.FirstOrDefault(d => d.Day == _day.Day);
                    _day.Day = _slot_day.Day;
                    _day.IsSelected = _slot_day.IsSelected;
                }
            }
            SlotRotationDays = slotRotationDays;
        }

    }
}