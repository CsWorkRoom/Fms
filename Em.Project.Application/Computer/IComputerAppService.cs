using System;
using System.Collections.Generic;
using Easyman.Dto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 终端类型管理
    /// </summary>
    public interface IComputerAppService : IApplicationService
    {

        /// <summary>
        /// 根据ID获取某个终端类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ComputerModel GetComputer(long id);
        /// <summary>
        /// 更新和新增终端类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ComputerModel InsertOrUpdateComputer(ComputerModel input);

        /// <summary>
        /// 删除一条终端类型
        /// </summary>
        /// <param name="input"></param>
        void DeleteComputer(EntityDto<long> input);
        /// <summary>
        /// 获取终端类型json
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetComputerTreeJson();
        /// <summary>
        /// 获取所有类型List
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> ComputerList();
    }
}
