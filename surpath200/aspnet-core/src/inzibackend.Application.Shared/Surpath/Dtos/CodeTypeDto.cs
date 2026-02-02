using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class CodeTypeDto : EntityDto<Guid>
    {
        public string Name { get; set; }

    }
}