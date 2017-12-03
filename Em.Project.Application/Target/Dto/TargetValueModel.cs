using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Easyman.Dto
{
    [AutoMap(typeof(TargetValue))]
    public class TargetValueModel
    {
        public virtual long Id { get; set; }

        /// <summary>
        /// 年份/月份
        /// </summary>
        public virtual string YearMonth { get; set; }

        public virtual long? TargetTagId { get; set; }

        #region 指标信息相关字段(过滤掉未用指标)
        /// <summary>
        /// 指标类型
        /// </summary>
        public virtual string TargetTypeName { get; set; }
        /// <summary>
        /// 指标名
        /// </summary>
        public virtual string TargetName { get; set; }
        /// <summary>
        /// 可选/必选
        /// </summary>
        public virtual string ChooseType { get; set; }
        /// <summary>
        /// 指标权重
        /// </summary>
        public virtual double? TargetWeight { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        public virtual string TargetRemark { get; set; }
        #endregion


        public virtual long? TargetId { get; set; }
        /// <summary>
        /// 组织（有值：客户经理指标。无值：区县指标） 
        /// </summary>
        public virtual long? DistrictId { get; set; }

        #region 20171026新增两字段MANAGER_NO、MANAGER_NAME
        /// <summary>
        /// 客户经理编号
        /// </summary>
        public virtual string ManagerNo { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public virtual string ManagerName { get; set; }
        #endregion

        /// <summary>
        /// 目标值
        /// </summary>
        public virtual double? TValue { get; set; }
        ///// <summary>
        ///// 指标得分比重--20171026取消字段
        ///// </summary>
        //public virtual double? ScoreWeight { get; set; }
        ///// <summary>
        ///// 是否在用（是否选中）  --20171030该字段作废
        ///// </summary>
        //public virtual bool IsUse { get; set; }

    }
}
