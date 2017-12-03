using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Base.Content.Dto
{
    /// <summary>
    /// 内容定义配置
    /// </summary>
    public class ContentConfigModel
    {
       
        public  int Id { get; set; }


        /// <summary>
        /// 内容定义ID
        /// </summary>
        public int ContentDefineId { get; set; }
        

        /// <summary>
        /// 名称
        /// </summary>
        public  string Name { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        [Column("IS_REPLY")]
        public  bool IsReoly { get; set; }

        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        [Column("IS_CONTENT_FILE")]
        public virtual bool IsContentFile { get; set; }

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

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public  DateTime? CreateTime { get; set; }
    }

    public class ContentModelNoRead
    {
        public long id { get; set; }
        /// <summary>
        /// 是否阅读0否1是
        /// </summary>
        public int is_user { get; set; }

        public string title { get; set; }

        public string type { get; set; }

        public DateTime? createTime { get; set; }
    }
}
