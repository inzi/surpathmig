using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string InstructionsFilter { get; set; }

        public string RecordRequirementNameFilter { get; set; }

        public string RecordCategoryRuleNameFilter { get; set; }

        public Guid? RecordRequirementIdFilter { get; set; }
    }
}