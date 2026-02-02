using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Dtos
{
    public class RequirementCreationDto
    {

        // the requirement
        public CreateOrEditRecordRequirementDto RecordRequirement { get; set; }
        // the steps
        public List<GetRecordCategoryForEditOutput> RecordCategories { get; set; }

        public string RecordRequirementName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public List<RecordCategoryRecordCategoryRuleLookupTableDto> RecordCategoryRecordCategoryRuleList { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }
        public bool IsEditMode => RecordRequirement.Id.HasValue;
    }
}
