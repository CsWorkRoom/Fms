using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标目标
    /// </summary>
    public interface ITargetValueAppService : IApplicationService
    {
        #region 
        /// <summary>
        /// 根据指标标识和组织获取对应的指标目标列表
        /// </summary>
        /// <param name="targetTagId">指标标识</param>
        /// <param name="districtId">组织 1=默认获取市设定的区县指标,其他=区县设定的客户经理指标值</param>
        /// <returns></returns>
        List<TargetValueModel> GetTargetValueList(long targetTagId,long districtId);

        /// <summary>
        /// 更新保存设定的指标
        /// </summary>
        /// <param name="targetVals">targetVals的指标集合json</param>
        /// <returns></returns>
        void SaveTargetValueList(string targetVals);
    
        #endregion
        
    }
}
