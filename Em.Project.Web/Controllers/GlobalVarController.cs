using Abp.AutoMapper;
using Abp.Domain.Uow;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Web.Mvc;

namespace Easyman.FwWeb.Controllers
{
    public class GlobalVarController : EasyManController
    {
        #region 初始化

        private readonly IGlobalVarAppService _globalVarService;

        public GlobalVarController(IGlobalVarAppService globalVarService)
        {
            _globalVarService = globalVarService;
        }

        #endregion


        public ActionResult EditGlobalVar(long? id)
        {
            if(id==null||id==0)
            {
                return View(new GlobalVarModel());
            }
            var glob = _globalVarService.GetGlobalVar(id.Value);
            return View(glob);
        }
    }
}
