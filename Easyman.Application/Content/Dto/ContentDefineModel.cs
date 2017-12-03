using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Users;
using EasyMan.Dtos;

namespace Easyman.Content.Dto
{
    [AutoMapFrom(typeof(Define))]
    public class ContentDefineInput : CommonEntityHelper
    {
        public new int Id { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        [Display(Name = "内容定义名称")]
        [Required(ErrorMessage = "内容定义名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 内容定义编码
        /// </summary>
        [Display(Name = "内容定义编码")]
        [Required(ErrorMessage = "内容定义编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUId { get; set; }

        ///// <summary>
        ///// 建立主外键关系
        ///// </summary>
        //public User UserCreate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public long UpdateUId { get; set; }

        ///// <summary>
        ///// 建立主外键关系
        ///// </summary>
        //public User UserUpdate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        [Column("IS_REPLY")]
        public bool IsReoly { get; set; }
        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        [Column("IS_CONTENT_FILE")]
        public  bool IsContentFile { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        [Column("IS_REPLY_FILE")]
        public bool IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        [Column("IS_REPLY_FLOOR")]
        public bool IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        [Column("IS_REPLY_FLOOR_FILE")]
        public bool IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        [Column("IS_TEXT")]
        public bool IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        [Column("IS_LIKE")]
        public bool IsLike { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        [Column("IS_DELETE")]
        public  bool IsDelete { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        [Column("IS_SHARE")]
        public bool IsShare { get; set; }


        /// <summary>
        /// 是否可选择用户发送
        /// </summary>
        [Column("IS_CHECK_USER")]
        public  bool IsChenkUser { get; set; }
        /// <summary>
        /// 是否可选择角色发送
        /// </summary>
        [Column("IS_CHECK_ROLE")]
        public  bool IsChenkRole { get; set; }
        /// <summary>
        /// 是否可选择组织发送
        /// </summary>
        [Column("IS_CHECK_DISTRICT")]
        public  bool IsChenkDistrict { get; set; }

    }

    [AutoMapFrom(typeof(Define))]
    public class ContentDefineOutput : EntityDto
    {
        public new int Id { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 内容定义编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUId { get; set; }


        /// <summary>
        /// 更新人
        /// </summary>
        public string  UpdateUId { get; set; }

        ///// <summary>
        ///// 建立主外键关系
        ///// </summary>
        //public User UserUpdate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        [Column("IS_REPLY")]
        public bool IsReoly { get; set; }
        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        [Column("IS_CONTENT_FILE")]
        public  bool IsContentFile { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        [Column("IS_REPLY_FILE")]
        public bool IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        [Column("IS_REPLY_FLOOR")]
        public bool IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        [Column("IS_REPLY_FLOOR_FILE")]
        public bool IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        [Column("IS_TEXT")]
        public bool IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        [Column("IS_LIKE")]
        public bool IsLike { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        [Column("IS_DELETE")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        [Column("IS_SHARE")]
        public bool IsShare { get; set; }
        /// <summary>
        /// 是否可选择用户发送
        /// </summary>
        [Column("IS_CHECK_USER")]
        public bool IsChenkUser { get; set; }
        /// <summary>
        /// 是否可选择角色发送
        /// </summary>
        [Column("IS_CHECK_ROLE")]
        public bool IsChenkRole { get; set; }
        /// <summary>
        /// 是否可选择组织发送
        /// </summary>
        [Column("IS_CHECK_DISTRICT")]
        public bool IsChenkDistrict { get; set; }


    }
    public class ContentDefineSearchInput : SearchInputDto
    {

    }

    public class ContentDefineSearchOutput : SearchOutputDto<ContentDefineOutput>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<ContentDefineOutput> Datas
        {
            get;
            set;
        }
    }
}
