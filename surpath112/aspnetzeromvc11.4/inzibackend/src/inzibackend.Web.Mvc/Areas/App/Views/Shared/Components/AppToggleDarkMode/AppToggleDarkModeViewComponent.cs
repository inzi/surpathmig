using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Layout;
using inzibackend.Web.Views;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppToggleDarkMode
{
    public class AppToggleDarkModeViewComponent : inzibackendViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}