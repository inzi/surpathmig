using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordStatusDto : EntityDto<Guid?>
    {

        [Required]
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