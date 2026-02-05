using System;

namespace inzibackend.Web.Areas.App.Models.UserPurchases
{
    public class UserPurchasesViewModel
    {
        public string FilterText { get; set; }
        public long? UserIdFilter { get; set; }
    }
}