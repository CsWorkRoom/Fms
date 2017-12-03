using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(InEvent))]
    public class InEventModel
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 报表类型（rdlc、表格、图形...）
        /// </summary>
        public virtual short ReportType { get; set; }
        /// <summary>
        /// 事件名
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 按钮html
        /// </summary>
        public virtual string BtnHtml { get; set; }

        public virtual string BtnJs { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}