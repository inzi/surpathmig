namespace inzibackend.Web.Areas.App.Models.TenantDepartments
{
    public class TenantDepartmentLookupTableViewModel
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string FilterText { get; set; }
        public bool confirm { get; set; } = false;

    }
}