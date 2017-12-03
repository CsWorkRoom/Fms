using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public class PreDataTypeAppService : ApplicationService, IPreDataTypeAppService
    {
        #region 初始化

        private readonly IRepository<PreDataType, long> _preDataTypeRepository;
        private readonly IRepository<DbType, long> _dbTypeRepository;

        public PreDataTypeAppService(IRepository<PreDataType, long> preDataTypeRepository, IRepository<DbType, long> dbTypeRepository)
        {
            _preDataTypeRepository = preDataTypeRepository;
            _dbTypeRepository = dbTypeRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public PreDataTypeSearchOutput GetAll(PreDataTypeSearchInput input)
        {
            int reordCount;
            var dataList = _preDataTypeRepository.GetAll()
                .Select(a => new PreDataTypeOutput()
                {
                    Id = a.Id, Name = a.Name, DataType = a.DataType,
                    Remark = a.Remark, DbTypeName = a.DbType.Name
                })
                .SearchByInputDto(input, out reordCount).ToList();
            input.Page.TotalCount = reordCount;
            return new PreDataTypeSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(PreDataTypeInput input)
        {
            var dbType = _dbTypeRepository.Get(input.DbTypeId);
            var dataType = input.MapTo<PreDataType>();
            dataType.DbType = dbType;
            _preDataTypeRepository.InsertOrUpdate(dataType);
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _preDataTypeRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.DataType, Value = x.DataType })
                .ToList();
            return dataList;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public PreDataTypeInput Get(long id)
        {
            var data = _preDataTypeRepository.Get(id);
            return data.MapTo<PreDataTypeInput>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        public void Del(EntityDto<long> input)
        {
            _preDataTypeRepository.Delete(a => a.Id == input.Id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbTypeId"></param>
        /// <returns></returns>
        public IEnumerable<object> GetObjectJson(long dbTypeId)
        {
            return _preDataTypeRepository.GetAllList(a=>a.DbTypeId == dbTypeId)
                .Select(a => new { id = a.DataType, name = a.DataType })
                .ToList();
        }
    }
}
