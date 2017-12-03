using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Easyman.Import
{
    public interface IImpTbFieldAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <param name="impTbId">外导表Id</param>
        /// <returns></returns>
        ImpTbFieldSearchOutput GetAll(ImpTbFieldSearchInput input, long impTbId);
        /// <summary>
        /// 新增或者删除
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(ImpTbFieldInput input);
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
        ImpTbFieldInput Get(long id);
        /// <summary>
        /// 删除指定的项
        /// </summary>
        /// <param name="input">需要删除的Id</param>
        void Del(EntityDto<long> input);
    }
}
