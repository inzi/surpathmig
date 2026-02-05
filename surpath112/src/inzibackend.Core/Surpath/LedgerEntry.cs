using inzibackend.MultiTenancy.Payments;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("LedgerEntries")]
    [Audited]
    public class LedgerEntry : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual EnumServiceType ServiceType { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal TotalPrice { get; set; }

        public virtual PaymentPeriodType PaymentPeriodType { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string TransactionName { get; set; }

        public virtual string TransactionKey { get; set; }

        public virtual string TransactionId { get; set; }

        public virtual bool Settled { get; set; }

        public virtual decimal AmountDue { get; set; }

        public virtual string PaymentToken { get; set; }

        public virtual string AuthNetCustomerProfileId { get; set; }

        public virtual string AuthNetCustomerPaymentProfileId { get; set; }

        public virtual string AuthNetCustomerAddressId { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual string Note { get; set; }

        public virtual string MetaData { get; set; }

        public virtual string AuthCode { get; set; }

        public virtual string ReferenceTransactionId { get; set; }

        public virtual string TransactionHash { get; set; }

        public virtual string AccountType { get; set; }

        public virtual string TransactionCode { get; set; }

        public virtual string TransactionMessage { get; set; }

        public virtual string AuthNetTransHashSha2 { get; set; }

        public virtual string AuthNetNetworkTransId { get; set; }

        public virtual decimal PaidAmount { get; set; }

        public virtual decimal PaidInCash { get; set; }

        public virtual decimal AvailableUserBalance { get; set; }

        [Required]
        public virtual string CardNameOnCard { get; set; }

        [Required]
        public virtual string CardZipCode { get; set; }

        public virtual decimal BalanceForward { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual Guid? TenantDocumentId { get; set; }

        [ForeignKey("TenantDocumentId")]
        public TenantDocument TenantDocumentFk { get; set; }

        public virtual Guid? CohortId { get; set; }

        [ForeignKey("CohortId")]
        public Cohort CohortFk { get; set; }
        
        public string CardLastFour { get; set; } = string.Empty;
        
        // New properties for user purchase tracking
        public virtual Guid? UserPurchaseId { get; set; }
        
        [ForeignKey("UserPurchaseId")]
        public UserPurchase UserPurchaseFk { get; set; }
        
        public virtual bool IsRefund { get; set; }
        
        public virtual DateTime? PaymentDate { get; set; }
        
        public virtual string PaymentMethod { get; set; }
        
        // Helper property to calculate balance
        [NotMapped]
        public virtual decimal CurrentBalance => TotalPrice - PaidAmount - BalanceForward;
    }
}
