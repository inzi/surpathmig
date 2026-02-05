using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDepartmentForComplianceViewDto
    {
        public TenantDepartmentDto TenantDepartment { get; set; }
        public List<ComplianceCohortTotalsForViewDto> ComplianceSummary { get; set; }
        public int ComplianceRecords { get; set; }

    }

}