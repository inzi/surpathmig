using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordRequirementDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Metadata { get; set; }

        public bool IsSurpathOnly { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public Guid? SurpathServiceId { get; set; }

        public Guid TenantSurpathServiceId { get; set; }

    }
}