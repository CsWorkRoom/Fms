using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Common;
using Easyman.Domain;
using EasyMan.Dtos;
using System.Collections.Generic;

namespace Easyman.Dto
{
    public class IconTypeModel
    {
        public string Value { get; set; }

        public long Id { get; set; }
    }

    [AutoMapFrom(typeof(IconType))]
    public class IconTypeInput : CommonEntityHelper
    {
        /// <summary>
        /// ID
        /// </summary>
       // public new long Id { get; set; }
        public long Id { get; set; }
        /// <summary>
        /// 图标类型名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标类型名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "图标类型名称")]
        public virtual string Name { get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AutoMapFrom(typeof(IconType))]
    public class IconTypeOutput : EntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
       // public new long Id { get; set; }
        public long Id { get; set; }
        /// <summary>
        /// 图标类型名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标类型名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "图标类型名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class IconTypeSearchInput : SearchInputDto { }

    public class IconTypeSearchOutput : SearchOutputDto<IconTypeOutput>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<IconTypeOutput> Datas
        {
            get;
            set;
        }
    }


}
