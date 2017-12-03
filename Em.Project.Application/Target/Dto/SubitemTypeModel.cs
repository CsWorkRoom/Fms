using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(SubitemType))]
    public class SubitemTypeModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "名称")]
        [Required(ErrorMessage ="名称不能为空")]
        public virtual string Name { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

    }
}
