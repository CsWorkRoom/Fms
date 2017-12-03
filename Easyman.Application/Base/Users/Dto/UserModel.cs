using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Users;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserInput : EntityDto
    {
        public new int Id { get; set; }

        [Display(Name = "用户姓名")]
        [Required(ErrorMessage = "用户姓名不能为空")]
        public string Name { get; set; }

        [Display(Name = "用户工号")]
        [Required(ErrorMessage = "用户工号不能为空")]
        public string UserName { get; set; }

        public int? TenantId { get; set; }

        [Display(Name = "部门")]
        //[Required(ErrorMessage = "部门不能为空")]
        public int? DepartmentId { get; set; }

        //[Display(Name = "邮箱")]
        //[Required(ErrorMessage = "邮箱不能为空")]
        public string EmailAddress { get; set; }

        public string PhoneNo { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string RoleIds { get; set; }

        public string RoleNames { get; set; }

        [Display(Name = "组织")]
        //[Required(ErrorMessage = "组织不能为空")]
        public long? DistrictId { get; set; }
    }

    [AutoMapFrom(typeof(User))]
    public class UserOutput : EntityDto
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        public long? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DistrictGroupName { get; set; }
        public long? DistrictId { get; set; }

        public string State { get; set; }
    }

    public class UserSearchInput : SearchInputDto
    {

    }

    public class UserSearchOutput : SearchOutputDto<UserOutput>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<UserOutput> Datas
        {
            get;
            set;
        }
    }
}
