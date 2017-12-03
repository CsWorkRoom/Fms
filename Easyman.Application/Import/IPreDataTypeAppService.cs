using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public interface IPreDataTypeAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        PreDataTypeSearchOutput GetAll(PreDataTypeSearchInput input);
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(PreDataTypeInput input);
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
        PreDataTypeInput Get(long id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        void Del(EntityDto<long> input);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTypeId"></param>
        /// <returns></returns>
        IEnumerable<object> GetObjectJson(long dbTypeId);
    }
}
