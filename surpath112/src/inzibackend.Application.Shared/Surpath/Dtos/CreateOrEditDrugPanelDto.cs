using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditDrugPanelDto : EntityDto<Guid?>
    {

        public Guid DrugId { get; set; }

        public Guid PanelId { get; set; }

    }
}