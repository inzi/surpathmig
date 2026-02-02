using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordCategoryForViewDto
    {
        public RecordCategoryDto RecordCategory { get; set; }

        public string RecordRequirementName { get; set; }

        public string RecordCategoryRuleName { get; set; }

    }
}