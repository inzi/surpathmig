using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace inzibackend.Surpath.Dtos
{
    public class CreateEditRecordRequirementDto : EntityDto<Guid?>
    {

        public CreateOrEditRecordRequirementDto CreateOrEditRecordRequirement { get; set; }
        public List<CreateOrEditRecordCategoryDto> CreateOrEditRecordCategories { get; set; } = new List<CreateOrEditRecordCategoryDto>();


    }
}