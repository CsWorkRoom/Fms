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
    [AutoMap(typeof(ComputerFolder))]
    public class ComputerShareFolderModel : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name = "主路径名称")]
        [Required(ErrorMessage = "主路径不能为空")]
        public virtual string Name { get; set; }

        [Display(Name= "所属终端")]
        [Required(ErrorMessage = "所属终端不能为空")]
        public virtual long? ComputerId { get; set; }

        [Display(Name = "权限信息")]
        public virtual string PowerMsg { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

        public List<SelectListItem> ComputerList { get; set; }


    }
}
