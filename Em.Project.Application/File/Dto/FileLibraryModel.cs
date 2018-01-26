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
    [AutoMap(typeof(FileLibrary))]
    public class FileLibraryModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 文件类型ID
        /// </summary>
        public virtual long? FileFormatId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>
        public virtual string MD5 { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual double? Size { get; set; }

        /// <summary>
        /// 是否拷贝（1=拷贝、0=未拷贝）
        /// </summary>
        public virtual bool? IsCopy { get; set; }

        /// <summary>
        /// 文件备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否拷贝（1=拷贝、0=未拷贝）
        /// </summary>
        public virtual bool? IsHide { get; set; }

    }
}
