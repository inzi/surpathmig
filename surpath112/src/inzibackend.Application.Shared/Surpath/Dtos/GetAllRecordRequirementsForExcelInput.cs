using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordRequirementsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string MetadataFilter { get; set; }

        public int? IsSurpathOnlyFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

        public string CohortNameFilter { get; set; }

        public string SurpathServiceNameFilter { get; set; }

        public string TenantSurpathServiceNameFilter { get; set; }

    }
}