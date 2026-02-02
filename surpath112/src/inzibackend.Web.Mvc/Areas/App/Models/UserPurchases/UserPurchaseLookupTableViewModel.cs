namespace inzibackend.Web.Areas.App.Models.UserPurchases
{
    public class UserPurchaseUserLookupTableViewModel
    {
        public long? Id { get; set; }
        public string DisplayName { get; set; }
        public string FilterText { get; set; }
    }

    public class UserPurchaseSurpathServiceLookupTableViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FilterText { get; set; }
    }

    public class UserPurchaseTenantSurpathServiceLookupTableViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FilterText { get; set; }
    }

    public class UserPurchaseCohortLookupTableViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FilterText { get; set; }
    }
}