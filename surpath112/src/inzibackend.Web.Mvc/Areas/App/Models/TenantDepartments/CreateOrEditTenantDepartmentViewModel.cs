using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TenantDepartments
{
    public class CreateOrEditTenantDepartmentModalViewModel
    {
        public CreateOrEditTenantDepartmentDto TenantDepartment { get; set; }

        public bool IsEditMode => TenantDepartment.Id.HasValue;
        public string FilterText { get; set; }
        public bool confirm { get; set; } = false;
    }
}