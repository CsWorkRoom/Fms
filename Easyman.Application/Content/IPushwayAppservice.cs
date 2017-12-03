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
    /// 推送管理
    /// </summary>
    public interface IPushwayAppService : IApplicationService
    {

        #region 推送模式
        /// <summary>
        /// 新增和修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrInserPushway(PushWayInput input);


        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PushWayInput GetPushway(long id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        void DelPushway(EntityDto<long> input);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PushWaySearchOutput SearchPushWay(PushWaySearchInput input);


        /// <summary>
        /// 推送类型树
        /// </summary>
        /// <returns></returns>
        List<PushWay> GetPushWay();

        /// <summary>
        /// 推送类型是否被选择
        /// </summary>
        /// <returns></returns>
        bool IsPushWay(int id, int contentId);
        #endregion
    }
}
