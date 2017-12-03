using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Regulars))]
    public class RegularInput : EntityDto<long>
    {
        /// <summary>
        /// 正则表达式ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 正则表达式名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        //[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "正则表达式不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "正则表达式")]
        public virtual string Regular { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "错误提示")]
        public virtual string ErrorMsg { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "备注")]
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Regulars))]
    public class RegularOutput : EntityDto<long>
    {
        /// <summary>
        /// 正则表达式ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 正则表达式名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "正则表达式不能为空")]
        public virtual string Regular { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "错误提示")]
        public virtual string ErrorMsg { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "备注")]
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RegularSearchOutput : SearchOutputDto<RegularOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class RegularSearchInput : SearchInputDto { }
}
