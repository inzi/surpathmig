using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.Payments.Dto;

namespace inzibackend.MultiTenancy;

public interface ISubscriptionAppService : IApplicationService
{
    Task DisableRecurringPayments();

    Task EnableRecurringPayments();

    Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);

    Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);

    Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
}

