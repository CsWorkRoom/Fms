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
    [AutoMap(typeof(FileFormat))]
    public class FileFormatModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public virtual bool? IsFolder { get; set; }

        /// <summary>
        /// 文件格式类型名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 文件图标
        /// </summary>
        public virtual string Icon { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        public virtual short? IsHide { get; set; }
    }
}
