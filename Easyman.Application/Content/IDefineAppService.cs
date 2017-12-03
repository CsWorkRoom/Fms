using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services;
using Easyman.App.Dto;
using Easyman.Base.Content.Dto;
using Easyman.Content.Dto;
using Easyman.Domain;
using Abp.Application.Services.Dto;

namespace Easyman.Content
{
    /// <summary>
    /// 功能定义管理
    /// </summary>
    public interface IDefineAppService : IApplicationService
    {
        /// <summary>
        /// 添加功能定义
        /// </summary>
        /// <param name="define"></param>
        /// <returns></returns>
        void AddContentDefine(Define define);

        /// <summary>
        /// 内容定义查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ContentDefineSearchOutput Search(ContentDefineSearchInput input);

       

        /// <summary>
        /// 内容定义新增和修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrInserContentDefine(ContentDefineInput input);

        /// <summary>
        /// 根据ID查询内容定义及配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContentDefineInput GetContentDefine(long id);

        /// <summary>
        /// 删除内容定义
        /// </summary>
        /// <param name="input"></param>
        void DelContentDefine(EntityDto<long> input);

       

        /// <summary>
        /// 内容定义树
        /// </summary>
        /// <returns></returns>
         Task<IEnumerable<object>> GetDefineTreeJson();

        /// <summary>
        /// 得到下拉类型的内容定义
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetDefine();

      
      
      

        
    }
}
