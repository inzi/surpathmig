using Abp.Application.Services.Dto;
using Abp.Webhooks;
using inzibackend.WebHooks.Dto;

namespace inzibackend.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
