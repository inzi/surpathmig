using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordCategoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Instructions { get; set; }

        public Guid? RecordRequirementId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }
        public bool IsSurpathService { get; set; } = false;
        public long? TenantId { get; set; }
        public RecordCategoryRuleDto RecordCategoryRule { get; set; }
    }
}