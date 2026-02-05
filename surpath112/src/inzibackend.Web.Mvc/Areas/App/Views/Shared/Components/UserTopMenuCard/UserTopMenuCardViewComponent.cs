using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Session;
using inzibackend.Web.Views;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppLogo
{
    public class UserTopMenuCardViewComponent : inzibackendViewComponent
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IAbpSession _abpSession;
        private readonly IPerRequestSessionCache _sessionCache;

        public UserTopMenuCardViewComponent(
            IPerRequestSessionCache sessionCache,
            IMultiTenancyConfig multiTenancyConfig,
            IAbpSession abpSession)
        {
            _sessionCache = sessionCache;
            _multiTenancyConfig = multiTenancyConfig;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new UserTopMenuCardViewModel
            {
            };

            return View(headerModel);
        }
    }
}
