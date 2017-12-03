using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(MonthTargetDetail))]
    public class MonthTargetDetailModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public virtual string Month { get; set; }
        /// <summary>
        /// 月度指标固化ID
        /// </summary>
        public virtual long? MonthTargetId { get; set; }
        /// <summary>
        /// 月度指标目标值固化ID
        /// </summary>
        public virtual long? MonthTargetValueId { get; set; }
        /// <summary>
        /// 指标类型ID
        /// </summary>
        public virtual long? TargetTypeId { get; set; }
        /// <summary>
        /// 指标标识ID
        /// </summary>
        public virtual long? TargetTagId { get; set; }

        #region 辅助字段
        /// <summary>
        /// 指标类型
        /// </summary>
        public virtual string TargetTypeName { get; set; }
        /// <summary>
        /// 指标标识
        /// </summary>
        public virtual string TargetTagName { get; set; }
        /// <summary>
        /// 指标名
        /// </summary>
        public virtual string TargetName { get; set; }
        /// <summary>
        /// 客户经理归属层级名
        /// </summary>
        public virtual string DistrictName { get; set; }
        /// <summary>
        /// 标识jqgrid各行状态.格式参考：0|0、1|0
        /// </summary>
        public virtual string Status { get; set; }


        #endregion

        /// <summary>
        /// 指标ID
        /// </summary>
        public virtual long? TargetId { get; set; }
        /// <summary>
        /// 组织编号
        /// </summary>
        public virtual long? DistrictId { get; set; }
        /// <summary>
        /// 客户经理编号
        /// </summary>
        public virtual string ManagerNo { get; set; }
        /// <summary>
        /// 客户经理名称
        /// </summary>
        public virtual string ManagerName { get; set; }
        /// <summary>
        /// 指标权重
        /// </summary>
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 指标目标值
        /// </summary>
        public virtual double? TValue { get; set; }
        /// <summary>
        /// 指标完成值
        /// </summary>
        public virtual double? ResultValue { get; set; }
        /// <summary>
        /// 指标得分
        /// </summary>
        public virtual double? Score { get; set; }
        /// <summary>
        /// 指标得分比重
        /// </summary>
        public virtual double? ScoreWeight { get; set; }
        /// <summary>
        /// 领导打分
        /// </summary>
        public virtual double? MarkScore { get; set; }
        /// <summary>
        /// 最终得分= 指标得分*（1-打分比重）+领导打分*打分比重
        /// </summary>
        public virtual double? EndScore { get; set; }

    }
}
