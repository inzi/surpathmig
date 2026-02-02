using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Hospitals
{
    public class CreateOrEditHospitalViewModel
    {
        public CreateOrEditHospitalDto Hospital { get; set; }

        public bool IsEditMode => Hospital.Id.HasValue;
    }
}