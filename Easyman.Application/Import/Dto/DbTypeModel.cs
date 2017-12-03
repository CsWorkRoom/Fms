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
    [AutoMap(typeof(DbType))]
    public class DbTypeInput : EntityDto<long>
    {
        /// <summary>
        /// 数据库类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 数据库类型名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "数据库类型名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "数据库类型名称")]
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
    [AutoMap(typeof(DbType))]
    public class DbTypeOutput : EntityDto<long>
    {
        /// <summary>
        /// 数据库类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 数据类型名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "数据库类型名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DbTypeSearchOutput : SearchOutputDto<DbTypeOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class DbTypeSearchInput : SearchInputDto { }
}
