using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TestCategories
{
    public class CreateOrEditTestCategoryViewModel
    {
        public CreateOrEditTestCategoryDto TestCategory { get; set; }

        public bool IsEditMode => TestCategory.Id.HasValue;
    }
}