using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetLedgerEntryForViewDto
    {
        public LedgerEntryDto LedgerEntry { get; set; }

        public string UserName { get; set; }

        public string TenantDocumentName { get; set; }

        public string CohortName { get; set; }

    }
}