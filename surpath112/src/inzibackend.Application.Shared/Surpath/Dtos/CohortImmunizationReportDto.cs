using System;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class CohortImmunizationReportDto
    {
        public string CohortName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ImmunizationRequirementDto> ImmunizationRequirements { get; set; } = new List<ImmunizationRequirementDto>();
    }

    public class ImmunizationRequirementDto
    {
        public string RequirementName { get; set; }
        public string CategoryName { get; set; }
        public string ComplianceStatus { get; set; }
        public DateTime? AdministeredDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string StatusColor { get; set; }
    }
} 