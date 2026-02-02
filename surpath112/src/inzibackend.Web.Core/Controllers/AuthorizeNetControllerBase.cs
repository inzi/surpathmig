using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.MultiTenancy.Payments.AuthorizeNet;
using inzibackend.MultiTenancy.Payments.Stripe.Dto;
using Stripe;
using inzibackend.MultiTenancy.Payments.AuthorizeNet.Dto;

namespace inzibackend.Web.Controllers
{
    public class AuthorizeNetControllerBase : inzibackendControllerBase
    {
        protected readonly IAuthorizeNetPaymentAppService AuthorizeNetPaymentAppService;
        private readonly AuthorizeNetManager _authorizeNetManager;
        private readonly AuthorizeNetConfiguration _authorizeNetConfiguration;

        public AuthorizeNetControllerBase(
            AuthorizeNetManager _Manager,
            AuthorizeNetConfiguration _configuration,
            IAuthorizeNetPaymentAppService _paymentAppService)
        {
            AuthorizeNetPaymentAppService = _paymentAppService;
            _authorizeNetManager = _Manager;
            _authorizeNetConfiguration = _configuration;
        }

        [HttpPost]
        public async Task<IActionResult> WebHooks()
        {
            string json;
            using (var streamReader = new StreamReader(HttpContext.Request.Body))
            {
                json = await streamReader.ReadToEndAsync();
            }

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _authorizeNetConfiguration.WebhookSecret);

                if (stripeEvent.Type == Events.InvoicePaid)
                {
                    await HandleSubscriptionCyclePaymentAsync(stripeEvent);
                }

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    await HandleCheckoutSessionCompletedAsync(stripeEvent);
                }

                // Other WebHook events can be handled here.

                return Ok();
            }
            catch (ApplicationException exception)
            {
                Logger.Error(exception.Message, exception);
                return BadRequest();
            }
            catch (StripeException exception)
            {
                Logger.Error(exception.Message, exception);
                return BadRequest();
            }
        }

        private async Task HandleSubscriptionCyclePaymentAsync(Event stripeEvent)
        {
            var invoice = stripeEvent.Data.Object as Invoice;
            if (invoice == null)
            {
                throw new ApplicationException("Unable to get Invoice information from stripeEvent.Data");
            }

            // see https://stripe.com/docs/api/invoices/object#invoice_object-billing_reason
            // only handle for subscription_cycle payments
            if (invoice.BillingReason == "subscription_cycle")
            {
                await _authorizeNetManager.HandleInvoicePaymentSucceededAsync();
            }
        }

        private async Task HandleCheckoutSessionCompletedAsync(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session == null)
            {
                throw new ApplicationException("Unable to get session information from stripeEvent.Data");
            }

            await AuthorizeNetPaymentAppService.ConfirmPayment(
                new AuthorizeNetPaymentInput
                {
                    SessionId = session.Id
                });
        }
    }
}
