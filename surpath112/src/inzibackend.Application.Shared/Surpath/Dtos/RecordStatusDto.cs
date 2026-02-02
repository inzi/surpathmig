using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordStatusDto : EntityDto<Guid>
    {
        public string StatusName { get; set; }

        public string HtmlColor { get; set; }

        public string CSSCLass { get; set; }

        public bool IsDefault { get; set; }

        public bool RequireNoteOnSet { get; set; }

        public bool IsSurpathServiceStatus { get; set; }

        public EnumStatusComplianceImpact ComplianceImpact { get; set; }

        public Guid? TenantDepartmentId { get; set; }

    }
}