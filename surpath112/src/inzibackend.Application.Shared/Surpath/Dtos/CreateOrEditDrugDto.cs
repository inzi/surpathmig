using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditDrugDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(DrugConsts.MaxNameLength, MinimumLength = DrugConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DrugConsts.MaxCodeLength, MinimumLength = DrugConsts.MinCodeLength)]
        public string Code { get; set; }

    }
}