using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordStatusesForExcelInput
    {
        public string Filter { get; set; }

        public string StatusNameFilter { get; set; }

        public string HtmlColorFilter { get; set; }

        public string CSSCLassFilter { get; set; }

        public int? IsDefaultFilter { get; set; }

        public int? RequireNoteOnSetFilter { get; set; }

        public int? IsSurpathServiceStatusFilter { get; set; }

        public int? ComplianceImpactFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

    }
}