using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetLedgerEntryDetailForViewDto
    {
        public LedgerEntryDetailDto LedgerEntryDetail { get; set; }

        public string LedgerEntryTransactionId { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

    }
}