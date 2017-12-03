using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;

namespace Easyman.Import
{
    /// <summary>
    /// 内置字段
    /// </summary>
    public interface IDefaultFieldAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        DefaultFieldSearchOutput GetAll(DefaultFieldSearchInput input);
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(DefaultFieldInput input);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        DefaultFieldInput Get(long id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        void Del(EntityDto<long> input);
        /// <summary>
        /// 默认字段
        /// </summary>
        /// <param name="id">数据类型Id</param>
        /// <returns></returns>
        object GetJsonObject(long id);
    }
}
