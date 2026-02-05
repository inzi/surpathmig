using inzibackend.Surpath;
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
    [Table("LedgerEntryDetails")]
    [Audited]
    public class LedgerEntryDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Note { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinAmountValue, LedgerEntryDetailConsts.MaxAmountValue)]
        public virtual decimal Amount { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinDiscountValue, LedgerEntryDetailConsts.MaxDiscountValue)]
        public virtual decimal Discount { get; set; }

        [Range(typeof(decimal), LedgerEntryDetailConsts.MinDiscountAmountValue, LedgerEntryDetailConsts.MaxDiscountAmountValue)]
        public virtual decimal DiscountAmount { get; set; }

        public virtual string MetaData { get; set; }

        public virtual decimal AmountPaid { get; set; }

        public virtual DateTime? DatePaidOn { get; set; }

        public virtual Guid? LedgerEntryId { get; set; }

        [ForeignKey("LedgerEntryId")]
        public LedgerEntry LedgerEntryFk { get; set; }

        public virtual Guid SurpathServiceId { get; set; }

        [ForeignKey("SurpathServiceId")]
        public SurpathService SurpathServiceFk { get; set; }

        public virtual Guid? TenantSurpathServiceId { get; set; }

        [ForeignKey("TenantSurpathServiceId")]
        public TenantSurpathService TenantSurpathServiceFk { get; set; }
        
        // New reference to UserPurchase
        public virtual Guid? UserPurchaseId { get; set; }
        
        [ForeignKey("UserPurchaseId")]
        public UserPurchase UserPurchaseFk { get; set; }
    }
}
