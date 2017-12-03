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
    [AutoMap(typeof(Target))]
    public class TargetModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 指标类型ID
        /// </summary>
        public virtual long? TargetTypeId { get; set; }

        /// <summary>
        /// 指标标识ID
        /// </summary>
        public virtual long? TargetTagId { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        [Display(Name = "指标名称")]
        [Required(ErrorMessage = "指标名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 可选/必选
        /// </summary>
        public virtual string ChooseType { get; set; }

        /// <summary>
        /// 指标权重
        /// </summary>
        [Display(Name = "指标权重")]
        [Required(ErrorMessage = "指标权重不能为空")]
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool IsUse { get; set; }
        /// <summary>
        /// 源头表(结果表)
        /// </summary>
        [Display(Name = "结果表")]
        [Required(ErrorMessage = "结果表不能为空")]
        public virtual string EndTable { get; set; }
        /// <summary>
        /// 主字段
        /// </summary>
        public virtual string MainField { get; set; }
        /// <summary>
        /// 计分门槛值
        /// </summary>
        public virtual double? CrisisValue { get; set; }
        /// <summary>
        /// 指标类型列表
        /// </summary>
        public List<SelectListItem> TargetTypeList { get; set; }
        /// <summary>
        /// 指标标识列表
        /// </summary>
        public List<SelectListItem> TargetTagList { get; set; }

    }
}
