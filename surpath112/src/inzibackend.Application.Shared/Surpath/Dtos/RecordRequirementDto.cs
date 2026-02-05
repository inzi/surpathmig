using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class RecordRequirementDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Metadata { get; set; }

        public bool IsSurpathOnly { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public Guid? SurpathServiceId { get; set; }

        public Guid TenantSurpathServiceId { get; set; }

        public List<RecordCategoryDto> CategoryDTOs { get; set; }
    }
}