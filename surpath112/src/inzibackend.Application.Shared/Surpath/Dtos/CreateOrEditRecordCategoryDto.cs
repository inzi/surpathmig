using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditRecordCategoryDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public string Instructions { get; set; }

        public Guid? RecordRequirementId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }

    }
}