using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTenantDepartmentUserDto : EntityDto<Guid?>
    {

        public long UserId { get; set; }

        public Guid TenantDepartmentId { get; set; }

    }
}