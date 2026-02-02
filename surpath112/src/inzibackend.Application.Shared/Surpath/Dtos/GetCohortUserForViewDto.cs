using inzibackend.Authorization.Users.Dto;
using inzibackend.MultiTenancy.Dto;
using inzibackend.Surpath.Compliance;

namespace inzibackend.Surpath.Dtos
{
    public class GetCohortUserForViewDto
    {
        public CohortUserDto CohortUser { get; set; }

        public string CohortDescription { get; set; }
        public string CohortName { get; set; }

        public string UserName { get; set; }

        public UserEditDto UserEditDto { get; set; }

        public TenantDepartmentDto TenantDepartmentDto { get; set; }

        public TenantEditDto TenantEditDto { get; set; }

        public bool DrugScreenCompliance { get; set; } = false;
        public bool BackgroundCheckCompliance { get; set; } = false;
        public bool ImmunizationCompliance { get; set; } = false;


        public ComplianceValues ComplianceValues { get; set; } = new ComplianceValues();
    }

}