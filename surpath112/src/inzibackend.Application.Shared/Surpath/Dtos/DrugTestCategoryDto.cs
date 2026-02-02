using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class DrugTestCategoryDto : EntityDto<Guid>
    {

        public Guid DrugId { get; set; }

        public Guid PanelId { get; set; }

        public Guid TestCategoryId { get; set; }

    }
}