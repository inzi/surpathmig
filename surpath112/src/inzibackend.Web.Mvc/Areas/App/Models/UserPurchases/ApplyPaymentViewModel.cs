using System;

namespace inzibackend.Web.Areas.App.Models.UserPurchases
{
    public class ApplyPaymentViewModel
    {
        public Guid UserPurchaseId { get; set; }
        public string UserPurchaseName { get; set; }
        public double BalanceDue { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
    }
}