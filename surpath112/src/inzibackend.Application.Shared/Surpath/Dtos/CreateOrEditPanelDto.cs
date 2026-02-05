using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditPanelDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(PanelConsts.MaxNameLength, MinimumLength = PanelConsts.MinNameLength)]
        public string Name { get; set; }

        [Range(PanelConsts.MinCostValue, PanelConsts.MaxCostValue)]
        public double Cost { get; set; }

        [StringLength(PanelConsts.MaxDescriptionLength, MinimumLength = PanelConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public Guid? TestCategoryId { get; set; }

    }
}