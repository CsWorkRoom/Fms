using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(AttrType))]
    public class AttrTypeModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "属性类型")]
        [Required(ErrorMessage ="属性类型不能为空")]
        public virtual string Name { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

    }
}
