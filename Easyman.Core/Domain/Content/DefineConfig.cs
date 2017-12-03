using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容定义配置
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "DEFINE_CONFIG")]
    public  class DefineConfig : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 内容定义ID
        /// </summary>
        [Column("DEFINE_ID")]
        public virtual long? DefineId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        [ForeignKey("DefineId")]
        public virtual Define Define { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        [Column("NAME"), StringLength(150)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        [Column("IS_CONTENT_FILE")]
        public virtual bool? IsContentFile { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        [Column("IS_REPLY")]
        public virtual bool? IsReoly { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        [Column("IS_REPLY_FILE")]
        public virtual bool? IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        [Column("IS_REPLY_FLOOR")]
        public virtual bool? IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        [Column("IS_REPLY_FLOOR_FILE")]
        public virtual bool? IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        [Column("IS_TEXT")]
        public virtual bool? IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        [Column("IS_LIKE")]
        public virtual bool? IsLike { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        [Column("IS_DELETE")]
        public virtual bool? IsDelete { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        [Column("IS_SHARE")]
        public virtual bool? IsShare { get; set; }

        /// <summary>
        /// 是否可选择用户发送
        /// </summary>
        [Column("IS_CHECK_USER")]
        public virtual bool? IsChenkUser { get; set; }
        /// <summary>
        /// 是否可选择角色发送
        /// </summary>
        [Column("IS_CHECK_ROLE")]
        public virtual bool? IsChenkRole { get; set; }
        /// <summary>
        /// 是否可选择组织发送
        /// </summary>
        [Column("IS_CHECK_DISTRICT")]
        public virtual bool? IsChenkDistrict { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime? CreateTime { get; set; }
        
    }
}
