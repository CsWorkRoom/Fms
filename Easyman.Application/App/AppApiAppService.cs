using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.Authorization;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Common.Helper;
using Easyman.Content;
using Newtonsoft.Json;

namespace Easyman.App
{
    /// <summary>
    /// APP统一调用服务
    /// </summary>
    public class AppApiAppService : EasymanAppServiceBase, IAppApiAppService
    {
        private readonly IFileAppService _fileAppService;
        private readonly IUserInfoAppService _userInfoAppService;
        private readonly IAppCommonAppService _appCommonAppService;
        private readonly IContentAppService _contentAppService;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="fileAppService"></param>
        /// <param name="userInfoAppService"></param>
        /// <param name="appCommonAppService"></param>
        /// <param name="contentAppService"></param>
        public AppApiAppService(IFileAppService fileAppService, 
            IUserInfoAppService userInfoAppService,
            IAppCommonAppService appCommonAppService,
            IContentAppService contentAppService)
        {
            _fileAppService = fileAppService;
            _userInfoAppService = userInfoAppService;
            _appCommonAppService = appCommonAppService;
            _contentAppService = contentAppService;
        }

        #region APP通用功能

        /// <summary>
        /// 登录时获取用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string GetUserLoginInfo(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _userInfoAppService.GetUserLoginInfo(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string CheckUpdate(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _appCommonAppService.CheckUpdate(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public string FileUp()
        {
            var result = _fileAppService.FileUp();

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string FileDel(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _fileAppService.FileDel(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        #endregion

        #region 用户信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string UserSingle(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _userInfoAppService.UserSingle(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string UserSave(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestSaveEntityBean<ApiUserBean>>(decrptStr);

            var result = _userInfoAppService.UserSave(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string UserEditPwd(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestSaveEntityBean<ApiKeyValueBean>>(decrptStr);

            var result = _userInfoAppService.UserEditPwd(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        #endregion

        #region 内容管理

        /// <summary>
        /// 获取某定义下的所有内容类型
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentGetAllType(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _contentAppService.GetAllContentType(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 获取某定义下所有内容
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentList(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestPageBean>(decrptStr);

            var result = _contentAppService.GetAllContent(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentSingle(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _contentAppService.GetContentDetail(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentReviewList(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestPageBean>(decrptStr);

            var result = _contentAppService.GetContentReviewList(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 分页获取评论的回复
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentReviewCommentList(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestPageBean>(decrptStr);

            var result = _contentAppService.GetPageReviewComment(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 保存内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentReviewSave(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestSaveEntityBean<ApiContentReviewBean>>(decrptStr);

            var result = _contentAppService.SaveContentReview(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 删除内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ContentReviewDelete(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _contentAppService.DeleteContentReview(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 点赞内容
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string LikeContent(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _contentAppService.LikeContent(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 点赞内容评论
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string LikeContentReview(ApiEncryptedRequestBean requestObject)
        {
            var decrptStr = EncryptHelper.AesDecrpt(requestObject.reqData);
            var request = JsonConvert.DeserializeObject<ApiRequestEntityBean>(decrptStr);

            var result = _contentAppService.LikeContentReview(request);

            return EncryptHelper.AesEncrypt(JsonConvert.SerializeObject(result));
        }

        #endregion

    }
}
