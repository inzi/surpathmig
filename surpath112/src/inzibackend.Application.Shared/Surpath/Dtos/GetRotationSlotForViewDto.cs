using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRotationSlotForViewDto
    {
        public RotationSlotDto RotationSlot { get; set; }

        public string HospitalName { get; set; }

        public string MedicalUnitName { get; set; }

    }
}