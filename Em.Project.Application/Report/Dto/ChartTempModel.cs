using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Easyman.Dto
{
    [AutoMap(typeof(ChartTemp))]
    public class ChartTempModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 图表种类ID
        /// </summary>
        public virtual long? ChartTypeId { get; set; }

        public virtual string ChartTypeName { get; set; }

        public virtual short? TempType { get; set; }

        /// <summary>
        /// 模版名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public virtual string TempCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 打分项类型列表
        /// </summary>
        public List<SelectListItem> ChartTypeList { get; set; }

    }
}
