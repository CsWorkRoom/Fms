using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(RdlcReport))]
    public class RdlcReportModel
    {
        /// <summary>
        /// 0=新增
        /// </summary>
        public virtual long Id { get; set; }

        public virtual long? ReportId { get; set; }
        /// <summary>
        /// 报表名称
        /// </summary>
        public virtual string ReportName { get; set; }
        /// <summary>
        /// 报表编码
        /// </summary>
        public virtual string ReportCode { get; set; }

        /// <summary>
        /// 应用侧（APP、PC）
        /// </summary>
        public virtual string ApplicationType { get; set; }
        public virtual int? RowNum { get; set; }
        public virtual bool IsOpen { get; set; }
        /// <summary>
        /// rdlc配置xml
        /// </summary>
        public virtual string RdlcXml { get; set; }
        /// <summary>
        /// 是否默认显示筛选区
        /// </summary>
        public virtual bool IsShowFilter { get; set; }
        /// <summary>
        /// 表格报表说明(clob)
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 内置事件
        /// 用于返回当前用户具有的权限的内置事件
        /// 目前只有两个内置事件：导出、调试
        /// </summary>
        public string InEventListJson { get; set; }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public string FilterListJson { get; set; }

    }
}
