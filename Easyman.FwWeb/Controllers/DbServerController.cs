using Abp.AutoMapper;
using Abp.Domain.Uow;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Import;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Web.Mvc;


namespace Easyman.FwWeb.Controllers
{
    public class DbServerController : EasyManController
    {
        #region 初始化

        private readonly IDbTagAppService _dbTagAppService;
        private readonly IDbServerAppService _dbServerService;
        private readonly IDbTypeAppService _dbTypeAppService;

        public DbServerController(IDbTagAppService dbTagService, IDbServerAppService dbServerService, IDbTypeAppService dbTypeAppService)
        {

            _dbTagAppService = dbTagService;
            _dbServerService = dbServerService;
            _dbTypeAppService = dbTypeAppService;
        }

        #endregion


        #region 数据库库标识

        public ActionResult InserDbTag()
        {
            var model = new DbTagInput();
            return View("Easyman.FwWeb.Views.DbServer.EditDbTag", model);
        }

        public ActionResult EditDbTag(long dbTagId)
        {
            var tag = _dbTagAppService.GetDbTag(dbTagId);
            var model = tag == null ? new DbTagInput() : new DbTagInput() { Id = tag.Id, Name = tag.Name, Remark = tag.Remark };
            return View("Easyman.FwWeb.Views.DbServer.EditDbTag", model);
        }

        public ActionResult DbTag()
        {
            return View("Easyman.FwWeb.Views.DbServer.DbTag");
        }

        //public ActionResult SaveEditDbTag(DbTagInput tag)
        //{
        //    try
        //    {
        //        _dbTagAppService.InsertOrUpdateDbTag(tag);
        //        return Json("保存成功");
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        #endregion

        #region 数据库管理

        public ActionResult InserDbServer()
        {
            var model = new DbServerInput();
            ViewBag.DbType = _dbTypeAppService.GetDropDownList();
            return View("Easyman.FwWeb.Views.DbServer.EditDbServer", model);
        }

        public ActionResult EditDbServer(long dbServerId)
        {
            var server = _dbServerService.GetDbServer(dbServerId);
            ViewBag.DbType = _dbTypeAppService.GetDropDownList();
            var model = server == null ? new DbServerInput() :
                AutoMapper.Mapper.Map<DbServerInput>(
                AutoMapper.Mapper.Map<Domain.DbServer>(server));
            return View("Easyman.FwWeb.Views.DbServer.EditDbServer", model);
        }

        public ActionResult DbServer()
        {
            return View("Easyman.FwWeb.Views.DbServer.DbServer");
        }

        #endregion

    }
}
