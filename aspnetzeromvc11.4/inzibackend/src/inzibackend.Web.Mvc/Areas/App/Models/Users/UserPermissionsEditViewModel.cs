using Abp.AutoMapper;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Web.Areas.App.Models.Common;

namespace inzibackend.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}