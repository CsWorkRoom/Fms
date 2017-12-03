using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(TargetTag))]
    public class TargetTagModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "指标标识")]
        [Required(ErrorMessage ="指标标识不能为空")]
        public virtual string Name { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

    }
}
