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
    /// 正则表达式
    /// </summary>
    public class RegularAppService : ApplicationService, IRegularAppService
    {
        #region 初始化

        private readonly IRepository<Regulars, long> _regularRepository;

        public RegularAppService(IRepository<Regulars, long> regularRepository)
        {
            _regularRepository = regularRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public RegularSearchOutput GetAll(RegularSearchInput input)
        {
            int reordCount;
            var dataList = _regularRepository.GetAll()
                .SearchByInputDto(input, out reordCount)
                .MapTo<List<RegularOutput>>();
            input.Page.TotalCount = reordCount;
            return new RegularSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(RegularInput input)
        {
            var data = input.MapTo<Regulars>();
            _regularRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _regularRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                .ToList();
            return dataList;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public RegularInput Get(long id)
        {
            var data = _regularRepository.Get(id);
            return data.MapTo<RegularInput>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体Id</param>
        public void Del(EntityDto<long> input)
        {
            _regularRepository.Delete(a => a.Id == input.Id);
        }
    }
}
