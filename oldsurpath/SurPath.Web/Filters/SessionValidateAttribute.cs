using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurPathWeb.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionValidateAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Url.ToString();

            if (HttpContext.Current.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                            new System.Web.Routing.RouteValueDictionary {
                                                { "action", "Login" },
                                                { "controller", "Authentication" },
                                                { "returnUrl", url }
                                            }
                                        );
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //
        }
    }
}