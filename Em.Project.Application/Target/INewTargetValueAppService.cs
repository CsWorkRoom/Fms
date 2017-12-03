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
    /// 指标目标
    /// </summary>
    public interface INewTargetValueAppService : IApplicationService
    {
        #region 获得当前指标目标值列表
        /// <summary>
        /// 获取指定区县的目标值列表json串
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        string GetQxTargetValueJson(QxTargetVal tv);

        /// <summary>
        /// 根据指标标识ID去获取对应的指标目标值列表（有效的、全量的）
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <returns></returns>
        DataTable GetTargetValueTable(long targetTagId,long districtId, string yearMonth);
        #endregion

        #region 行转列-基准列
        /// <summary>
        /// 获取指定区县的客户经理列表
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        string GetManagerJson(long? districtId);
        /// <summary>
        /// 获取区县指标列表json串
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <returns></returns>
        string GetQxTargetJson(long? targetTagId);
        #endregion

        #region 目标值保存
        /// <summary>
        /// 客户经理目标值保存
        /// </summary>
        /// <param name="vals"></param>
        void SaveManagerTargetValue(TargetVals vals);
        /// <summary>
        /// 区县目标值保存
        /// </summary>
        /// <param name="vals"></param>
        void SaveQxTargetValue(string vals);
        #endregion

    }
}
