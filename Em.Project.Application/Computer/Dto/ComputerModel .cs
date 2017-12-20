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
    [AutoMap(typeof(Computer))]
    public class ComputerModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "终端名称")]
        [Required(ErrorMessage ="终端不能为空")]
        public virtual string Name { get; set; }

        [Display(Name = "终端编号")]
        [Required(ErrorMessage = "终端编号不能为空")]
        public virtual string Code { get; set; }

        [Display(Name = "终端IP")]
        [Required(ErrorMessage = "终端IP不能为空")]
        public virtual string Ip { get; set; }

        [Display(Name = "登陆账号")]
        [Required(ErrorMessage = "登陆账号不能为空")]
        public virtual string UserName { get; set; }


        [Display(Name = "登陆密码")]
        [Required(ErrorMessage = "登陆密码不能为空")]
        public virtual string Pwd { get; set; }


        public virtual long? ComputerTypeId { get; set; }


        public virtual long? DistrictId { get; set; }

        public virtual bool IsUse { get; set; }


        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

        public List<SelectListItem> ComputerTypeList { get; set; }

    }
}
