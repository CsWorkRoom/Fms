using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easyman.Dto;
using Abp.Application.Services.Dto;

namespace Easyman.Service
{
    /// <summary>
    /// 全局变量管理
    /// </summary>
    public interface IGlobalVarAppService : IApplicationService
    {
        /// <summary>
        /// 根据id返回一个全局变量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GlobalVarModel GetGlobalVar(long id);
        /// <summary>
        /// 根据name返回一个全局变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GlobalVarModel GetGlobalVar(string name);
        /// <summary>
        /// 新增或修改一个全局变量
        /// </summary>
        /// <param name="gloVar"></param>
        void SaveGlobalVar(GlobalVarModel gloVar);
        /// <summary>
        /// 根据id删除一个全局变量
        /// </summary>
        /// <param name="input"></param>
        void DeleteGlobalVar(EntityDto<long> input);
        /// <summary>
        /// 根据name删除一个全局变量
        /// </summary>
        /// <param name="name"></param>
        void DeleteGlobalVar(string name);
        /// <summary>
        /// 获取所有全局变量
        /// </summary>
        /// <returns></returns>
        IList<GlobalVarModel> GetGlobalVarList();
    }
}
