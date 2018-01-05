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
    [AutoMap(typeof(FileAttr))]
    public class FileAttrModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 文件库ID
        /// </summary>
        public virtual long? FileLibraryId { get; set; }

        /// <summary>
        /// 文件库ID
        /// </summary>
        public virtual long? AttrId { get; set; }

        /// <summary>
        /// 文件属性值
        /// </summary>
        public virtual string AttrValue { get; set; }
    }
}
