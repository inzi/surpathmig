using System.Threading.Tasks;
using inzibackend.Authorization.Users;

namespace inzibackend.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
