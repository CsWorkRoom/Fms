using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 属性类型管理
    /// </summary>
    public interface IAttrTypeAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个属性类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AttrTypeModel GetAttrType(long id);
        /// <summary>
        /// 更新和新增属性类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        AttrTypeModel InsertOrUpdateAttrType(AttrTypeModel input);

        /// <summary>
        /// 删除一条属性类型
        /// </summary>
        /// <param name="input"></param>
        //[System.Web.Http.HttpPost]
        void DeleteAttrType(EntityDto<long> input);
        /// <summary>
        /// 获取属性类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAttrTypeTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> AttrTypeList();
    }
}
