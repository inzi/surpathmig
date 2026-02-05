using System.Threading.Tasks;
using Abp.Application.Services;

namespace inzibackend.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
