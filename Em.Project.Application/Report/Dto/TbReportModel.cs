using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    /// <summary>
    /// 表格报表属性（输入输出公用）
    /// </summary>
    [AutoMap(typeof(TbReport))]
    public class TbReportModel
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

        /// <summary>
        /// 报表类型（网格、键值）
        /// </summary>
        public virtual short ReportType { get; set; }

        /// <summary>
        /// 报表风格
        /// </summary>
        public virtual string ReportStyle { get; set; }

        public virtual bool IsCheck { get; set; }

        //于2017.7.27日取消是否调试。改由在内置事件中管理
        //public virtual bool IsDebug { get; set; }

        public virtual bool IsAutoLoad { get; set; }

        public virtual bool IsBigdataLoad { get; set; }

        public virtual int? MaxExportCount { get; set; }

        public virtual bool IsPaination { get; set; }

        public virtual bool IsScroll { get; set; }

        public virtual string EmptyRecord { get; set; }

        public virtual int? RowNum { get; set; }

        public virtual string RowList { get; set; }

        public virtual string RowStyle { get; set; }

        public virtual string SelectColor { get; set; }

        //public virtual bool IsPlaceholder { get; set; }

        public virtual bool IsOpen { get; set; }

        /// <summary>
        /// 是否显示行号
        /// </summary>
        public virtual bool IsRowNumber { get; set; }
        /// <summary>
        /// 行号宽度
        /// </summary>
        public virtual int? RownumWidth { get; set; }

        /// <summary>
        /// 字段json串
        /// </summary>
        public string FieldListJson { get; set; }

        /// <summary>
        /// 多表头列串
        /// </summary>
        public string FieldTopListJson { get; set; }

        /// <summary>
        /// 外置事件（含有参数）
        /// </summary>
        public string OutEventListJson { get; set; }

        /// <summary>
        /// 内置事件
        /// 2017.7.27上午cs添加，用于返回当前用户具有的权限的内置事件
        /// 目前只有两个内置事件：导出、调试
        /// </summary>
        public string InEventListJson { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public string FilterListJson { get; set; }
        /// <summary>
        /// 单元格合并信息
        /// </summary>
        public string MergeCellJson { get; set; }
        /// <summary>
        /// 复选框互斥（行选中）
        /// </summary>
        public virtual bool MultiboxOnly { get; set; }
        /// <summary>
        /// 是否组合排序
        /// </summary>
        public virtual bool IsMultiSort { get; set; }
        /// <summary>
        /// 是否显示头部
        /// </summary>
        public virtual bool IsShowHeader { get; set; }
        /// <summary>
        /// 是否默认显示筛选区
        /// </summary>
        public virtual bool IsShowFilter { get; set; }
        /// <summary>
        /// 自定义js
        /// </summary>
        public virtual string JsFun { get; set; }
        /// <summary>
        /// 表格报表说明(clob)
        /// </summary>
        public virtual string Remark { get; set; }
    }

}
