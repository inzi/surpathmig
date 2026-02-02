using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TenantDepartmentUsers
{
    public class CreateOrEditTenantDepartmentUserModalViewModel
    {
        public CreateOrEditTenantDepartmentUserDto TenantDepartmentUser { get; set; }

        public string UserName { get; set; }

        public string TenantDepartmentName { get; set; }

        public bool IsEditMode => TenantDepartmentUser.Id.HasValue;
    }
}