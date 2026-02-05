using System;
namespace surpath200.Core.DTOs
{
    public class CohortDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultCohort { get; set; }
        public Guid? TenantDepartmentId { get; set; }
    }
}