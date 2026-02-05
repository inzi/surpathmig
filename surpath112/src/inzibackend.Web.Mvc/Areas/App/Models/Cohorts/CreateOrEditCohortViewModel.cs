using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Cohorts
{
    public class CreateOrEditCohortModalViewModel
    {
        public CreateOrEditCohortDto Cohort { get; set; }

        public string TenantDepartmentName { get; set; }

        public bool IsEditMode => Cohort.Id.HasValue;
    }
}