using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Session;
using inzibackend.Web.Views;

namespace inzibackend.Web.Areas.App.Views.Shared.Themes.Theme4.Components.AppTheme4Footer
{
    public class AppTheme4FooterViewComponent : inzibackendViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme4FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
