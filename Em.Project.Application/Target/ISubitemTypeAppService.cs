using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;
using System.Data;

namespace Easyman.Service
{
    /// <summary>
    /// 打分类型
    /// </summary>
    public interface ISubitemTypeAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubitemTypeModel GetSubitemType(long id);
        /// <summary>
        /// 更新和新增指标类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        SubitemTypeModel InsertOrUpdateSubitemType(SubitemTypeModel input);

        /// <summary>
        /// 删除一条指标类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteSubitemType(EntityDto<long> input);
        /// <summary>
        /// 获取指标类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetSubitemTypeTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> SubitemTypeList();


    }
}
