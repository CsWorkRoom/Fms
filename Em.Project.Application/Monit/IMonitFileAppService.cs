using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using EasyMan.Dtos;

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

        /// <summary>
        /// 根据版本获取文件目录
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        List<MonitFileModel> GetMonitFileByVersion(long versionId);


        #region 扩展方法:监控文件(夹)的上载、下载、还原等功能

        #region 监控日志的写入
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="log">日志实例</param>
        void Log(MonitLogModel log);
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="caseVersionId"></param>
        /// <param name="mointFileId"></param>
        /// <param name="logType"></param>
        /// <param name="logMsg"></param>
        void Log(long? caseVersionId, long? mointFileId, short? logType, string logMsg);
        /// <summary>
        /// 插入一条监控日志(含日志批次ID)
        /// </summary>
        /// <param name="caseVersionId"></param>
        /// <param name="monitLogVersionId"></param>
        /// <param name="mointFileId"></param>
        /// <param name="logType"></param>
        /// <param name="logMsg"></param>
        void Log(long? caseVersionId, long? monitLogVersionId, long? mointFileId, short? logType, string logMsg);
        #endregion

        #region 上传监控文件到服务端+将服务端文件(夹)还原到客户端
        /// <summary>
        /// 监控并上传监控文件到服务器（单个文件的上传）
        /// </summary>
        /// <param name="monitFileId"></param>
        [System.Web.Http.HttpGet]
        string UpFileByMonitFile(long? monitFileId);
        /// <summary>
        /// 还原服务端的文件到客户端（含文件夹和文件两种形式还原处理）
        /// </summary>
        /// <param name="monitFileId"></param>
        [System.Web.Http.HttpGet]
        string RestoreFileByMonitFile(long? monitFileId);
        #endregion

        #region 客户在web端下载文件(夹)到本机
        /// <summary>
        /// 下载前的准备：生成待下载文件
        /// 1）文件夹下载：生成临时文件夹及子目录,压缩成zip包,删除生成临时文件夹 ->返回生成后的压缩文件名
        /// 2）文件下载：生成临时文件 ->返回生成后的临时文件名
        /// </summary>
        /// <param name="monitFileId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        string GenerateFile(long? monitFileId);
        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        [System.Web.Http.HttpGet]
        void DeleteFile(string fileName);

        /// <summary>
        /// 获取当前用户管辖共享文件夹的监控错误数
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        int GetErrorNumByUser();
        #endregion

    }
}
