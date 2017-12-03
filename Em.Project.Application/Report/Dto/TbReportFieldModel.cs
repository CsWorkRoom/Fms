using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(TbReportField))]
    public class TbReportFieldModel
    {
        /// <summary>
        /// 0=新增
        /// </summary>
        public virtual long Id { get; set; }

        public virtual long? ReportId { get; set; }

        public virtual long? TbReportId { get; set; }
        /// <summary>
        /// 多表头ID
        /// </summary>
        public virtual long? TbReportFieldTopId { get; set; }
        /// <summary>
        /// 多表头名称
        /// </summary>
        public virtual string TbReportFieldTopName { get; set; }

        public virtual string FieldCode { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string DataType { get; set; }

        public virtual bool IsOrder { get; set; }

        public virtual bool IsShow { get; set; }

        public virtual int? Width { get; set; }

        public virtual bool IsSearch { get; set; }

        public virtual bool IsFrozen { get; set; }

        public virtual string Align { get; set; }

        public virtual int? OrderNum { get; set; }

        public virtual string Remark { get; set; }

        public virtual long? TbReportOutEventId { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public virtual bool IsDelete { get; set; }
    }
    
}