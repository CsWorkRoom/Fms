using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(Easyman.Domain.Report))]
    public class ReportInput : EntityDto<long>
    {

        public new long Id { get; set; }

        public virtual long? DbServerId { get; set; }

        [Required(ErrorMessage = "报表名称不能为空")]
        [Display(Name = "报表名称")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "编号不能为空")]
        [Display(Name = "报表编号")]
        public virtual string Code { get; set; }

        [Required(ErrorMessage = "查询sql")]
        [Display(Name = "SQL")]
        public virtual string Sql { get; set; }

        public virtual string Remark { get; set; }

        [Display(Name = "字段信息集合")]
        public virtual string FieldJson { get; set; }

        /// <summary>
        /// 子报表的集合（用于在页面表格列表展示）
        /// 体现子报表详细配置信息
        /// 用于放置各类子报表信息（表格、键值、图形、RDLC等）
        /// </summary>
        public virtual string ChildReportListJson { get; set; }

        /// <summary>
        /// 是否开启查询条件占位
        /// 若开启占位，系统不会自动拼凑查询筛选条件，
        /// 而是由用户在sql中自行编写
        /// </summary>
        public virtual bool IsPlaceholder { get; set; }

    }

    [AutoMap(typeof(Report))]
    public class ReportOutput : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual long? EmId { get; set; }

        public virtual long? DbServerId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Sql { get; set; }

        public virtual string Remark { get; set; }

        [Display(Name = "字段信息集合")]
        public virtual string FieldJson { get; set; }

        /// <summary>
        /// 子报表的集合（用于在页面表格列表展示）
        /// 体现子报表详细配置信息
        /// 用于放置各类子报表信息（表格、键值、图形、RDLC等）
        /// </summary>
        public virtual string ChildReportListJson { get; set; }

        /// <summary>
        /// 是否开启查询条件占位
        /// 若开启占位，系统不会自动拼凑查询筛选条件，
        /// 而是由用户在sql中自行编写
        /// </summary>
        public virtual bool? IsPlaceholder { get; set; }

        /// <summary>
        /// url链接中的默认参数串
        /// </summary>
        public virtual string KVJson { get; set; }

    }

}
