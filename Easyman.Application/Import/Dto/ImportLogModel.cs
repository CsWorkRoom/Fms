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
    [AutoMap(typeof(ImportLog))]
    public class ImportLogInput : EntityDto<long>
    {
        /// <summary>
        /// 代码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "代码不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "代码")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 菜单代码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "菜单代码")]
        public virtual string ModuleCode { get; set; }
        /// <summary>
        /// 文件编号
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "文件编号")]
        public virtual long? FileId { get; set; }
        /// <summary>
        /// 导入模式
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "导入模式")]
        public virtual string ImportMode { get; set; }

        /// <summary>
        /// 导入模式
        /// </summary>
        [System.ComponentModel.DataAnnotations.Display(Name = "自定义后缀")]
        public virtual string Suffix { get; set; }
        
        /// <summary>
        /// 离线运行时间
        /// </summary>
        //[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        //[System.ComponentModel.DataAnnotations.Display(Name = "名称")]
        //public virtual DateTime? OfflineRunTime { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(ImportLog))]
    public class ImportLogOutput : EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "编码不能为空")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "表名称不能为空")]
        public virtual string TableName { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public virtual string DatabaseName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImportLogSearchOutput : SearchOutputDto<ImportLogOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class ImportLogSearchInput : SearchInputDto { }
}
