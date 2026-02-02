using System.Collections.Generic;
using inzibackend.Authorization.Permissions.Dto;

namespace inzibackend.Authorization.Users.Dto;

public class GetUserPermissionsForEditOutput
{
    public List<FlatPermissionDto> Permissions { get; set; }

    public List<string> GrantedPermissionNames { get; set; }
}

