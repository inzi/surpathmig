using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class DeptCodeDto : EntityDto<Guid>
    {
        public string Code { get; set; }

        public Guid CodeTypeId { get; set; }

        public Guid TenantDepartmentId { get; set; }

    }
}