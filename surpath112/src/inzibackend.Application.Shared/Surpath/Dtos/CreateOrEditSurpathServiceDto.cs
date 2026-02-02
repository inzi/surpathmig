using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditSurpathServiceDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        [Range(SurpathServiceConsts.MinPriceValue, SurpathServiceConsts.MaxPriceValue)]
        public double Price { get; set; }

        [Range(SurpathServiceConsts.MinDiscountValue, SurpathServiceConsts.MaxDiscountValue)]
        public decimal Discount { get; set; }

        public string Description { get; set; }

        public bool IsEnabledByDefault { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public long? UserId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }

        public string FeatureIdentifier { get; set; }

    }
}