using inzibackend.MultiTenancy.Payments;
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditUserPurchaseDto : EntityDto<Guid?>
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public EnumPurchaseStatus Status { get; set; }
        
        [Range(0, 1000000)]
        public double OriginalPrice { get; set; }
        
        [Range(0, 1000000)]
        public double AdjustedPrice { get; set; }
        
        [Range(0, 1000000)]
        public double DiscountAmount { get; set; }
        
        [Range(0, 1000000)]
        public double AmountPaid { get; set; }
        
        public PaymentPeriodType PaymentPeriodType { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }
        
        public bool IsRecurring { get; set; }
        
        public string Notes { get; set; }
        
        public string MetaData { get; set; }
        
        public long? UserId { get; set; }
        
        public Guid? SurpathServiceId { get; set; }
        
        public Guid? TenantSurpathServiceId { get; set; }
        
        public Guid? CohortId { get; set; }
    }
}