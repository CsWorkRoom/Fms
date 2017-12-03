using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Easyman.Import
{
    /// <summary>
    /// 内置字段
    /// </summary>
    public class DefaultFieldAppService : ApplicationService, IDefaultFieldAppService
    {
        #region 初始化

        private readonly IRepository<DefaultField, long> _defaultFieldRepository;
        private readonly IRepository<DbType, long> _dbTypeRepository;

        public DefaultFieldAppService(IRepository<DefaultField, long> defaultFieldRepository, IRepository<DbType, long> dbTypeRepository)
        {
            _defaultFieldRepository = defaultFieldRepository;
            _dbTypeRepository = dbTypeRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public DefaultFieldSearchOutput GetAll(DefaultFieldSearchInput input)
        {
            int reordCount;
            var dataList = _defaultFieldRepository.GetAll()
                .SearchByInputDto(input, out reordCount)
                .MapTo<List<DefaultFieldOutput>>();
            foreach (var item in dataList)
            {
                item.DbTypeName = _dbTypeRepository.Get(item.DbTypeId) == null ? "" : _dbTypeRepository.Get(item.DbTypeId).Name;
            }
            input.Page.TotalCount = reordCount;
            return new DefaultFieldSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(DefaultFieldInput input)
        {
            var data = new DefaultField();
            var list = new List<DefaultField>();
            if (input.Id == 0)
            {
                list = _defaultFieldRepository
                    .GetAllList(a => (a.FieldName == input.FieldName
                    || a.FieldCode.ToLower() == input.FieldCode.ToLower()) && a.DbTypeId == input.DbTypeId);
                input.CreateTime = DateTime.Now;
                data = input.MapTo<DefaultField>();
            }
            else
            {
                list = _defaultFieldRepository
                     .GetAllList(a => (a.FieldName == input.FieldName
                     || a.FieldCode.ToLower() == input.FieldCode.ToLower()) && a.Id != input.Id && a.DbTypeId == input.DbTypeId);
                var item = _defaultFieldRepository.Get(input.Id);
                input.CreateTime = item.CreateTime;
                data = input.MapTo<DefaultFieldInput, DefaultField>(item);
            }
            if (list.Count > 0) "字段重复".ErrorMsg();
            if (!Regex.IsMatch(input.FieldCode, @"^[a-zA-Z][a-zA-Z_]*$"))
                "字段编码只能输入字母和下滑线的组合".ErrorMsg();
            _defaultFieldRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public DefaultFieldInput Get(long id)
        {
            var data = _defaultFieldRepository.Get(id);
            return data.MapTo<DefaultFieldInput>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        public void Del(EntityDto<long> input)
        {
            _defaultFieldRepository.Delete(a => a.Id == input.Id);
        }
        /// <summary>
        /// 默认字段
        /// </summary>
        /// <param name="id">数据类型Id</param>
        /// <returns></returns>
        public object GetJsonObject(long id)
        {
            var dataList = _defaultFieldRepository.GetAllList(a => a.DbTypeId == id)
                .Select(b => new { id = b.Id, name = b.FieldName, code = b.FieldCode })
                .ToList();
            return dataList;
        }
    }
}
