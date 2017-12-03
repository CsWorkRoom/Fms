using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.FwWeb
{
    public class EasymanFwWebAreaRegistration : AreaRegistration
    {
        public override string AreaName => "FwWeb";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FwWeb_default",
                "FwWeb/{controller}/{action}/{id}",
                new { controller = "Test", action = "Index", id = UrlParameter.Optional },
                new[] { "Easyman.FwWeb.Controllers" }
            );
        }
    }
}
