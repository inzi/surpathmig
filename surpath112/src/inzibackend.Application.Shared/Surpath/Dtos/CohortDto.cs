using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class CohortDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool DefaultCohort { get; set; }

        public Guid? TenantDepartmentId { get; set; }

    }
}