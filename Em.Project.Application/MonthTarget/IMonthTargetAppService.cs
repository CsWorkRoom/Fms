using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    /// <summary>
    /// 指标固化+结果运算
    /// </summary>
    public interface IMonthTargetAppService : IApplicationService
    {
        /// <summary>
        /// 固化指定月份指标（区县+客户经理指标/目标值）
        /// </summary>
        /// <param name="month"></param>
        /// <param name="curWay"></param>
        void CuringMonthTarget(CurTarget obj);
        
    }
}
