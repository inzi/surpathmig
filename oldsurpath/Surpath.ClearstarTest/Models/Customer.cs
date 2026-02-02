using System;

namespace Surpath.CSTest.Models
{
    [Serializable]
    public class CustomerModel
    {
        public string CustomerId { get; set; }
        public string ParentId { get; set; }
        public int PaymentTerms { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool isCentInvoice { get; set; }
        public bool isCentContract { get; set; }
        public string Comments { get; set; }
        public bool isSurcharge { get; set; }
        public bool isReqReview { get; set; }
        public string Email { get; set; }
        public int DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryFrom { get; set; }
        public string TimeZone { get; set; }
        public string DateFormat { get; set; }
        public double TaxRate { get; set; }
        public string InvGroup { get; set; }
        public bool isOnlyCompOrder { get; set; }
        public int DistId { get; set; }
        public bool isReviewFlaggedOnly { get; set; }
        public bool isAutoSummarize { get; set; }
        public bool isAutoPass { get; set; }
        public bool isCrossCustomerActive { get; set; }
        public bool isPassFailDashboardActive { get; set; }
        public bool isBatchImportActive { get; set; }
        public bool isResultsTagActive { get; set; }
        public bool isProfileExpirationActive { get; set; }
        public bool isInvoiceCompProfiles { get; set; }
    }
}