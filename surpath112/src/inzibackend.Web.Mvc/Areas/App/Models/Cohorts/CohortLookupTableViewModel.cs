using inzibackend.Surpath.DTOBases;
using System; 

namespace inzibackend.Web.Areas.App.Models.Cohorts
{
    public class CohortLookupTableViewModel :GenericDTOBase
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string FilterText { get; set; }
        public bool confirm { get; set; } = false;

        public Guid? TenantDepartmentId { get; set; }

    }
}