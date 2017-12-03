using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easyman.Domain;
using Easyman.Users;

namespace Easyman.Content.Dto
{
    public class ReplyModel
    {
       
        public  long Id { get; set; }
        
        public  long ContentId { get; set; }

        public  Domain.Content Content { get; set; }

      
        public  long? ParentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public  string Info { get; set; }

        /// <summary>
        /// 评论人
        /// </summary>     
        public  long ReolyUId { get; set; }


        public  User ReplyUser { get; set; }

        /// <summary>
        /// 评论人名字
        /// </summary>
        public string ReplyUserName { get; set; }

        /// <summary>
        /// 评论的上一个人名字
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public  DateTime CreationTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public  string IpAddr { get; set; }

        /// <summary>
        /// IP预计    
        /// </summary>
        public  string IpRomise { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public  bool? IsDelete { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public  DateTime DeleteTime { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public  long DeleteUid { get; set; }
        
        public  User DeleteUser { get; set; }

        /// <summary>
        /// 删除原因
        /// </summary>
        public  string DeleteReason { get; set; }

        public  List<ReplyModel> ChildContentReply { get; set; }

        /// <summary>
        /// 评论点赞次数
        /// </summary>
        public int ReplyPraiseCount { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        public bool IsReoly { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        public bool IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        public bool IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        public bool IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        public bool IsLike { get; set; }
        

        /// <summary>
        /// 允许转发
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// 评论是否已经点赞
        /// </summary>
        public bool IsReolyOrLike { get; set; }

        /// <summary>
        /// 评论上传附件数量
        /// </summary>
        public int IsFileNumber { get; set; }



    }
}
