using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.DrugTestCategories
{
    public class CreateOrEditDrugTestCategoryViewModel
    {
        public CreateOrEditDrugTestCategoryDto DrugTestCategory { get; set; }

        public string DrugName { get; set; }

        public string PanelName { get; set; }

        public string TestCategoryName { get; set; }

        public List<DrugTestCategoryTestCategoryLookupTableDto> DrugTestCategoryTestCategoryList { get; set; }

        public bool IsEditMode => DrugTestCategory.Id.HasValue;
    }
}