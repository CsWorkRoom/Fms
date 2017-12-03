using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Easyman.App.Dto;

namespace Easyman.App
{
    /// <summary>
    /// APP统一调用接口
    /// </summary>
    public interface IAppApiAppService : IApplicationService
    {
        #region APP通用功能

        /// <summary>
        /// 登录时获取用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string GetUserLoginInfo(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string CheckUpdate(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        string FileUp();

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string FileDel(ApiEncryptedRequestBean requestObject);

        #endregion

        #region 用户信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string UserSingle(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string UserSave(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string UserEditPwd(ApiEncryptedRequestBean requestObject);

        #endregion

        #region 内容管理

        /// <summary>
        /// 获取某定义下的所有内容类型
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentGetAllType(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 获取某定义下所有内容
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentList(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentSingle(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentReviewList(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 分页获取评论的回复
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentReviewCommentList(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 保存内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentReviewSave(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 删除内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string ContentReviewDelete(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 点赞内容
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string LikeContent(ApiEncryptedRequestBean requestObject);

        /// <summary>
        /// 点赞内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        string LikeContentReview(ApiEncryptedRequestBean requestObject);

        #endregion

    }
}
