using inzibackend.MultiTenancy.Payments;
using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class UserPurchaseDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public EnumPurchaseStatus Status { get; set; }
        
        public double OriginalPrice { get; set; }
        
        public double AdjustedPrice { get; set; }
        
        public double DiscountAmount { get; set; }
        
        public double AmountPaid { get; set; }
        
        public PaymentPeriodType PaymentPeriodType { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }
        
        public bool IsRecurring { get; set; }
        
        public string Notes { get; set; }
        
        public string MetaData { get; set; }
        
        public double BalanceDue { get; set; }
        
        public long? UserId { get; set; }
        
        public Guid? SurpathServiceId { get; set; }
        
        public Guid? TenantSurpathServiceId { get; set; }
        
        public Guid? CohortId { get; set; }
    }
}