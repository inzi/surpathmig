using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTenantSurpathServiceDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        [Range(TenantSurpathServiceConsts.MinPriceValue, TenantSurpathServiceConsts.MaxPriceValue)]
        public double Price { get; set; }

        public string Description { get; set; }

        public bool IsPricingOverrideEnabled { get; set; }

        public Guid? SurpathServiceId { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public long? UserId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }
        
        public int? TenantId { get; set; }
        
        public bool IsInvoiced { get; set; }

    }
}