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
    public class FileController : EasyManController
    {
        #region 初始化

        private readonly IFileFormatAppService _FileFormatAppService;

        public FileController(IFileFormatAppService FileFormatAppService)
        {
            _FileFormatAppService = FileFormatAppService;
        }

        #endregion


        #region 属性管理
        public ActionResult EditFileFormat(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new FileFormatModel());
            }
            var entObj = _FileFormatAppService.GetFileFormat(id.Value);
            return View(entObj);
        }
        #endregion

    }
}
