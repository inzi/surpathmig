using inzibackend.Authorization.Users;
using inzibackend.MultiTenancy.Payments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("UserPurchases")]
    [Audited]
    public class UserPurchase : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual EnumPurchaseStatus Status { get; set; }

        [Range(UserPurchaseConsts.MinPriceValue, UserPurchaseConsts.MaxPriceValue)]
        public virtual double OriginalPrice { get; set; }

        [Range(UserPurchaseConsts.MinPriceValue, UserPurchaseConsts.MaxPriceValue)]
        public virtual double AdjustedPrice { get; set; }

        [Range(UserPurchaseConsts.MinDiscountAmountValue, UserPurchaseConsts.MaxDiscountAmountValue)]
        public virtual double DiscountAmount { get; set; }

        [Range(UserPurchaseConsts.MinAmountPaidValue, UserPurchaseConsts.MaxAmountPaidValue)]
        public virtual double AmountPaid { get; set; }

        public virtual PaymentPeriodType PaymentPeriodType { get; set; }

        public virtual DateTime PurchaseDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual bool IsRecurring { get; set; }

        public virtual string Notes { get; set; }

        public virtual string MetaData { get; set; }

        // Calculated property - not stored in database
        [NotMapped]
        public virtual double BalanceDue => AdjustedPrice - AmountPaid;

        // Foreign keys
        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual Guid? SurpathServiceId { get; set; }

        [ForeignKey("SurpathServiceId")]
        public SurpathService? SurpathServiceFk { get; set; }

        public virtual Guid? TenantSurpathServiceId { get; set; }

        [ForeignKey("TenantSurpathServiceId")]
        public TenantSurpathService? TenantSurpathServiceFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort? CohortFk { get; set; }
    }
}