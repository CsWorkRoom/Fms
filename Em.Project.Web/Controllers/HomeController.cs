using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Easyman.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : EasymanControllerBase
    {
        public ActionResult Index()
        {
            return View(); //Layout of the angular application.
        }

        public ActionResult NoAccess()
        {
            return View(); //Layout of the angular application.
        }
    }
}