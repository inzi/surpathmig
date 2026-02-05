using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.CohortUsers
{
    public class MasterDetailChild_Cohort_CreateOrEditCohortUserModalViewModel
    {
        public CreateOrEditCohortUserDto CohortUser { get; set; }

        public string UserName { get; set; }

        public bool IsEditMode => CohortUser.Id.HasValue;
    }
}