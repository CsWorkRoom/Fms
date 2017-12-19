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
    [AutoMap(typeof(Attr))]
    public class AttrModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 属性类型ID
        /// </summary>
        public virtual long? AttrTypeId { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        [Display(Name = "属性名")]
        [Required(ErrorMessage = "属性名不能为空")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }


        public List<SelectListItem> AttrTypeList { get; set; }

    }
}
