using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 外导表管理
    /// </summary>
    public interface IImpTbAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        ImpTbSearchOutput GetAll(ImpTbSearchInput input);
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(ImpTbInput input);
        /// <summary>
        /// 获取所有下拉数据库类型列表
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetDropDownList();
        /// <summary>
        /// 获取单个数据模型
        /// </summary>
        /// <param name="id">需要修改的Id</param>
        /// <returns></returns>
        ImpTbInput Get(long id);
        /// <summary>
        /// 删除指定的项
        /// </summary>
        /// <param name="input">需要删除的Id</param>
        void Del(EntityDto<long> input);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetIds(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        long GetDbTypeId(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CreateSqlScript(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        byte[] GetbytesByCode(string code, out string name);
    }
}
