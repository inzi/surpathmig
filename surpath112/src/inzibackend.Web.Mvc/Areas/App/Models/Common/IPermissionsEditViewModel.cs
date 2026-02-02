using System.Collections.Generic;
using inzibackend.Authorization.Permissions.Dto;

namespace inzibackend.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}