using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordStateForEditOutput
    {
        public CreateOrEditRecordStateDto RecordState { get; set; }

        public string Recordfilename { get; set; }

        public string RecordCategoryName { get; set; }
        public string RecordRequirementName { get; set; }

        public string UserName { get; set; }

        public string RecordStatusStatusName { get; set; }
        public int? TenantId { get; set; }
        public bool IsSurpathOnly { get; set; } = false;

    }
}