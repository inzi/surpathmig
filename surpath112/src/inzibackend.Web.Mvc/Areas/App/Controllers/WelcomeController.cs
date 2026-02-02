using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Controllers;
using inzibackend.Surpath;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : inzibackendControllerBase
    {
        private readonly IWelcomemessagesAppService _welcomemessagesAppService;
        public WelcomeController(IWelcomemessagesAppService welcomemessagesAppService)
        {
            _welcomemessagesAppService = welcomemessagesAppService;
        }
        public ActionResult Index()
        {
            var _w = _welcomemessagesAppService.GetCurrent().GetAwaiter().GetResult();
            ViewData["CurrentWelcome"] = _w;
            return View();
        }
    }
}