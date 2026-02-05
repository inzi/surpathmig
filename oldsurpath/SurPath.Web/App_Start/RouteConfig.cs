using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Protocols;

namespace SurPathWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            //routes.EnableFriendlyUrls();

            //var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            //routes.EnableFriendlyUrls(settings);

            // This is a possible solution - but can't make it work
            // https://stackoverflow.com/questions/48578423/asmx-web-services-routing-in-asp-net-web-forms


            //var r =  new WebServiceHandlerFactory().GetHandler(HttpContext.Current, "*", "/api/formfox/WebService.asmx", HttpContext.Current.Server.MapPath(aspxToLoad))
            //var r = new WebServiceHandlerFactory().GetHandler(HttpContext.Current, "*", "/api/formfox/updateOrderStatus", HttpContext.Current.Server.MapPath("/api/formfox/FormFox.asmx"));
            //var r = new WebServiceHandlerFactory().GetHandler(HttpContext.Current, "*", "/api/formfox/updateOrderResult", HttpContext.Current.Server.MapPath("/api/formfox/FormFox.asmx"));

            //routes.MapPageRoute("updateOrderResultRoute", "~/api/formfox/updateOrderResult", "/api/formfox/FormFox.asmx");
            // routes.MapPageRoute("updateOrderStatusRoute", "~/api/formfox/updateOrderStatus", "/api/formfox/FormFox.asmx");
            //routes.MapRoute("UOSR", "api/formfox/updateOrderStatus",)
            //routes.MapPageRoute("FormFoxASMXRoute", "api/formfox/", "~/api/formfox/formfox.asmx");

            //  routes.Add("RouteName", new Route("api/formfox/", new RouteValueDictionary() { { "controller", null }, { "action", null } }, new ServiceRouteHandler("~/api/formfox/formfox.asmx")));


            //routes.Add("RouteName", new Route("api/formfox", new RouteValueDictionary() { { "controller", null } }, new ServiceRouteHandler("~/api/formfox.asmx")));
            //routes.Add("RouteName", new Route("api/formfox", new RouteValueDictionary() { { "controller", null }, { "action", null } }, new ServiceRouteHandler("~/api/formfox.asmx")));
            //routes.Add("RouteName2", new Route("api/formfox/updateOrderResult", new RouteValueDictionary() { { "controller", null }, { "action", "updateOrderResult" } }, new ServiceRouteHandler("~/api/formfox.asmx")));


            //routes.MapServiceRoute("updateOrderResultRoute", "api/formfox/updateOrderResult", "~/api/formfox.asmx?op=updateOrderResult");
            // routes.MapServiceRoute("updateOrderResultRoute", "path/to/your/service", "~/actualservice.asmx");


            // routes.Add("updateOrderResultRoute", new Route("api/formfox/updateOrderResult", new RouteValueDictionary() { { "controller", null }, { "action", null } }, new ServiceRouteHandler("~/api/formfox.asmx?op=updateOrderResult")));
            // routes.MapServiceRoute("updateOrderResultRoute", "api/formfox/updateOrderResult", "~/api/formfox.asmx?op=updateOrderResult");

            //// from https://stackoverflow.com/questions/4675367/ignoreroute-with-webservice-exclude-asmx-urls-from-routing
            routes.IgnoreRoute("{*url}", new { url = @".*\.asmx(/.*)?" });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("api/{*pathInfo}");

            //routes.MapMvcAttributeRoutes(); //Attribute routing
            routes.MapMvcAttributeRoutes(); //Attribute routing

            //routes.MapRoute(
            //    name: "Api",
            //    url: "api/{controller}/{action}/{id}",
            //    defaults: new { controller = "Integration", action = "Echo" }
            //);
            //routes.MapRoute(
            //    name: "handoff",
            //    url: "{controller}/{action}/{donorid}/{clientid}",
            //    defaults: new { controller = "Registration", action = "handoff", id = UrlParameter.Optional, dto = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Doc",
                url: "{controller}/{action}/{donorid}/{clientid}",
                defaults: new { controller = "Donor", action = "DocumentManage", donorid = UrlParameter.Optional, clientid = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "DocApprove",
               url: "{controller}/{action}/{docID}",
               defaults: new { controller = "Donor", action = "DocumentApprove", docID = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "DocReject",
               url: "{controller}/{action}/{docID}",
               defaults: new { controller = "Donor", action = "DocumentReject", docID = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Authentication", action = "Login", id = UrlParameter.Optional }
            );

        }

    }

    public static class RouteCollectionExtensions
    {
        public static void MapServiceRoutes(
            this RouteCollection routes,
            Dictionary<string, string> urlToVirtualPathMap,
            object defaults = null,
            object constraints = null)
        {
            foreach (var kvp in urlToVirtualPathMap)
                MapServiceRoute(routes, null, kvp.Key, kvp.Value, defaults, constraints);
        }

        public static Route MapServiceRoute(
            this RouteCollection routes,
            string url,
            string virtualPath,
            object defaults = null,
            object constraints = null)
        {
            return MapServiceRoute(routes, null, url, virtualPath, defaults, constraints);
        }

        public static Route MapServiceRoute(
            this RouteCollection routes,
            string routeName,
            string url,
            string virtualPath,
            object defaults = null,
            object constraints = null)
        {
            if (routes == null)
                throw new ArgumentNullException("routes");

            Route route = new ServiceRoute(
                url: url,
                virtualPath: virtualPath,
                defaults: new RouteValueDictionary(defaults) { { "controller", null }, { "action", null } },
                constraints: new RouteValueDictionary(constraints)
            );
            routes.Add(routeName, route);
            return route;
        }
    }

    public class ServiceRouteHandler : IRouteHandler
    {
        private readonly string virtualPath;
        private readonly WebServiceHandlerFactory handlerFactory = new WebServiceHandlerFactory();

        public ServiceRouteHandler(string virtualPath)
        {
            if (virtualPath == null)
                throw new ArgumentNullException(nameof(virtualPath));
            if (!virtualPath.StartsWith("~/"))
                throw new ArgumentException("Virtual path must start with ~/", "virtualPath");
            this.virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Strip the query string (if any) off of the file path
            string filePath = virtualPath;
            int qIndex = filePath.IndexOf('?');
            if (qIndex >= 0)
                filePath = filePath.Substring(0, qIndex);

            // Note: can't pass requestContext.HttpContext as the first 
            // parameter because that's type HttpContextBase, while 
            // GetHandler expects HttpContext.
            return handlerFactory.GetHandler(
                HttpContext.Current,
                requestContext.HttpContext.Request.HttpMethod,
                virtualPath,
                requestContext.HttpContext.Server.MapPath(filePath));
        }
    }

    public class ServiceRoute : Route
    {
        public ServiceRoute(string url, string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints)
            : base(url, defaults, constraints, new ServiceRouteHandler(virtualPath))
        {
            this.VirtualPath = virtualPath;
        }

        public string VirtualPath { get; private set; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Run a test to see if the URL and constraints don't match
            // (will be null) and reject the request if they don't.
            if (base.GetRouteData(httpContext) == null)
                return null;

            // Use URL rewriting to fake the query string for the ASMX
            httpContext.RewritePath(this.VirtualPath);

            return base.GetRouteData(httpContext);
        }
    }
}