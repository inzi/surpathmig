using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class TenantDepartmentUserDto : EntityDto<Guid>
    {

        public long UserId { get; set; }

        public Guid TenantDepartmentId { get; set; }

    }
}