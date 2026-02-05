using System;

namespace inzibackend.Web.Areas.App.Models.UserPurchases
{
    public class AdjustPriceViewModel
    {
        public Guid UserPurchaseId { get; set; }
        public string UserPurchaseName { get; set; }
        public double OriginalPrice { get; set; }
        public double CurrentAdjustedPrice { get; set; }
        public double NewAdjustedPrice { get; set; }
        public string Reason { get; set; }
    }
}