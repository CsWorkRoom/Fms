using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Authorization.Roles;
using EasyMan.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(Role))]
    public class RoleInput : EntityDto
    {
        public new int Id { get; set; }

        public int? ParentId { get; set; }

        public int? TenantId { get; set; }

        [Display(Name="角色名")]
        [Required(ErrorMessage ="角色名称不能为空")]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string NavIds { get; set; }

        public string ParentNavIds { get; set; }

        public string ChildNavIds { get; set; }

        public string UserIds { get; set; }

        public string FunIds { get; set; }

    }

    [AutoMapFrom(typeof(Role))]
    public class RoleOutput : EntityDto
    {
        public new int Id { get; set; }
        public int? ParentId { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string ParentDisplayName { get; set; }
        public string DisplayName { get; set; }
        public string NavIds { get; set; }
        public DateTime CreationTime { get; set; }
        public string ParentNavIds { get; set; }
        public string ChildNavIds { get; set; }
    }

    public class RoleSearchInput : SearchInputDto
    {
    }

    public class RoleSearchOutput : SearchOutputDto<RoleOutput>
    {
    }
}
