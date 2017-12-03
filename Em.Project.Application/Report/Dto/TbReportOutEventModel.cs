using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(TbReportOutEvent))]
    public class TbReportOutEventModel
    {
        public virtual long Id { get; set; }

        public virtual long? TbReportId { get; set; }

        public virtual string EventType { get; set; }
        /// <summary>
        /// 此处注意：多表头和列以这个字段判断
        /// </summary>
        public virtual string FieldCode { get; set; }

        public virtual string DisplayWay { get; set; }

        public virtual string DisplayCondition { get; set; }

        public virtual string OpenWay { get; set; }

        public virtual string Url { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Icon { get; set; }

        public virtual string Style { get; set; }

        public virtual string Title { get; set; }

        public virtual int? Height { get; set; }

        public virtual int? Width { get; set; }

        public virtual string GroupName { get; set; }

        public virtual int? OrderNum { get; set; }

        public string ParamListJson { get; set; }

        /// <summary>
        /// 标识符（区分事件唯一性）
        /// </summary>
        public virtual string Identifier { get; set; }
    }
}