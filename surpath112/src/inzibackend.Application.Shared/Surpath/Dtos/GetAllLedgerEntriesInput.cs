using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllLedgerEntriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? ServiceTypeFilter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public decimal? MaxDiscountAmountFilter { get; set; }
        public decimal? MinDiscountAmountFilter { get; set; }

        public decimal? MaxTotalPriceFilter { get; set; }
        public decimal? MinTotalPriceFilter { get; set; }

        public int? PaymentPeriodTypeFilter { get; set; }

        public DateTime? MaxExpirationDateFilter { get; set; }
        public DateTime? MinExpirationDateFilter { get; set; }

        public string TransactionNameFilter { get; set; }

        public string TransactionKeyFilter { get; set; }

        public string TransactionIdFilter { get; set; }

        public int? SettledFilter { get; set; }

        public decimal? MaxAmountDueFilter { get; set; }
        public decimal? MinAmountDueFilter { get; set; }

        public string PaymentTokenFilter { get; set; }

        public string AuthNetCustomerProfileIdFilter { get; set; }

        public string AuthNetCustomerPaymentProfileIdFilter { get; set; }

        public string AuthNetCustomerAddressIdFilter { get; set; }

        public string AccountNumberFilter { get; set; }

        public string NoteFilter { get; set; }

        public string MetaDataFilter { get; set; }

        public string AuthCodeFilter { get; set; }

        public string ReferenceTransactionIdFilter { get; set; }

        public string TransactionHashFilter { get; set; }

        public string AccountTypeFilter { get; set; }

        public string TransactionCodeFilter { get; set; }

        public string TransactionMessageFilter { get; set; }

        public string AuthNetTransHashSha2Filter { get; set; }

        public string AuthNetNetworkTransIdFilter { get; set; }

        public decimal? MaxPaidAmountFilter { get; set; }
        public decimal? MinPaidAmountFilter { get; set; }

        public decimal? MaxPaidInCashFilter { get; set; }
        public decimal? MinPaidInCashFilter { get; set; }

        public decimal? MaxAvailableUserBalanceFilter { get; set; }
        public decimal? MinAvailableUserBalanceFilter { get; set; }

        public string CardNameOnCardFilter { get; set; }

        public string CardZipCodeFilter { get; set; }

        public decimal? MaxBalanceForwardFilter { get; set; }
        public decimal? MinBalanceForwardFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string TenantDocumentNameFilter { get; set; }

        public string CohortNameFilter { get; set; }

        public long? UserIdFilter { get; set; }


    }
}
