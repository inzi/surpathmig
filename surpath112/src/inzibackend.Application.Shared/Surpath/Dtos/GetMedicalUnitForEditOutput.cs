using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetMedicalUnitForEditOutput
    {
        public CreateOrEditMedicalUnitDto MedicalUnit { get; set; }

        public string HospitalName { get; set; }

    }
}