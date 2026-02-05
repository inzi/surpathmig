using System.Collections.Generic;
using Abp.Application.Services.Dto;
using inzibackend.Authorization.Permissions.Dto;
using inzibackend.Web.Areas.App.Models.Common;

namespace inzibackend.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}