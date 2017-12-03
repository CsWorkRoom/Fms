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
    [AutoMap(typeof(PreDataType))]
    public class PreDataTypeInput : EntityDto<long>
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 类型名称(描述)
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "类型名称(描述)不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "类型名称(描述)")]
        public virtual string Name { get; set; }
        /// <类型定义>
        /// 数据类型
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "类型定义不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "类型定义")]
        public virtual string DataType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "备注")]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 数据库类型ID
        /// </summary>
        public virtual long DbTypeId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(PreDataType))]
    public class PreDataTypeOutput : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 类型名称(描述)
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "类型名称(描述)不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 类型定义
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "类型定义不能为空")]
        public virtual string DataType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public virtual string DbTypeName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PreDataTypeSearchOutput : SearchOutputDto<PreDataTypeOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class PreDataTypeSearchInput : SearchInputDto { }

}
