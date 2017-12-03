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
using System.Collections.Generic;
using System.Web.Mvc;


namespace Easyman.Web.Controllers
{
    public class SubitemController : EasyManController
    {
        #region 初始化

        private readonly ISubitemTypeAppService _SubitemTypeAppService;
        private readonly ISubitemAppService _SubitemAppService;
        private readonly IUserAppService _UserAppService;
        private readonly IDistrictAppService _DistrictAppService;

        public SubitemController(ISubitemTypeAppService SubitemTypeAppService,
            ISubitemAppService SubitemAppService,
            IUserAppService UserAppService,
            IDistrictAppService DistrictAppService)
        {

            _SubitemTypeAppService = SubitemTypeAppService;
            _SubitemAppService = SubitemAppService;
            _UserAppService = UserAppService;
            _DistrictAppService = DistrictAppService;
        }

        #endregion


        #region 打分类型
        public ActionResult EditSubitemType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new SubitemTypeModel());
            }
            try
            {
                var entObj = _SubitemTypeAppService.GetSubitemType(id.Value);
                return View(entObj);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion

        #region 打分项
        public ActionResult EditSubitem(long? id)
        {
            var entObj = new SubitemModel();//初始化基础数据

            if (id!=null&&id!=0)
            {
                entObj = _SubitemAppService.GetSubitem(id.Value);
            }
            entObj.SubitemTypeList = _SubitemTypeAppService.SubitemTypeList();
            //List<SelectListItem> objList = new List<SelectListItem>();
            //objList.Add(new SelectListItem { Text = "--请选择--", Value = "" });
            //var typeList = _SubitemTypeAppService.SubitemTypeList();
            //if(typeList!=null&&typeList.Count>0)
            //{
            //    objList.InsertRange(1, typeList);
            //}
            //entObj.SubitemTypeList = objList;
            return View(entObj);
        }
        #endregion

        #region 领导打分
        public ActionResult MarkScore(string month, long? districtId)
        {
            try
            {
                ViewData["subitemJson"] = JSON.DecodeToStr(_SubitemAppService.AllSubitemList());
                ViewData["month"] = month;
                if(districtId!=null)
                {
                    var dis= _DistrictAppService.GetDistrict(districtId.Value);
                    if(dis!=null)
                    {
                        ViewData["districtName"] = dis.Name;
                    }
                }
                else
                {
                    var user = _UserAppService.GetUser(CurrUserId());
                    districtId = user.DistrictId.Value;
                    ViewData["districtName"] = user.District.Name;
                }
                ViewData["districtId"] = districtId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return View();
        }

        public void SaveSubitemScoreList(string sbScores)
        {
            _SubitemAppService.SaveSubitemScore(sbScores);
        }
        #endregion
    }
}
