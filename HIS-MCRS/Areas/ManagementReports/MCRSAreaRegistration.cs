using System.Web.Mvc;

namespace HIS_MCRS.Areas.ManagementReports
{
    public class MCRSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ManagementReports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ManagementReports_default",
                "ManagementReports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                //,new[] { "HIS_MCRS.Areas.ManagementReports.Controllers" }
            );

         
        }
    }
}
