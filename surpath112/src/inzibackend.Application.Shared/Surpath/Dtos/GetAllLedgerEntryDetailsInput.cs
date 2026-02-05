using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllLedgerEntryDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NoteFilter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public decimal? MaxDiscountFilter { get; set; }
        public decimal? MinDiscountFilter { get; set; }

        public decimal? MaxDiscountAmountFilter { get; set; }
        public decimal? MinDiscountAmountFilter { get; set; }

        public string MetaDataFilter { get; set; }

        public decimal? MaxAmountPaidFilter { get; set; }
        public decimal? MinAmountPaidFilter { get; set; }

        public DateTime? MaxDatePaidOnFilter { get; set; }
        public DateTime? MinDatePaidOnFilter { get; set; }

        public string LedgerEntryTransactionIdFilter { get; set; }

        public string SurpathServiceNameFilter { get; set; }

        public string TenantSurpathServiceNameFilter { get; set; }

        public Guid? LedgerEntryIdFilter { get; set; }
    }
}
