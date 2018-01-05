using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 文件夹及文件管理管理
    /// </summary>
    public interface IMonitFileAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个文件夹及文件管理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MonitFileModel GetMonitFile(long id);
        /// <summary>
        /// 更新和新增文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        MonitFileModel InsertOrUpdateMonitFile(MonitFileModel input);

        /// <summary>
        /// 删除一条文件夹及文件管理
        /// </summary>
        /// <param name="input"></param>
        void DeleteMonitFile(EntityDto<long> input);
        /// <summary>
        /// 获取文件夹及文件管理json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetMonitFileTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> MonitFileList();

        /// <summary>
        /// 根据路径查询文件信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        MonitFileModel GetMonitFileByPath(string path);

        #region 扩展方法
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="log">日志实例</param>
        void Log(MonitLogModel log);

        //从客户端拷贝文件
        

        //从服务端还原文件
        #endregion
    }
}
