using Abp.AspNetCore.Mvc.ViewComponents;
using inzibackend.Web.Views;
using Microsoft.AspNetCore.Mvc;

namespace inzibackend.Web.Areas.App.Views.Shared.Components.AppLegalDocumentFooter
{
    public class AppLegalDocumentFooterViewComponent : inzibackendViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}