using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditCodeTypeDto : EntityDto<Guid?>
    {

        [Required]
        [StringLength(CodeTypeConsts.MaxNameLength, MinimumLength = CodeTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}