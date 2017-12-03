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
    [AutoMap(typeof(Subitem))]
    public class SubitemModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 打分类型ID
        /// </summary>
        public virtual long? SubitemTypeId { get; set; }

        public virtual string SubitemTypeName { get; set; }

        /// <summary>
        /// 打分项
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 打分权重分
        /// </summary>
        [Required, RegularExpression(@"^[0-9]+(.[0-9]{2})?$", ErrorMessage = "应为数值型")]//有两位小数的正实数
        public virtual double? Weight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        
        /// <summary>
        /// 打分项类型列表
        /// </summary>
        public List<SelectListItem> SubitemTypeList { get; set; }

    }
}
