using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordRequirements
{
    public class CreateOrEditRecordRequirementModalViewModel
    {
        public CreateOrEditRecordRequirementDto RecordRequirement { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

        //public List<RecordRequirementSurpathServiceLookupTableDto> RecordRequirementSurpathServiceList { get; set; }

        //public List<RecordRequirementTenantSurpathServiceLookupTableDto> RecordRequirementTenantSurpathServiceList { get; set; }

        public bool IsEditMode => RecordRequirement.Id.HasValue;
    }
}