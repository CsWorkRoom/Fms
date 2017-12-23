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
        private readonly IFolderAppService _FolderAppService;
        private readonly IDistrictAppService _DistrictAppService;

        public ComputerController(IComputerTypeAppService ComputerTypeAppService,
                                    IComputerAppService ComputerAppService,
                                    IFolderAppService FolderAppService,
                                    IDistrictAppService DistrictAppService)
        {

            _ComputerTypeAppService = ComputerTypeAppService;
            _ComputerAppService = ComputerAppService;
            _FolderAppService = FolderAppService;
            _DistrictAppService = DistrictAppService;
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
            var entObj = new ComputerModel { IsUse=true};

            if (id != null && id != 0)
            {
                entObj = _ComputerAppService.GetComputer(id.Value);
            }
            entObj.ComputerTypeList = _ComputerTypeAppService.ComputerTypeList();        
            return View(entObj);
        }
        #endregion

        #region 终端共享目录
        public ActionResult EditFolder(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new ComputerModel());
            }
            var entObj = _FolderAppService.GetFolder(id.Value);
            return View(entObj);
        }
        #endregion

        #region 资源管理器（终端、目录、文件查看）
        public ActionResult Explorer()
        {
            //根据当前用户返回对应树结构（先同步、暂不考虑异步）
            return View();
        }
        #endregion
    }
}
