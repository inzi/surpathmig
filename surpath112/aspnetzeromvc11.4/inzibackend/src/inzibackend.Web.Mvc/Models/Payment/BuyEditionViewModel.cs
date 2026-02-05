using System.Collections.Generic;
using inzibackend.Editions;
using inzibackend.Editions.Dto;
using inzibackend.MultiTenancy.Payments;
using inzibackend.MultiTenancy.Payments.Dto;

namespace inzibackend.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
