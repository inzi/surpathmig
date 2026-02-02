using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.MultiTenancy.Payments.AuthorizeNet
{
    public class AuthorizeNetConfiguration : IPaymentGatewayConfiguration
    {
        private readonly IConfigurationRoot _appConfiguration;

        bool IPaymentGatewayConfiguration.IsActive => throw new NotImplementedException();

        bool IPaymentGatewayConfiguration.SupportsRecurringPayments => throw new NotImplementedException();

        SubscriptionPaymentGatewayType IPaymentGatewayConfiguration.GatewayType => throw new NotImplementedException();

        public string WebhookSecret => _appConfiguration["Payment:AuthorizeNet:WebhookSecret"];

    }
}
