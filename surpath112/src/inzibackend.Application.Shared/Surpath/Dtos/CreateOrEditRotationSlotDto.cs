using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRotationSlotDto : EntityDto<int?>
    {

        [Required]
        public string SlotId { get; set; } = RotationSlotConsts.DefaultSlotId;


        //[Range(RotationSlotConsts.MinAvailableSlotsValue, RotationSlotConsts.MaxAvailableSlotsValue)]
        public int AvailableSlots { get; set; }

        public DateTime ShiftStartDate { get; set; }

        public DateTime ShiftEndDate { get; set; }

        public DateTime ShiftStartTime { get; set; }

        public DateTime ShiftEndTime { get; set; }

        public decimal ShiftHours { get; set; }

        public bool NotifyHospital { get; set; }

        public DateTime? HospitalNotifiedDateTime { get; set; }

        public EnumSlotShiftType ShiftType { get; set; }

        public DateTime BidStartDateTime { get; set; }

        public DateTime BidEndDateTime { get; set; }

        public int HospitalId { get; set; }

        public int MedicalUnitId { get; set; }

        public List<SlotAvailableDayDto> SlotAvailableDays { get; set; }
        public List<SlotRotationDayDto> SlotRotationDays { get; set; }
    }
}