using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class TenantDepartmentDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public EnumClientMROTypes MROType { get; set; }

        public string Description { get; set; }

    }
}