using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordCategories
{
    public class CreateOrEditRecordCategoryModalViewModel
    {
        public CreateOrEditRecordCategoryDto RecordCategory { get; set; }

        public string RecordRequirementName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public List<RecordCategoryRecordCategoryRuleLookupTableDto> RecordCategoryRecordCategoryRuleList { get; set; }

        public bool IsEditMode => RecordCategory.Id.HasValue;
        public int RecordCategoryRecordStateCount { get; set; } = 0;
        public bool HideRequirementLookup { get; set; } = false;

    }
}