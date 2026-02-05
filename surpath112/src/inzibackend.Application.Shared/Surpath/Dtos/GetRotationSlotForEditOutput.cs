using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRotationSlotForEditOutput
    {
        public CreateOrEditRotationSlotDto RotationSlot { get; set; }

        public string HospitalName { get; set; }

        public string MedicalUnitName { get; set; }

    }
}