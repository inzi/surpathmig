using Abp.AutoMapper;
using inzibackend.Authorization.Roles.Dto;
using inzibackend.Web.Areas.App.Models.Common;

namespace inzibackend.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}