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
    [AutoMap(typeof(ImpTb))]
    public class ImpTbInput : EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "代码不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "代码")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 表中文名称别名
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "表别名不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "表别名")]
        public virtual string CnTableName { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "表名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "表名称")]
        public virtual string EnTableName { get; set; }
        /// <summary>
        /// 建表规则
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "建表规则")]
        public virtual string Rule { get; set; }
        /// <summary>
        /// 建表语句
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "建表语句")]
        public virtual string Sql { get; set; }
        /// <summary>
        /// 执行库ID
        /// </summary>
        public virtual long DbServerId { get; set; }
        /// <summary>
        /// 外导表分类
        /// </summary>
        public virtual long ImpTypeId { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public virtual long DbTypeId { get; set; }
        /// <summary>
        /// 内置字段id用都好分割
        /// </summary>
        public virtual string DefaultFields { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(ImpTb))]
    public class ImpTbOutput : EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 表中文名称别名
        /// </summary>
        public virtual string CnTableName { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public virtual string EnTableName { get; set; }
        /// <summary>
        /// 建表规则
        /// </summary>
        public virtual string Rule { get; set; }
        /// <summary>
        /// 执行库ID
        /// </summary>
        public virtual string DbServerName { get; set; }
        /// <summary>
        /// 外导表分类
        /// </summary>
        public virtual string ImpTypeName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTbSearchOutput : SearchOutputDto<ImpTbOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class ImpTbSearchInput : SearchInputDto { }

}
