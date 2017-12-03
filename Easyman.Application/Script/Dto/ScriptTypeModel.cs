using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{
    [AutoMap(typeof(ScriptType))]
    public class ScriptTypeInput : EntityDto<long>
    {
        public new long Id { get; set; }

        [Required(ErrorMessage ="脚本类型名称不能为空")]
        [Display(Name = "脚本类型名")]
        public virtual string Name { get; set; }
        
        [Display(Name ="备注")]
        public virtual string Remark { get; set; }
    }

    [AutoMap(typeof(ScriptType))]
    public class ScriptTypeOutput : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        public virtual string CreateTime
        {
            get { return this.CreationTime.ToString("yyyy-MM-dd"); }
        }
        public virtual DateTime CreationTime { get; set; }

        public virtual string CreatorUserName
        {
            get;set;
        }
        
    }

    public class ScriptTypeSearchOutput : SearchOutputDto<ScriptTypeOutput,long>{ }

    public class ScriptTypeSearchInput : SearchInputDto{ }
}
