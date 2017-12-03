using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(MonthBonus))]
    public class MonthBonusModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 月份 yyyyMM
        /// </summary>
        [Display(Name = "月份")]
        [Required(ErrorMessage = "月份不能为空")]
        public virtual string Month { get; set; }
        /// <summary>
        /// 奖金总额
        /// </summary>
        public virtual double? BonusValue { get; set; }
        /// <summary>
        /// 录入方式
        /// </summary>
        public virtual string InWay { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Remark { get; set; }

    }
}
