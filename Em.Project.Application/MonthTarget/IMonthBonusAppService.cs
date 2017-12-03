using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    /// <summary>
    /// 月度总奖金管理
    /// </summary>
    public interface IMonthBonusAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个月度总奖金
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MonthBonusModel GetMonthBonus(long id);

        /// <summary>
        /// 根据月份month获取某个月度总奖金
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        MonthBonusModel GetMonthBonus(string month);
        /// <summary>
        /// 更新和新增月度总奖金
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        MonthBonusModel InsertOrUpdateMonthBonus(MonthBonusModel input);

        /// <summary>
        /// 删除一条月度总奖金
        /// </summary>
        /// <param name="input"></param>
        void DeleteMonthBonus(EntityDto<long> input);


    }
}
