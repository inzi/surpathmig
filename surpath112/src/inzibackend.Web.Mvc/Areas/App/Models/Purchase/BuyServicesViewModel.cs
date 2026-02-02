using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.Purchase
{
    public class BuyServicesViewModel
    {
        //public SubscriptionStartType? SubscriptionStartType { get; set; }

        //public EditionSelectDto Edition { get; set; }

        //public decimal? AdditionalPrice { get; set; }

        //public EditionPaymentType EditionPaymentType { get; set; }

        //public List<PaymentGatewayModel> PaymentGateways { get; set; }
        public decimal AmountDue { get; set; }
       // public List<SurpathServiceDto> SurpathServices { get; set; } = new List<SurpathServiceDto>();
        public List<TenantSurpathServiceDto> TenantSurpathServices { get; set; } = new List<TenantSurpathServiceDto>();
        public bool UseSandboxPayment { get; set; } = true;
    }
}
