using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class GetCohortForComplianceViewDto
    {
        public CohortComplianceDto Cohort { get; set; }
        public int ComplianceRecords { get; set; }
        public string TenantDepartmentName { get; set; }

    }

}