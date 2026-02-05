using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditCohortDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public bool DefaultCohort { get; set; }

        public Guid? TenantDepartmentId { get; set; }

    }
}