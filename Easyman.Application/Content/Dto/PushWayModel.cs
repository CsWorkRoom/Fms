using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Common;
using Easyman.Domain;
using EasyMan.Dtos;
using Abp.Domain.Entities;

namespace Easyman.Content.Dto
{
   
    [AutoMapFrom(typeof(PushWay))]
    public class PushWayInput : EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 推送模式名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "推送模式名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "推送模式名称")]
        public virtual string Name { get; set; }
       
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AutoMapFrom(typeof(PushWay))]
    public class PushWayOutput : EntityDto<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 推送模式名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "推送模式名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "推送模式名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PushWaySearchInput : SearchInputDto<long> { }

    public class PushWaySearchOutput : SearchOutputDto<PushWayOutput,long>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<PushWayOutput> Datas
        {
            get;
            set;
        }
    }


}
