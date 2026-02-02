using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Controllers;

namespace inzibackend.Web.Public.Controllers
{
    public class AboutController : inzibackendControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}