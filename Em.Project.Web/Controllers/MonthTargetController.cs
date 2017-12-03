using Abp.AutoMapper;
using Abp.Domain.Uow;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using EasyMan.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;


namespace Easyman.Web.Controllers
{
    public class MonthTargetController : EasyManController
    {
        #region 初始化

        private readonly IMonthBonusAppService _MonthBonusAppService;
        private readonly IUserAppService _UserAppService;
        private readonly ITargetTypeAppService _TargetTypeAppService;
        private readonly ITargetAppService _TargetAppService;

        public MonthTargetController(IMonthBonusAppService MonthBonusAppService, 
            IUserAppService UserAppService,
            ITargetTypeAppService TargetTypeAppService,
             ITargetAppService TargetAppService)
        {

            _MonthBonusAppService = MonthBonusAppService;
            _UserAppService = UserAppService;
            _TargetTypeAppService = TargetTypeAppService;
            _TargetAppService = TargetAppService;
        }

        #endregion


        #region 月度总奖金
        public ActionResult EditMonthBonus(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new MonthBonusModel());
            }
            var entObj = _MonthBonusAppService.GetMonthBonus(id.Value);
            return View(entObj);
        }
        #endregion

    }
}
