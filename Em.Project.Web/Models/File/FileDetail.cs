using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Easyman.Web.Models
{
    /// <summary>
    /// 文件明细
    /// </summary>
    public class FileDetail
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public virtual string id { get; set; }

        /// <summary>
        /// 文件夹及文件ID
        /// </summary>
        public virtual long? FolderVersionId { get; set; }

        /// <summary>
        /// 所属终端ID
        /// </summary>
        public virtual long? ComputerId { get; set; }

        /// <summary>
        /// 所属终端ID
        /// </summary>
        public virtual long? FolderId { get; set; }

        /// <summary>
        /// 上个依赖ID
        /// </summary>
        public virtual long? RelyMonitFileId { get; set; }

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

        #region 其他字段
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public virtual bool? IsFolder { get; set; }

        /// <summary>
        /// 文件格式类型名
        /// </summary>
        public virtual string FormatName { get; set; }
        /// <summary>
        /// 文件图标
        /// </summary>
        public virtual string FileIcon { get; set; }
        /// <summary>
        /// 文件库名
        /// </summary>
        public virtual string FileMd5Name { get; set; }
        #endregion
    }
}
