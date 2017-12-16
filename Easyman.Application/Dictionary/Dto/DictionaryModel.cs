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
    [AutoMap(typeof(Dictionary))]
    public class DictionaryModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public virtual long? DictionaryTypeId { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "字典名称不能为空")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 字典类型列表
        /// </summary>
        public List<SelectListItem> DictionaryTypeList { get; set; }

    }
}
