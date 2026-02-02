using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Threading;
using inzibackend.Editions;
//using Stripe;
//using Stripe.Checkout;



namespace inzibackend.MultiTenancy.Payments.AuthorizeNet
{
    public class AuthorizeNetManager : inzibackendServiceBase,
        ISupportsRecurringPayments,
        ITransientDependency
    {
        void IEventHandler<RecurringPaymentsDisabledEventData>.HandleEvent(RecurringPaymentsDisabledEventData eventData)
        {
            //throw new NotImplementedException();
        }

        void IEventHandler<RecurringPaymentsEnabledEventData>.HandleEvent(RecurringPaymentsEnabledEventData eventData)
        {
           // throw new NotImplementedException();
        }

        void IEventHandler<TenantEditionChangedEventData>.HandleEvent(TenantEditionChangedEventData eventData)
        {
           // throw new NotImplementedException();
        }

        public async Task HandleInvoicePaymentSucceededAsync()
        {
            //var customerService = new CustomerService();
            //var customer = await customerService.GetAsync(invoice.CustomerId);

            //int tenantId;
            //int editionId;

            //PaymentPeriodType? paymentPeriodType;

            //using (CurrentUnitOfWork.SetTenantId(null))
            //{
            //    var tenant = await _tenantManager.FindByTenancyNameAsync(customer.Description);
            //    tenantId = tenant.Id;
            //    editionId = tenant.EditionId.Value;

            //    using (CurrentUnitOfWork.SetTenantId(tenantId))
            //    {
            //        var lastPayment = GetLastCompletedSubscriptionPayment(tenantId);
            //        paymentPeriodType = lastPayment.GetPaymentPeriodType();
            //    }

            //    await _tenantManager.UpdateTenantAsync(
            //        tenant.Id,
            //        isActive: true,
            //        isInTrialPeriod: false,
            //        paymentPeriodType,
            //        tenant.EditionId.Value,
            //        EditionPaymentType.Extend);
            //}

            //var payment = new SubscriptionPayment
            //{
            //    TenantId = tenantId,
            //    Amount = ConvertFromStripePrice(invoice.AmountPaid),
            //    DayCount = (int)paymentPeriodType,
            //    PaymentPeriodType = paymentPeriodType,
            //    EditionId = editionId,
            //    ExternalPaymentId = invoice.ChargeId,
            //    Gateway = SubscriptionPaymentGatewayType.Stripe,
            //    IsRecurring = true
            //};

            //payment.SetAsPaid();

            //await _subscriptionPaymentRepository.InsertAsync(payment);
        }

    }
}
