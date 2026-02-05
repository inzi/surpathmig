using Abp.Events.Bus;

namespace inzibackend.MultiTenancy.Subscription;

public class SubscriptionCancelledEventData : EventData
{
    public long PaymentId { get; set; }

    public string ExternalPaymentId { get; set; }
}

