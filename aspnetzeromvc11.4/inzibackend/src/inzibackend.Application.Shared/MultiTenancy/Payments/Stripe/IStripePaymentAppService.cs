using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.MultiTenancy.Payments.Dto;
using inzibackend.MultiTenancy.Payments.Stripe.Dto;

namespace inzibackend.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}