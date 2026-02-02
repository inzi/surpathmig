using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.ConfirmationValues
{
    public class CreateOrEditConfirmationValueViewModel
    {
        public CreateOrEditConfirmationValueDto ConfirmationValue { get; set; }

        public string DrugName { get; set; }

        public string TestCategoryName { get; set; }

        public List<ConfirmationValueTestCategoryLookupTableDto> ConfirmationValueTestCategoryList { get; set; }

        public bool IsEditMode => ConfirmationValue.Id.HasValue;
    }
}