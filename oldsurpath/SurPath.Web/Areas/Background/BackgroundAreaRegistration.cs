using System.Web.Mvc;

namespace SurPathWeb.Areas.Background
{
    public class BackgroundAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Background";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Background_default",
                "Background/{controller}/{action}/{id}",
                new { controller = "BackgroundAuth", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}