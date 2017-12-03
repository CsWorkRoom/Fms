using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(TargetFormula))]
    public class TargetFormulaModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 公式类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 公式名
        /// </summary>
        [Display(Name = "公式名称")]
        [Required(ErrorMessage = "公式名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 中文表达式
        /// </summary>
        public virtual string CnExpression { get; set; }
        /// <summary>
        /// 表达式(暂未启用)
        /// </summary>
        public virtual string EnExpression { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

    }
}
