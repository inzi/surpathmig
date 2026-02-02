using inzibackend.Dto;

namespace inzibackend.Organizations.Dto
{
    public class FindOrganizationUnitTenantDepartmentInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}