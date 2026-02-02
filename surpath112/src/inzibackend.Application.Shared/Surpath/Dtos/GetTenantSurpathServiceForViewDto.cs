using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantSurpathServiceForViewDto
    {
        public TenantSurpathServiceDto TenantSurpathService { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public string TenantName { get; set; }


    }
}