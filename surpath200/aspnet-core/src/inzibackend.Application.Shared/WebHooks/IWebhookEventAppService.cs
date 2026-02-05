using System.Threading.Tasks;
using Abp.Webhooks;

namespace inzibackend.WebHooks;

public interface IWebhookEventAppService
{
    Task<WebhookEvent> Get(string id);
}

