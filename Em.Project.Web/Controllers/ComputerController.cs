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
        private readonly IComputerAppService _ComputerAppService;
        private readonly IComputerShareFolderAppService _ComputerShareFolderAppService;

        public ComputerController(IComputerTypeAppService ComputerTypeAppService,
                                    IComputerAppService ComputerAppService,
                                    IComputerShareFolderAppService ComputerShareFolderAppService)
        {

            _ComputerTypeAppService = ComputerTypeAppService;
            _ComputerAppService = ComputerAppService;
            _ComputerShareFolderAppService = ComputerShareFolderAppService;
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

        #region 终端管理
        public ActionResult EditComputer(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerModel());
            }
            var entObj = _ComputerAppService.GetComputer(id.Value);
            return View(entObj);
        }
        #endregion

        #region 终端管理
        public ActionResult EditComputerShareFolder(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerModel());
            }
            var entObj = _ComputerShareFolderAppService.GetComputerShareFolder(id.Value);
            return View(entObj);
        }
        #endregion
    }
}
