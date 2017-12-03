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
    public class TargetController : EasyManController
    {
        #region 初始化

        private readonly ITargetTypeAppService _TargetTypeAppService;
        private readonly ITargetTagAppService _TargetTagAppService;
        private readonly ITargetAppService _TargetAppService;
        private readonly ITargetFormulaAppService _TargetFormulaAppService;
        private readonly IUserAppService _UserAppService;
        //private readonly ITargetValueAppService _TargetValueAppService;

        private readonly INewTargetValueAppService _NewTargetValueAppService;
        private readonly ISubitemAppService _SubitemAppService;
        private readonly IDistrictAppService _DistrictAppService;

        public TargetController(ITargetTypeAppService TargetTypeAppService,
            ITargetTagAppService TargetTagAppService,
            ITargetAppService TargetAppService,
            ITargetFormulaAppService TargetFormulaAppService,
            IUserAppService UserAppService,
            //ITargetValueAppService TargetValueAppService,
            INewTargetValueAppService NewTargetValueAppService,
            ISubitemAppService SubitemAppService,
            IDistrictAppService DistrictAppService)
        {

            _TargetTypeAppService = TargetTypeAppService;
            _TargetTagAppService = TargetTagAppService;
            _TargetAppService = TargetAppService;
            _TargetFormulaAppService = TargetFormulaAppService;
            _UserAppService = UserAppService;
            //_TargetValueAppService = TargetValueAppService;

            _NewTargetValueAppService = NewTargetValueAppService;
            _SubitemAppService = SubitemAppService;
            _DistrictAppService = DistrictAppService;
        }

        #endregion


        #region 指标类型
        public ActionResult EditTargetType(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new TargetTypeModel());
            }
            var entObj = _TargetTypeAppService.GetTargetType(id.Value);
            return View(entObj);
        }
        #endregion

        #region 指标标识
        public ActionResult EditTargetTag(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new TargetTagModel());
            }
            var entObj = _TargetTagAppService.GetTargetTag(id.Value);
            return View(entObj);
        }
        #endregion

        #region 指标
        public ActionResult EditTarget(long? id)
        {
            var entObj = new TargetModel { ChooseType = "必选", IsUse = true,CrisisValue=0 };//初始化基础数据

            if (id!=null&&id!=0)
            {
                entObj = _TargetAppService.GetTarget(id.Value);
            }
            entObj.TargetTagList = _TargetTagAppService.TargetTagList();
            entObj.TargetTypeList = _TargetTypeAppService.TargetTypeList();
            return View(entObj);
        }
        #endregion

        #region 公式
        public ActionResult EditTargetFormula(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new TargetFormulaModel());
            }
            var entObj = _TargetFormulaAppService.GetTargetFormula(id.Value);
            return View(entObj);
        }
        #endregion

        #region 指标目标值
        /// <summary>
        /// 获得各区县客户经理目标值列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerTargetValueList()
        {
            string result = "";
            //获得参数
            long targetTagId = Convert.ToInt64(Request["targetTagId"].Trim());//获得指标标识
            string month = Request["month"].Trim();//月份
            long district_id = 0;
            if (!string.IsNullOrEmpty(Request["districtId"].Trim()))
            {
                district_id = Convert.ToInt64(Request["districtId"].Trim());
            }
            else
            {
                var user = _UserAppService.GetUser(CurrUserId());
                district_id = user.DistrictId.Value;
            }

            try
            {
                //获得客户经理目标值信息
                var valueList = _NewTargetValueAppService.GetTargetValueTable(targetTagId, district_id, month);
                //拼凑json串
                result = "{\"rows\":" + JSON.DecodeToStr(valueList) + "}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Content(result);
        }
        /// <summary>
        /// 各区县客户经理目标值编辑页面
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="targetTagId"></param>
        /// <param name="districtId">区县ID</param>
        /// <returns></returns>
        public ActionResult EditManagerTargetValue(string month, long? targetTagId, long? districtId)
        {
            try
            {
                if (targetTagId == null || districtId == null || string.IsNullOrEmpty(month))
                {
                    var user = _UserAppService.GetUser(CurrUserId());
                    targetTagId = 2;
                    districtId = user.DistrictId;
                    month = DateTime.Now.ToString("yyyyMM");
                }
                ViewData["managerJson"] = _NewTargetValueAppService.GetManagerJson(districtId);
                ViewData["month"] = month;
                ViewData["targetTagId"] = targetTagId;
                ViewData["districtId"] = districtId;

                if (districtId != null)
                {
                    var dis = _DistrictAppService.GetDistrict(districtId.Value);
                    if (dis != null)
                    {
                        ViewData["districtName"] = dis.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 各区县的目标值编辑页
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="targetTagId"></param>
        /// <param name="districtId">市ID</param>
        /// <returns></returns>
        public ActionResult EditQxTargetValue(string year, long? targetTagId, long? districtId)
        {
            try
            {
                if (targetTagId == null || districtId == null || string.IsNullOrEmpty(year))
                {
                    var user = _UserAppService.GetUser(CurrUserId());
                    targetTagId = 1;
                    districtId = user.DistrictId;
                    year = DateTime.Now.Year.ToString();
                }
                ViewData["targetJson"] = _NewTargetValueAppService.GetQxTargetJson(targetTagId);
                ViewData["year"] = year;
                ViewData["targetTagId"] = targetTagId;
                ViewData["districtId"] = districtId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 区县目标值保存
        /// </summary>
        /// <param name="targetVals"></param>
        public void SaveQxTargetValueList(string targetVals)
        {
            _NewTargetValueAppService.SaveQxTargetValue(targetVals);
        }

      
        #endregion
    }
}
