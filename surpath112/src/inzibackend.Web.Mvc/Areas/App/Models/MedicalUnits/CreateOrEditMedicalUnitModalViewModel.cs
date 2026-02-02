using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.MedicalUnits
{
    public class CreateOrEditMedicalUnitModalViewModel
    {
        public CreateOrEditMedicalUnitDto MedicalUnit { get; set; }

        public string HospitalName { get; set; }

        public List<MedicalUnitHospitalLookupTableDto> MedicalUnitHospitalList { get; set; }

        public bool IsEditMode => MedicalUnit.Id.HasValue;
    }
}