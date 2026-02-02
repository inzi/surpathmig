using inzibackend.Dto;

namespace inzibackend.Organizations.Dto;

public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
{
    public long OrganizationUnitId { get; set; }
}

