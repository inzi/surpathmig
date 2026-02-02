using System.Collections.Generic;
using inzibackend.Editions.Dto;
using inzibackend.MultiTenancy.Payments;

namespace inzibackend.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}