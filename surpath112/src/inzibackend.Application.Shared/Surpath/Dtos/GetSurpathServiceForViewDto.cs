using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetSurpathServiceForViewDto
    {
        public SurpathServiceDto SurpathService { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public long? TenantId { get; set; }

        public bool IsEnabled { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}