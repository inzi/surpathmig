using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class PanelDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public Guid? TestCategoryId { get; set; }

    }
}