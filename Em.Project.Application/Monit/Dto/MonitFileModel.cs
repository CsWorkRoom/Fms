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
    [AutoMap(typeof(MonitFile))]
    public class MonitFileModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 文件夹及文件ID
        /// </summary>
        public virtual long? FolderVersionId { get; set; }     

        /// <summary>
        /// 所属终端ID
        /// </summary>
        public virtual long? ComputerId { get; set; }

        /// <summary>
        /// 所属共享文件夹ID
        /// </summary>
        public virtual long? FolderId { get; set; }

        /// <summary>
        /// 上个依赖ID
        /// </summary>
        public virtual long? RelyMonitFileId { get; set; }
        //[ForeignKey("RelyMonitFileId")]
        //public virtual MonitFile RelyMonitFile { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
     
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 文件格式ID
        /// </summary>
        public virtual long? FileFormatId { get; set; }

        /// <summary>
        /// 文件库ID
        /// </summary>
        public virtual long? FileLibraryId { get; set; }

        public virtual long? CaseVersionId { get; set; }
        public virtual short? CopyStatus { get; set; }

        /// <summary>
        /// 客户端路径
        /// </summary>
        public virtual string ClientPath { get; set; }
        /// <summary>
        /// 服务器路径
        /// </summary>
        public virtual string ServerPath { get; set; }
        /// <summary>
        /// MD5
        /// </summary>
        public virtual string MD5 { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual short? Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
