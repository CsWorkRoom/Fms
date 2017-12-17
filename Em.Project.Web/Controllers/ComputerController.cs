using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Easyman.Web.Controllers
{
    public class ComputerController : EasyManController
    {
        #region 初始化

        private readonly IComputerTypeAppService _ComputerTypeAppService;
       

        public ComputerController(IComputerTypeAppService ComputerTypeAppService)
        {

            _ComputerTypeAppService = ComputerTypeAppService;
        }

        #endregion


        #region 终端类型
        public ActionResult EditComputerType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerTypeModel());
            }
            var entObj = _ComputerTypeAppService.GetComputerType(id.Value);
            return View(entObj);
        }
        #endregion

       
    }
}
