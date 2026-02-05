using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Session;
using inzibackend.Web.Views;

namespace inzibackend.Web.Areas.App.Views.Shared.Themes.Theme5.Components.AppTheme5Brand
{
    public class AppTheme5BrandViewComponent : inzibackendViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme5BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
