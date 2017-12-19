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
    public class AttrController : EasyManController
    {
        #region 初始化

        private readonly IAttrTypeAppService _AttrTypeAppService;
        private readonly IAttrAppService _AttrAppService;

        public AttrController(IAttrTypeAppService AttrTypeAppService,
                                    IAttrAppService AttrAppService)
        {
            _AttrTypeAppService = AttrTypeAppService;
            _AttrAppService = AttrAppService;
        }

        #endregion


        #region 属性类型
        public ActionResult EditAttrType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new AttrTypeModel());
            }
            var entObj = _AttrTypeAppService.GetAttrType(id.Value);
            return View(entObj);
        }
        #endregion

        #region 属性管理
        public ActionResult EditAttr(long? id)
        {
            var entObj = new AttrModel();//初始化基础数据

            if (id != null && id != 0)
            {
                entObj = _AttrAppService.GetAttr(id.Value);
            }
            entObj.AttrTypeList = _AttrTypeAppService.AttrTypeList();
            return View(entObj);
        }
        #endregion

    }
}
