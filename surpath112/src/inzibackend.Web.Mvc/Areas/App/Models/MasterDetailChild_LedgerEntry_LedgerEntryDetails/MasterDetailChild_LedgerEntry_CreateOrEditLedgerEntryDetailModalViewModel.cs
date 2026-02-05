using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.LedgerEntryDetails
{
    public class MasterDetailChild_LedgerEntry_CreateOrEditLedgerEntryDetailModalViewModel
    {
        public CreateOrEditLedgerEntryDetailDto LedgerEntryDetail { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantSurpathServiceName { get; set; }

        public bool IsEditMode => LedgerEntryDetail.Id.HasValue;
    }
}