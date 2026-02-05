using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurPathWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.MapHttpAttributeRoutes();

            ////config.Routes.MapHttpRoute(
            ////    name: "FormFoxupdateOrderResult",
            ////    routeTemplate: "api/formfox/{action}",
            ////    defaults: new {controller = "FormFox"}
            ////    );

            ////config.Routes.MapHttpRoute(
            ////name: "FormFoxupdateOrderResult",
            ////routeTemplate: "api/{controller}/{action}",
            ////defaults: new { controller = "FormFox" }

            ////);

            ////config.Routes.MapHttpRoute(
            ////        name: "FormFox",
            ////        routeTemplate: "api/{controller}/",
            ////        defaults: new { controller = "FormFox" }

            ////        );

            //RouteTable.Routes.MapRoute(
            //    "DefaultApi",
            //    "api/{controller}/{id}",
            //    new { action = "DefaultAction", id = System.Web.Http.RouteParameter.Optional }
            //);

        }
    }
}
