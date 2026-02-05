using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class ToggleEnabledDto : EntityDto<Guid>
    {
        public bool IsEnabled { get; set; }
    }
}