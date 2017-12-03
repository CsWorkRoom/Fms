using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using System.Data;
using EasyMan.Dtos;
using EasyMan.Common.Data;

namespace Easyman.Service
{
    /// <summary>
    /// 数据库管理 + 基于数据库的sqlhelper
    /// </summary>
    public interface IDbServerAppService : IApplicationService
    {
        #region 对dbserver的基础操作
        /// <summary>
        /// 获取数据库集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DbServerSearchOutput GetDbServerSearch(DbServerSearchInput input);
        /// <summary>
        /// 根据ID获取某个数据库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DbServerOutput GetDbServer(long id);
        /// <summary>
        /// 更新和新增数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DbServerOutput InsertOrUpdateDbServer(DbServerInput input);
        /// <summary>
        /// 删除一条数据库
        /// </summary>
        /// <param name="input"></param>
        void DeleteDbServer(EntityDto<long> input);
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetDbServerTreeJson();
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetDropDownList();
        #endregion

        #region 根据dbserver实现对数据库的操作
        /// <summary>
        /// 根据数据库ID获取链接
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        IDbSession GetSession(long dbId);
        /// <summary>
        ///  根据数据库信息获取链接
        /// </summary>
        /// <param name="dbserver"></param>
        /// <returns></returns>
        IDbSession GetSession(DbServerOutput dbserver);
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbserver"></param>
        /// <returns></returns>
        string GetConnectStr(DbServerOutput dbserver);
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        string GetConnectStr(long dbId);

        #region sqlhelper执行方法块
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        DataTable ExecuteGetTable(long dbId, string sql, ref ErrorInfo err);
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable ExecuteGetTable(long dbId, string sql);
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable ExecuteGetTable(DbServerOutput dbserver, string sql);
        /// <summary>
        /// 传入sql及分页信息得到一个DataTable
        /// </summary>
        /// <param name="dbServer"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        DataTable ExecuteGetTable(DbServerOutput dbServer, string sql, int pageIndex, int pageSize);
        /// <summary>
        /// 传入sql及分页信息得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        DataTable ExecuteGetTable(long dbId, string sql, int pageIndex, int pageSize);
        /// <summary>
        /// 获得DataReader
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(long dbId, string sql);
        /// <summary>
        /// 获得DataReader
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IDataReader ExecuteDataReader(DbServerOutput dbserver, string sql);
        /// <summary>
        /// 执行sql返回一个值
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        object ExecuteScalar(long dbId, string sql, ref ErrorInfo err);
        /// <summary>
        /// 执行sql返回一个值
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        object ExecuteScalar(DbServerOutput dbserver, string sql);
        /// <summary>
        /// 执行sql，根据dbserver
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        int Execute(DbServerOutput dbserver, string sql);
        /// <summary>
        /// 根据数据库dbId执行sql
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        int Execute(long dbId, string sql);
        /// <summary>
        /// 根据数据库dbID执行sql插入dataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="strDataTable"></param>
        /// <returns></returns>
        bool Execute(long dbId, string sql, string strDataTable);

        #endregion

        #endregion
    }
}
