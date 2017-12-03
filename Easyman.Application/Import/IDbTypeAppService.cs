using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Domain;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public interface IDbTypeAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        DbTypeSearchOutput GetAll(DbTypeSearchInput input);
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        void AddOrUpdate(DbTypeInput input);
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
        DbTypeInput Get(long id);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        DbTypeInput Get(Expression<Func<DbType, bool>> predicate);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        void Del(EntityDto<long> input);
    }
}
