using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Controllers;

namespace inzibackend.Web.Public.Controllers;

public class HomeController : inzibackendControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}

