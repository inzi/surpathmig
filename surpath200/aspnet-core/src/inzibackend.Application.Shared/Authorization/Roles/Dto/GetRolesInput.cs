using System.Collections.Generic;

namespace inzibackend.Authorization.Roles.Dto;

public class GetRolesInput
{
    public List<string> Permissions { get; set; }
}

