using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Service
{
    /// <summary>
    /// 数据库标识管理
    /// </summary>
    public class DbTagAppService : EasymanAppServiceBase, IDbTagAppService
    {
        #region 初始化

        private readonly DbTagManager _dbTagManager;
        /// <summary>
        /// 构造函数注入DbTag仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public DbTagAppService(DbTagManager dbTagManager )
        {
            _dbTagManager = dbTagManager;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取数据库标识集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DbTagSearchOutput GetDbTagSearch(DbTagSearchInput input)
        {
            var rowCount = 0;
            var tags = _dbTagManager.Query.SearchByInputDto(input, out rowCount);

            var outPut = new DbTagSearchOutput()
            {
                Datas=tags.ToList().Select(s => s.MapTo<DbTagOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        /// <summary>
        /// 根据ID获取某个数据库标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DbTagOutput GetDbTag(long id)
        {
            var _tag = _dbTagManager.GetDbTag(id);
            if (_tag != null)
            {
                return AutoMapper.Mapper.Map<DbTagOutput>(_tag);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的标识！");
        }

        /// <summary>
        /// 更新和新增数据库标识
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DbTagOutput InsertOrUpdateDbTag(DbTagInput input)
        {
            var dbTag = _dbTagManager.GetDbTag(input.Id) ?? new DbTag();
            dbTag.Name = input.Name;
            dbTag.Remark = input.Remark;

            var tag = _dbTagManager.SaveOrUpdateDbTag(dbTag);

            if (tag != null)
            {
                return AutoMapper.Mapper.Map<DbTagOutput>(tag);
            }
            else
            {
                throw new UserFriendlyException("更新失败！");
            }
        }

        /// <summary>
        /// 删除一条数据库标识
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDbTag(EntityDto<long> input)
        {
            try
            {
                _dbTagManager.DeleteDbTag(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
        /// <summary>
        /// 获取数据库标识json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDbTagTreeJson()
        {
            var builder = new StringBuilder();

            var tags = _dbTagManager.GetAllDbTag();

            var tagNodes = tags.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                iconSkin = "menu"
            }).ToList();

            return tagNodes;
        }
        #endregion

    }
}
