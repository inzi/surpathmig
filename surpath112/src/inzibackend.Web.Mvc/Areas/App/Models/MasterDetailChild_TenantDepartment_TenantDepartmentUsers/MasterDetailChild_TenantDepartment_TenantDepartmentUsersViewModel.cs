using System;

namespace inzibackend.Web.Areas.App.Models.TenantDepartmentUsers
{
    public class MasterDetailChild_TenantDepartment_TenantDepartmentUsersViewModel
    {
        public string FilterText { get; set; }

        public Guid TenantDepartmentId { get; set; }
    }
}