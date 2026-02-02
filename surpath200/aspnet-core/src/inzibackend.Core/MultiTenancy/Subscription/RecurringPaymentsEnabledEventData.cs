using Abp.Events.Bus;

namespace inzibackend.MultiTenancy.Subscription;

public class RecurringPaymentsEnabledEventData : EventData
{
    public int TenantId { get; set; }
}

