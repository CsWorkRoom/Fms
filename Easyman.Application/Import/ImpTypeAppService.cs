using Abp.Application.Services;
using System.Collections.Generic;
using System.Linq;
using Easyman.Dto;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Easyman.Domain;
using EasyMan;
using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Easyman.Import
{
    /// <summary>
    /// 外导表分类
    /// </summary>
    public class ImpTypeAppService : ApplicationService, IImpTypeAppService
    {

        #region 初始化

        private readonly IRepository<ImpType, long> _impTypeRepository;

        public ImpTypeAppService(IRepository<ImpType, long> impTypeRepository)
        {
            _impTypeRepository = impTypeRepository;
        }

        #endregion

        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(ImpTypeInput input)
        {
            var data = input.MapTo<ImpType>();
            _impTypeRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        public void Del(EntityDto<long> input)
        {
            _impTypeRepository.Delete(a => a.Id == input.Id);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public ImpTypeInput Get(long id)
        {
            var data = _impTypeRepository.Get(id);
            return data.MapTo<ImpTypeInput>();
        }
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public ImpTypeSearchOutput GetAll(ImpTypeSearchInput input)
        {
            int reordCount;
            var dataList = _impTypeRepository.GetAll()
                .SearchByInputDto(input, out reordCount)
                .MapTo<List<ImpTypeOutput>>();
            input.Page.TotalCount = reordCount;
            return new ImpTypeSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _impTypeRepository.GetAll()
              .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
              .ToList();
            return dataList;
        }
    }
}
