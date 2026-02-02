using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetCohortForEditOutput
    {
        public CreateOrEditCohortDto Cohort { get; set; }

        public string TenantDepartmentName { get; set; }

    }
}