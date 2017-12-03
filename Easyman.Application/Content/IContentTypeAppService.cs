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
    /// 内容类型管理
    /// </summary>
    public interface IContentTypeAppService : IApplicationService
    {
                
        /// <summary>
        /// 内容类别查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ContentTypeSearchOutput SearchContentType(ContentTypeSearchInput input);

     

        /// <summary>
        /// 内容类别新增和修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrInserContentType(ContentTypeInput input);

        

        /// <summary>
        /// 根据内容定义Id 获取内容类别树
        /// </summary>
        /// <returns></returns>
        object GetContentTypeParentTreeJson(int id, int conntentTypeId);


        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContentTypeInput GetContentType(long id);     
        /// <summary>
        /// 删除内容类别
        /// </summary>
        /// <param name="input"></param>
        void DelContentType(EntityDto<long> input);
        
    }
}
