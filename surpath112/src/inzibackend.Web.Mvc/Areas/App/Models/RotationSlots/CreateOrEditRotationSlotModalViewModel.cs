using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RotationSlots
{
    public class CreateOrEditRotationSlotModalViewModel
    {
        public CreateOrEditRotationSlotDto RotationSlot { get; set; }

        public string HospitalName { get; set; }

        public string MedicalUnitName { get; set; }

        public List<RotationSlotHospitalLookupTableDto> RotationSlotHospitalList { get; set; }

        public List<RotationSlotMedicalUnitLookupTableDto> RotationSlotMedicalUnitList { get; set; }

        public bool IsEditMode => RotationSlot.Id.HasValue;
    }
}