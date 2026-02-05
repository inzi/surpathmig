namespace inzibackend.Surpath.Dtos
{
    public class GetCohortForViewDto
    {
        public CohortDto Cohort { get; set; }

        public string TenantDepartmentName { get; set; }
        public string TenantName { get; set; }
        public int TenantId { get; set; }
        public bool ExpandAndSearch { get; set; } = false;
        public int CohortusersCount { get; set; } = 0;
    }
}