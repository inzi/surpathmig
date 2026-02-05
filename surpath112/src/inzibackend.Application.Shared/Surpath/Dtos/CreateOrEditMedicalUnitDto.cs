using inzibackend;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditMedicalUnitDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string PrimaryContact { get; set; }

        [Required]
        public string PrimaryContactPhone { get; set; }

        public string PrimaryContactEmail { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        public enumUSStates State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public int HospitalId { get; set; }

    }
}