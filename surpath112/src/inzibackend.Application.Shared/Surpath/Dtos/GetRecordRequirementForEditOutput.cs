using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordRequirementForEditOutput
    {
        public CreateOrEditRecordRequirementDto RecordRequirement { get; set; } = new CreateOrEditRecordRequirementDto();

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

    }
}