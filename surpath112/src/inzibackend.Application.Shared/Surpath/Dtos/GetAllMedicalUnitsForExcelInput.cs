using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllMedicalUnitsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string PrimaryContactFilter { get; set; }

        public string PrimaryContactPhoneFilter { get; set; }

        public string PrimaryContactEmailFilter { get; set; }

        public string Address1Filter { get; set; }

        public string Address2Filter { get; set; }

        public string CityFilter { get; set; }

        public int? StateFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string HospitalNameFilter { get; set; }

    }
}