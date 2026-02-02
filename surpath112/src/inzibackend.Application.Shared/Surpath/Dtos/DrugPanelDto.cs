using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class DrugPanelDto : EntityDto<Guid>
    {

        public Guid DrugId { get; set; }

        public Guid PanelId { get; set; }

    }
}