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
    public class TreeNode
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public virtual string id { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public virtual string pId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public virtual string name { get; set; }

        /// <summary>
        /// 节点说明
        /// </summary>
        public virtual string title { get; set; }

        /// <summary>
        /// 节点类型：computer、folder、file
        /// </summary>
        public virtual string nodeType { get; set; }

        ///// <summary>
        ///// 图标
        ///// </summary>
        //public virtual string icon { get; set; }
        /// <summary>
        /// 自定义图标
        /// </summary>
        public virtual string iconSkin { get; set; }
        /// <summary>
        /// 是否默认展开
        /// </summary>
        public virtual bool open { get; set; }

        /// <summary>
        /// 是否父节点
        /// </summary>
        public virtual bool isParent { get; set; }

        /// <summary>
        /// 是否文件夹
        /// </summary>
        public virtual bool? isFolder { get; set; }

        /// <summary>
        /// 其他辅助信息
        /// </summary>
        public virtual string assist { get; set; }

        /// <summary>
        /// 文件格式图标
        /// </summary>
        public virtual string fileFormatIcon { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public virtual bool? isHide { get; set; }
    }
}
