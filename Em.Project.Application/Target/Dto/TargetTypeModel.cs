using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(TargetType))]
    public class TargetTypeModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "指标类型")]
        [Required(ErrorMessage ="指标类型不能为空")]
        public virtual string Name { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

    }
}
