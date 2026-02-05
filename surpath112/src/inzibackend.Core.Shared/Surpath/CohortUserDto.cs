using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class CohortUserDto : EntityDto<Guid>
    {

        public Guid? CohortId { get; set; }

        public long UserId { get; set; }
        public int? TenantId { get; set; }
    }
}