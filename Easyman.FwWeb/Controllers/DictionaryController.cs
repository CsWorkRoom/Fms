using Abp.AutoMapper;
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
using System.Web.Mvc;


namespace Easyman.Web.Controllers
{
    public class DictionaryController : EasyManController
    {
        #region 初始化

        private readonly IDictionaryTypeAppService _DictionaryTypeAppService;
        private readonly IDictionaryAppService _DictionaryAppService;
        private readonly IUserAppService _UserAppService;

        public DictionaryController(IDictionaryTypeAppService DictionaryTypeAppService,
            IDictionaryAppService DictionaryAppService,
            IUserAppService UserAppService)
        {

            _DictionaryTypeAppService = DictionaryTypeAppService;
            _DictionaryAppService = DictionaryAppService;
            _UserAppService = UserAppService;
        }

        #endregion


        #region 字典类型
        public ActionResult EditDictionaryType(long? id)
        {
           
            var data = new DictionaryTypeModel();
            if (id != null)
            {
                data = _DictionaryTypeAppService.GetDictionaryType(id.Value);
            }
            return View("Easyman.FwWeb.Views.Dictionary.EditDictionaryType", data);
        }
        #endregion

        #region 字典
        public ActionResult EditDictionary(long? id)
        {
            var entObj = new DictionaryModel();//初始化基础数据

            if (id!=null&&id!=0)
            {
                entObj = _DictionaryAppService.GetDictionary(id.Value);
            }
            entObj.DictionaryTypeList = _DictionaryTypeAppService.DictionaryTypeList();
            return View("Easyman.FwWeb.Views.Dictionary.EditDictionary", entObj);
        }
        #endregion

    }
}
