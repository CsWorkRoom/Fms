using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：评论
    /// </summary>
    public class ApiContentBean
    {
        public long ID { get; set; }

        public long DEFINE_TYPE_ID { get; set; }

        public string TITLE { get; set; }

        public string SUMMARY { get; set; }

        public string INFO { get; set; }

        public string IMAGE { get; set; }

        public DateTime BEGINTIME { get; set; }

        public DateTime ENDTIME { get; set; }

        public DateTime CREATETIME { get; set; }

        public DateTime UPDATETIME { get; set; }

        public string PUBLISHER { get; set; }

        public bool? IS_IMPORT { get; set; }

        public bool? IS_URGENT { get; set; }

        public int ReadCount { get; set; }

        public int ReviewCount { get; set; }

        public int ContentLikeCount { get; set; }

        public bool IsReplyAllowed { get; set; }

        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        public bool IsReplyFileAllowed { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        public bool IsReplyFloorAllowed { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        public bool IsReplyFloorFileAllowed { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        public bool IsTextAllowed { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        public bool IsLikeAllowed { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        public bool IsDeleteAllowed { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        public bool IsShareAllowed { get; set; }

        /// <summary>
        /// 是否已经点赞
        /// </summary>
        public bool IsOrLike { get; set; }

        /// <summary>
        /// 评论是否已经点赞
        /// </summary>
        public bool IsReplyOrLike { get; set; }

        /// <summary>
        /// 是否是选择的全部用户
        /// </summary>
        public bool IsAll { get; set; }
    }
}
