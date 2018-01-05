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
    public class MonitFileTemp
    {
        public virtual string Id { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public virtual string ParentId { get; set; }
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

        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public virtual bool IsDir { get; set; }
        /// <summary>
        /// 文件格式名称
        /// </summary>
        public virtual string FormatName { get; set; }


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
        /// 是否发生变化
        /// </summary>
        public virtual bool IsChange { get; set; }


        public virtual double? Size { get; set; }

        public Dictionary<string, string> Properties { get; set; }

    }
}
