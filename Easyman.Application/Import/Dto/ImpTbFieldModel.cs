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
    [AutoMap(typeof(ImpTbField))]
    public class ImpTbFieldInput : EntityDto<long>
    {
        /// <summary>
        /// 字段类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 字段别名
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "字段别名不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "字段别名")]
        public virtual string FieldName { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "字段编码不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "字段编码")]
        public virtual string FieldCode { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "字段类型不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "字段类型")]
        public virtual string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string DType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "备注")]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public virtual long RegularId { get; set; }
        /// <summary>
        /// 外导表
        /// </summary>
        public virtual long ImpTbId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(ImpTbField))]
    public class ImpTbFieldOutput : EntityDto<long>
    {
        /// <summary>
        /// 字段类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 字段别名
        /// </summary>
        public virtual string FieldName { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        public virtual string FieldCode { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public virtual string DataType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public virtual string RegularName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTbFieldSearchOutput : SearchOutputDto<ImpTbFieldOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTbFieldSearchInput : SearchInputDto { }
}
