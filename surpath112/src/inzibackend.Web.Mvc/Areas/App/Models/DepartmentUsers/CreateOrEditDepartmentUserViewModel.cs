using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.DepartmentUsers
{
    public class CreateOrEditDepartmentUserViewModel
    {
        public CreateOrEditDepartmentUserDto DepartmentUser { get; set; }

        public string UserName { get; set; }

        public string TenantDepartmentName { get; set; }

        public bool IsEditMode => DepartmentUser.Id.HasValue;
    }
}