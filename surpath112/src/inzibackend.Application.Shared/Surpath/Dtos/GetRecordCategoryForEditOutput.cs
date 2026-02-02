using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordCategoryForEditOutput
    {
        public CreateOrEditRecordCategoryDto RecordCategory { get; set; }

        public string RecordRequirementName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public int RecordCategoryRecordStateCount { get; set; } = 0;
    }
}