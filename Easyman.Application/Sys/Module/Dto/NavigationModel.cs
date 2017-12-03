using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(Module))]
    public class NavigationInput : EntityDto
    {
        public NavigationInput()
        {
            RoleIds = "";
            ParentId = null;
            TenantId = null;
        }

        public new int Id { get; set; }

        [Display(Name = "菜单名称")]
        [Required(ErrorMessage = "菜单名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "菜单代码")]
        [Required(ErrorMessage = "菜单代码不能为空")]
        public string Code { get; set; }

        [Required]
        public int? TenantId { get; set; }

        public int? ParentId { get; set; }

        public int? ShowOrder { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string UrlActionList { get; set; }

        public string RoleIds { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool? IsUse { get; set; }
    }

    [AutoMapFrom(typeof(Module))]
    public class NavigationOutput : EntityDto
    {
        public new int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 类型(APP/PCWEB)
        /// </summary>
        public virtual int? Type { get; set; }

        public int TenantId { get; set; }
        public string TenantName { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public DateTime CreationTime { get; set; }

        public int? ShowOrder { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool? IsUse { get; set; }
    }

    public class NavigationSerachInput : SearchInputDto
    {
    }

    public class NavigationSerachOutput : SearchOutputDto<NavigationOutput>
    {

    }
}
