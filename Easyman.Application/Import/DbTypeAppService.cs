using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using EasyMan.Dtos;
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
    public class DbTypeAppService : ApplicationService, IDbTypeAppService
    {
        #region 初始化

        private readonly IRepository<DbType, long> _dbTypeRepository;

        public DbTypeAppService(IRepository<DbType, long> dbTypeRepository)
        {
            _dbTypeRepository = dbTypeRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public DbTypeSearchOutput GetAll(DbTypeSearchInput input)
        {
            int reordCount;
            var dataList = _dbTypeRepository.GetAll()
                .SearchByInputDto(input, out reordCount)
                .MapTo<List<DbTypeOutput>>();
            input.Page.TotalCount = reordCount;
            return new DbTypeSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(DbTypeInput input)
        {
            var data = input.MapTo<DbType>();
            _dbTypeRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _dbTypeRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                .ToList();
            return dataList;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public DbTypeInput Get(long id)
        {
            var data = _dbTypeRepository.Get(id);
            return data.MapTo<DbTypeInput>();
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DbTypeInput Get(Expression<Func<DbType, bool>> predicate)
        {
            var data = _dbTypeRepository.FirstOrDefault(predicate);
            return data.MapTo<DbTypeInput>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        public void Del(EntityDto<long> input)
        {
            _dbTypeRepository.Delete(a => a.Id == input.Id);
        }
    }
}
