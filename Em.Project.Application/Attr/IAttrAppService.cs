using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 属性类型管理
    /// </summary>
    public interface IAttrAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个属性类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AttrModel GetAttr(long id);
        /// <summary>
        /// 更新和新增属性类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        AttrModel InsertOrUpdateAttr(AttrModel input);

        /// <summary>
        /// 删除一条属性类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteAttr(EntityDto<long> input);
        /// <summary>
        /// 获取属性类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAttrTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> AttrList();
    }
}
