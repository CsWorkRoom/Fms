using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    /// <summary>
    /// 数据库标识管理
    /// </summary>
    public interface IDbTagAppService : IApplicationService
    {
        /// <summary>
        /// 获取数据库标识集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DbTagSearchOutput GetDbTagSearch(DbTagSearchInput input);

        /// <summary>
        /// 根据ID获取某个数据库标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DbTagOutput GetDbTag(long id);

        /// <summary>
        /// 更新和新增数据库标识
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DbTagOutput InsertOrUpdateDbTag(DbTagInput input);

        /// <summary>
        ///  删除一条数据库标识
        /// </summary>
        /// <param name="input"></param>
        void DeleteDbTag(EntityDto<long> input);
        /// <summary>
        /// 获取数据库标识json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetDbTagTreeJson();
    }
}
