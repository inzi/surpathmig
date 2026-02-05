using inzibackend.Surpath;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTenantDepartmentDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public bool Active { get; set; }

        public EnumClientMROTypes MROType { get; set; }

        public string Description { get; set; }

    }
}