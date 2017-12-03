using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 外导表分类
    /// </summary>
    public interface IImpTypeAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        ImpTypeSearchOutput GetAll(ImpTypeSearchInput input);
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(ImpTypeInput input);
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetDropDownList();
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        ImpTypeInput Get(long id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        void Del(EntityDto<long> input);
    }
}
