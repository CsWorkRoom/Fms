using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    /// <summary>
    /// 表头
    /// </summary>
    [AutoMap(typeof(TbReportFieldTop))]
    public class TbReportFieldTopModel
    {
        public virtual long Id { get; set; }

        public virtual long? ParentID { get; set; }

        public virtual string ParentName { get; set; }

        public virtual long? TbReportId { get; set; }

        /// <summary>
        /// 多表头名/字段中文名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 字段编码（字段特有）
        /// </summary>
        public virtual string FieldCode { get; set; }

        public virtual string Remark { get; set; }

        public virtual long? TbReportOutEventId { get; set; }

    }
    
}
