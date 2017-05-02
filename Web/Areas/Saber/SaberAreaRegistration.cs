using System.Web.Mvc;

namespace Web.Areas.Saber
{
    public class SaberAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Saber";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Saber_default",
                "Saber/{controller}/{action}/{id}/{params1}",
                new { controller = "Index", action = "Index", id = UrlParameter.Optional, params1 = UrlParameter.Optional },
                    new string[] { "Web.Areas.Saber.Controllers" }
            ); 
        }
    }
}
