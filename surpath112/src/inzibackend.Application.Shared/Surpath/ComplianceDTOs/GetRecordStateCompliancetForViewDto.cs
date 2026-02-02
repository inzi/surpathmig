using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordStateCompliancetForViewDto
    {
        public RecordRequirementDto RecordRequirement { get; set; }
        public RecordCategoryDto RecordCategory { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        // public string SurpathServiceName { get; set; }
        public SurpathServiceDto SurpathService { get; set; }
        public TenantSurpathServiceDto TenantSurpathServiceDto { get; set; }
        public GetRecordStateForViewDto GetRecordStateForViewDto { get; set; }
        public int RecCount { get; set; } = 1;
        public bool IsParentRow { get; set; } = false;
        public bool IsChildRow { get; set; } = false;
    }

    //public class RecordRequirementComplianceViewDto : EntityDto<Guid>
    //{
    //    public string Name { get; set; }

    //    public string Description { get; set; }

    //    public string Metadata { get; set; }

    //    public bool IsSurpathOnly { get; set; }

    //    public Guid? TenantDepartmentId { get; set; }

    //    public Guid? CohortId { get; set; }

    //    public Guid? SurpathServiceId { get; set; }

    //    public List<RecordCategoryDto> RecordCategory { get; set; }

    //}
}