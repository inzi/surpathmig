using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRotationSlotsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string SlotIdFilter { get; set; }

        public int? MaxAvailableSlotsFilter { get; set; }
        public int? MinAvailableSlotsFilter { get; set; }

        public DateTime? MaxShiftStartDateFilter { get; set; }
        public DateTime? MinShiftStartDateFilter { get; set; }

        public DateTime? MaxShiftEndDateFilter { get; set; }
        public DateTime? MinShiftEndDateFilter { get; set; }

        public DateTime? MaxShiftStartTimeFilter { get; set; }
        public DateTime? MinShiftStartTimeFilter { get; set; }

        public DateTime? MaxShiftEndTimeFilter { get; set; }
        public DateTime? MinShiftEndTimeFilter { get; set; }

        public decimal? MaxShiftHoursFilter { get; set; }
        public decimal? MinShiftHoursFilter { get; set; }

        public int? NotifyHospitalFilter { get; set; }

        public DateTime? MaxHospitalNotifiedDateTimeFilter { get; set; }
        public DateTime? MinHospitalNotifiedDateTimeFilter { get; set; }

        public int? ShiftTypeFilter { get; set; }

        public DateTime? MaxBidStartDateTimeFilter { get; set; }
        public DateTime? MinBidStartDateTimeFilter { get; set; }

        public DateTime? MaxBidEndDateTimeFilter { get; set; }
        public DateTime? MinBidEndDateTimeFilter { get; set; }

        public string HospitalNameFilter { get; set; }

        public string MedicalUnitNameFilter { get; set; }

    }
}