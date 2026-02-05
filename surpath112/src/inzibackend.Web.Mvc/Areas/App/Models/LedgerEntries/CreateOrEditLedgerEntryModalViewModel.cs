using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.LedgerEntries
{
    public class CreateOrEditLedgerEntryModalViewModel
    {
        public CreateOrEditLedgerEntryDto LedgerEntry { get; set; }

        public string UserName { get; set; }

        public string TenantDocumentName { get; set; }

        public string CohortName { get; set; }

        public bool IsEditMode => LedgerEntry.Id.HasValue;
    }
}