using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath
{
    public class SlotRotationDayDto : EntityDto<int?>
    {
        public DayOfWeek Day { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
