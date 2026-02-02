using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllUserPurchasesForExcelInput
    {
        public string Filter { get; set; }
        
        public string NameFilter { get; set; }
        
        public string DescriptionFilter { get; set; }
        
        public int? StatusFilter { get; set; }
        
        public double? MinOriginalPriceFilter { get; set; }
        public double? MaxOriginalPriceFilter { get; set; }
        
        public double? MinAdjustedPriceFilter { get; set; }
        public double? MaxAdjustedPriceFilter { get; set; }
        
        public double? MinDiscountAmountFilter { get; set; }
        public double? MaxDiscountAmountFilter { get; set; }
        
        public double? MinAmountPaidFilter { get; set; }
        public double? MaxAmountPaidFilter { get; set; }
        
        public int? PaymentPeriodTypeFilter { get; set; }
        
        public DateTime? MinPurchaseDateFilter { get; set; }
        public DateTime? MaxPurchaseDateFilter { get; set; }
        
        public DateTime? MinExpirationDateFilter { get; set; }
        public DateTime? MaxExpirationDateFilter { get; set; }
        
        public int? IsRecurringFilter { get; set; }
        
        public string NotesFilter { get; set; }
        
        public string UserNameFilter { get; set; }
        public long? UserIdFilter { get; set; }
        
        public string SurpathServiceNameFilter { get; set; }
        
        public string TenantSurpathServiceNameFilter { get; set; }
        
        public string CohortNameFilter { get; set; }
    }
}