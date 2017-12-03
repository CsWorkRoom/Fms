using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Easyman.Common.Mvc;
using Easyman.Users;

namespace Easyman.FwWeb.Controllers
{
    public class AppRequestController : EasyManController
    {
        private readonly IUserAppService _userService;

        public AppRequestController(IUserAppService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View("Easyman.FwWeb.Views.AppRequest.Index");
        }
    }
}
