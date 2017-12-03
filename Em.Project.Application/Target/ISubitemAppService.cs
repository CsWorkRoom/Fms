using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using System.Data;

namespace Easyman.Service
{
    /// <summary>
    /// 打分项管理
    /// </summary>
    public interface ISubitemAppService : IApplicationService
    {
        #region 打分项目基础管理
        /// <summary>
        /// 根据ID获取某个打分项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubitemModel GetSubitem(long id);
        /// <summary>
        /// 更新和新增打分项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        SubitemModel InsertOrUpdateSubitem(SubitemModel input);

        /// <summary>
        /// 删除一条打分项
        /// </summary>
        /// <param name="input"></param>
        void DeleteSubitem(EntityDto<long> input);
        /// <summary>
        /// 获取打分项json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetSubitemTreeJson();
        /// <summary>
        /// 获取所有类型 List<SelectListItem>
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> SubitemList();
        /// <summary>
        /// 获取所有类型 List<SubitemModel>
        /// </summary>
        /// <returns></returns>
        List<SubitemModel> AllSubitemList();
        #endregion

        #region 领导打分
        /// <summary>
        /// 获取指定区县的打分项json串
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        string GetSubitemJson(SubitemScore ss);
        /// <summary>
        /// 获取指定区县/月份的打分值table
        /// </summary>
        /// <param name="districtId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        DataTable GetSubitemScoreTable( long districtId, string month);
        /// <summary>
        /// 循环保存打分值
        /// </summary>
        /// <param name="sbScores"></param>
        void SaveSubitemScore(string sbScores);
        #endregion
    }
}
