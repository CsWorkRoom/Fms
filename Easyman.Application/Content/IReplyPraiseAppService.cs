using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Easyman.App.Dto;
using Easyman.Base.Content.Dto;
using Easyman.Content.Dto;
using Easyman.Domain;
using Abp.Application.Services.Dto;

namespace Easyman.Content
{
    /// <summary>
    /// 评论和点赞管理
    /// </summary>
    public interface IReplyPraiseAppService : IApplicationService
    {
      
        /// <summary>
        /// 内容阅读日志
        /// </summary>
        void ContentReadLog(int contentId);

        /// <summary>
        /// 内容阅统计数量
        /// </summary>
        int ContentReadContent(int contentId);

        /// <summary>
        /// 评论次数
        /// </summary>
        int ReplyContent(int contentId);

        /// <summary>
        /// 内容点赞次数
        /// </summary>
        int ContentPraiseCount(int contentId);

        /// <summary>
        /// 评论点赞次数
        /// </summary>
        int ReplyPraiseCount(int replyId);

        /// <summary>
        /// 内容是否已经点赞
        /// </summary>
        bool IsOrLike(int contentId);

        /// <summary>
        /// 评论是否已经点赞
        /// </summary>
        bool IsReplyOrLike(int replyId);
        /// <summary>
        /// 根据内容ID查询评论，返回一定的条数
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        List<ReplyModel> GetContentReplyListId(int contentId);
        /// <summary>
        /// 根据内容Id查询评论
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        List<ReplyModel> GetContentReply(int contentId);

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="replyInfo"></param>
        /// <param name="fileIds"></param>
        /// <param name="replyId"></param>
        /// <returns></returns>
        object CerateReply(int contentId, string replyInfo, string fileIds, int? replyId);
        /// <summary>
        /// 保存评论上传文件,返回附件
        /// </summary>
        /// <param name="repid"></param>
        /// <param name="filesid"></param>
        List<FileTemp> ContentReplyFiles(long repid, string filesid);

        /// <summary>
        /// 根据评论ID查询对应附件
        /// </summary>
        /// <param name="repid"></param>
        /// <returns></returns>
        List<FileTemp> ReplyFilesList(long repid);
        /// <summary>
        /// 根据评论ID查询附件
        /// </summary>
        /// <param name="repid"></param>
        /// <returns></returns>
        List<FileTemp> ContentReplyFileList(string repid);
        /// <summary>
        /// 内容点赞
        /// </summary>
        /// <param name="contentId"></param>
        int ContentPraise(int contentId);

        /// <summary>
        /// 评论点赞
        /// </summary>
        /// <param name="replyId"></param>
        void ReplyPraise(int replyId);

        

        
    }
}
