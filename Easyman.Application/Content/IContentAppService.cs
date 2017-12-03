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
    /// 内容管理
    /// </summary>
    public interface IContentAppService : IApplicationService
    {
        
        /// <summary>
        /// 内容查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ContentIndexSearchOutput SearchContent(ContentIndexSearchInput input);

        /// <summary>
        /// 内容查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ContentIndexSearchOutput NewSearchContent(ContentIndexSearchInput input);
        /// <summary>
        /// 查询一条最新内容
        /// </summary>
        /// <returns></returns>
        string FirstSearchContent();

        /// <summary>
        /// 根据文件ID查询文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileTemp GetContentsFile(long id);

        /// <summary>
        /// 根据文件ID集合查询文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<object> GetContentsFileIds(string id);

        /// <summary>
        /// 获取未读内容，最新内容
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetNewContents();

        /// <summary>
        /// 根据传入的功能定义CODE获取未读内容，最新内容
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetDefineNewContents(string codes);
        /// <summary>
        /// 判断是否有阅读权限
        /// </summary>
        /// <param name="id">内容ID</param>
        /// <returns></returns>
        bool GetIsAllow(long id);

        /// <summary>
        /// 内容新增和修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrInserContentIndex(ContentIndexInput input);
        

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="input"></param>
        void DelContentIndex(EntityDto<long> input);

        /// <summary>
        /// 根据ID查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContentIndexInput GetContent(long id);


        /// <summary>
        /// 根据角色Id查询用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        object GetUserByRoleId(int roleId);

        /// <summary>
        /// 根据用户ID查询用户
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        object GetUserByUserId(int uId);

        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        object GetUserByName(string uName); 

        /// <summary>
        /// 根据用户名模糊查询
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        object GetUserByNameList(string uName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<ContentRole> GetAllContentRole();

        /// <summary>
        /// 指定组织树获取
        /// </summary>
        /// <returns></returns>
        object GetDistrictParentTreeJson(long cid);
        /// <summary>
        /// 限制组织树获取
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        object GetDistrictParentTreeJsonNo(long cid);

        #region APP

        /// <summary>
        /// 获取某定义下的所有内容类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<ApiKeyValueBean> GetAllContentType(ApiRequestEntityBean request);

        /// <summary>
        /// 获取某定义下所有内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiPagingDataBean<ApiContentBean> GetAllContent(ApiRequestPageBean request);

        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiContentBean GetContentDetail(ApiRequestEntityBean request);

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiPagingDataBean<ApiContentReviewBean> GetContentReviewList(ApiRequestPageBean request);

        /// <summary>
        /// 分页获取评论的回复
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiPagingDataBean<ApiContentReviewBean> GetPageReviewComment(ApiRequestPageBean request);

        /// <summary>
        /// 保存内容评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiContentReviewBean SaveContentReview(ApiRequestSaveEntityBean<ApiContentReviewBean> request);

        /// <summary>
        /// 删除内容评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiErrorBean DeleteContentReview(ApiRequestEntityBean request);

        /// <summary>
        /// 点赞内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiErrorBean LikeContent(ApiRequestEntityBean request);

        /// <summary>
        /// 点赞评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiErrorBean LikeContentReview(ApiRequestEntityBean request);

        #endregion

        
    }
}
