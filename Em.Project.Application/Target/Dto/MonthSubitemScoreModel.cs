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
    [AutoMap(typeof(MonthSubitemScore))]
    public class MonthSubitemScoreModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public virtual string Month { get; set; }
        /// <summary>
        /// 打分项
        /// </summary>
        public virtual long? SubitemId { get; set; }

        /// <summary>
        /// 组织
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
        /// 打分值
        /// </summary>
        public virtual double? MarkScore { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

    }
}
