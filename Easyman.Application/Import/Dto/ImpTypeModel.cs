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
    [AutoMap(typeof(ImpType))]
    public class ImpTypeInput : EntityDto<long>
    {
        /// <summary>
        /// 外导表分类ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "备注")]
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(ImpType))]
    public class ImpTypeOutput : EntityDto<long>
    {
        /// <summary>
        /// 外导表分类ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTypeSearchOutput : SearchOutputDto<ImpTypeOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTypeSearchInput : SearchInputDto { }
}
