using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Drugs
{
    public class CreateOrEditDrugViewModel
    {
        public CreateOrEditDrugDto Drug { get; set; }

        public bool IsEditMode => Drug.Id.HasValue;
    }
}