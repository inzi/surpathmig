using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordCategoryRules
{
    public class CreateOrEditRecordCategoryRuleViewModel
    {
        public CreateOrEditRecordCategoryRuleDto RecordCategoryRule { get; set; }

        public List<RecordStatusDto> RecordStatusList { get; set; }

        public bool IsEditMode => RecordCategoryRule.Id.HasValue;
    }
}