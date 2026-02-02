using inzibackend.Surpath.Dtos;
using inzibackend.Web.Areas.App.Models.RecordCategories;
using inzibackend.Web.Areas.App.Models.RecordRequirements;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.Compliance
{
    public class CreateRequirementViewModel
    {
        public CreateOrEditRecordRequirementModalViewModel CreateOrEditRecordRequirement { get; set; }
        public List<CreateOrEditRecordCategoryModalViewModel> CreateOrEditRecordCategories { get; set; } = new List<CreateOrEditRecordCategoryModalViewModel>();
        public List<RecordCategoryRecordCategoryRuleLookupTableDto> RecordCategoryRecordCategoryRuleList { get; set; }
        public int? step { get; set; }

    }
}
