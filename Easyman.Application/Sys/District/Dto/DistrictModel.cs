using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(District))]
    public class DistrictInput : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public virtual int? TenantId { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 部门分类
        /// </summary>
        public virtual string ObjectType { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>
        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "部门名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 部门代码
        /// </summary>
        [Display(Name = "部门代码")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool? IsUse { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public virtual int? CurLevel { get; set; }
        /// <summary>
        /// ID层级结构
        /// </summary>
        public virtual string IdPath { get; set; }
        /// <summary>
        /// NAME层级结构
        /// </summary>
        public virtual string NamePath { get; set; }
        /// <summary>
        /// 部门联系人
        /// </summary>
        public virtual string LinkMan { get; set; }
        /// <summary>
        /// 部门联系人电话
        /// </summary>
        public virtual string LinkPhone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    [AutoMap(typeof(District))]
    public class DistrictOutput:EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public virtual int? TenantId { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 部门分类
        /// </summary>
        public virtual string ObjectType { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>
        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "部门名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 部门代码
        /// </summary>
        [Display(Name = "部门代码")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool? IsUse { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public virtual int? CurLevel { get; set; }
        /// <summary>
        /// ID层级结构
        /// </summary>
        public virtual string IdPath { get; set; }
        /// <summary>
        /// NAME层级结构
        /// </summary>
        public virtual string NamePath { get; set; }
        /// <summary>
        /// 部门联系人
        /// </summary>
        public virtual string LinkMan { get; set; }
        /// <summary>
        /// 部门联系人电话
        /// </summary>
        public virtual string LinkPhone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    public class DistrictSearchInput : SearchInputDto<long>
    {
    }

    public class DistrictSearchOutput : SearchOutputDto<DistrictOutput,long>
    {
        public override Pager Page { get; set; }

        public override IEnumerable<DistrictOutput> Datas { get; set; }
    }
}
