using inzibackend.Surpath.DTOBases;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordRequirementForViewDto: GenericDTOBase
    {
        public RecordRequirementDto RecordRequirement { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

    }
}