using inzibackend.Surpath;
using inzibackend.MultiTenancy.Payments;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditLedgerEntryDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public EnumServiceType ServiceType { get; set; }

        public decimal Amount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalPrice { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string TransactionName { get; set; }

        public string TransactionKey { get; set; }

        public string TransactionId { get; set; }

        public bool Settled { get; set; }

        public decimal AmountDue { get; set; }

        public string PaymentToken { get; set; }

        public string AuthNetCustomerProfileId { get; set; }

        public string AuthNetCustomerPaymentProfileId { get; set; }

        public string AuthNetCustomerAddressId { get; set; }

        public string AccountNumber { get; set; }

        public string Note { get; set; }

        public string MetaData { get; set; }

        public string AuthCode { get; set; }

        public string ReferenceTransactionId { get; set; }

        public string TransactionHash { get; set; }

        public string AccountType { get; set; }

        public string TransactionCode { get; set; }

        public string TransactionMessage { get; set; }

        public string AuthNetTransHashSha2 { get; set; }

        public string AuthNetNetworkTransId { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal PaidInCash { get; set; }

        public decimal AvailableUserBalance { get; set; }

        [Required]
        public string CardNameOnCard { get; set; }

        [Required]
        public string CardZipCode { get; set; }

        public decimal BalanceForward { get; set; }

        public long? UserId { get; set; }

        public Guid? TenantDocumentId { get; set; }

        public Guid? CohortId { get; set; }

    }
}
