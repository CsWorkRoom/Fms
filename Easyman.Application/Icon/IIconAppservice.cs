using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Easyman.Sys
{
    public interface IIconAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有图标
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAllIcons();
        /// <summary>
        /// 获取所有图标,类型区分
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAllIconsId();
        /// <summary>
        /// 获取所有图标类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAllTypeIcons();

        /// <summary>
        /// 根据类型ID或者该类型下所有图标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<object> GetTypeIcons(long id);

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        IconSearchOutput GetAll(IconSearchInput input);
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(IconInput input);
        /// <summary>
        /// 获取单个数据模型
        /// </summary>
        /// <param name="id">需要修改的Id</param>
        /// <returns></returns>
        IconInput Get(long id);
        /// <summary>
        /// 删除指定的项
        /// </summary>
        /// <param name="input">需要删除的Id</param>
        void Del(EntityDto<long> input);

        /// <summary>
        /// 得到下拉类型的内容定义
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetIconType();

        #region 图标类型
        /// <summary>
        /// 类别新增和修改
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrInserIconType(IconTypeInput input);


        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IconTypeInput GetIconType(long id);

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="input"></param>
        void DelIconType(EntityDto<long> input);

        /// <summary>
        /// 类别查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IconTypeSearchOutput SearchIconType(IconTypeSearchInput input);


        /// <summary>
        /// 根据图标定义Id 获取类别树
        /// </summary>
        /// <returns></returns>
       // object GetIconTypeParentTreeJson(int id, int conntentTypeId);

        #endregion
    }
}
