using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Session;
using inzibackend.Web.Views;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppLogo
{
    public class AppLogoViewComponent : inzibackendViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppLogoViewComponent(
            IPerRequestSessionCache sessionCache
        )
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string logoSkin = null, string logoClass = "")
        {
            var headerModel = new LogoViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                LogoSkinOverride = logoSkin,
                LogoClassOverride = logoClass
            };

            return View(headerModel);
        }
    }
}
