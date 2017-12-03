using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Easyman.Common;
using System.Text.RegularExpressions;
using Abp.Application.Services.Dto;

namespace Easyman.Import
{
    public class ImpTbFieldAppService : ApplicationService, IImpTbFieldAppService
    {
        #region 初始化

        private readonly IRepository<ImpTb, long> _impTbRepository;
        private readonly IRepository<ImpTbField, long> _impTbFieldRepository;
        private readonly IRepository<Regulars, long> _regularRepository;
        private readonly IRepository<PreDataType, long> _preDataTypeRepository;
        private readonly IRepository<DefaultField, long> _defaultFieldRepository;

        public ImpTbFieldAppService(IRepository<ImpTb, long> impTbRepository, IRepository<ImpTbField, long> impTbFieldRepository,
            IRepository<Regulars, long> regularRepository, IRepository<PreDataType, long> preDataTypeRepository,
            IRepository<DefaultField, long> defaultFieldRepository)
        {
            _impTbRepository = impTbRepository;
            _impTbFieldRepository = impTbFieldRepository;
            _regularRepository = regularRepository;
            _preDataTypeRepository = preDataTypeRepository;
            _defaultFieldRepository = defaultFieldRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <param name="impTbId">外导表Id</param>
        /// <returns></returns>
        public ImpTbFieldSearchOutput GetAll(ImpTbFieldSearchInput input, long impTbId)
        {
            int reordCount;
            var dataList = _impTbFieldRepository.GetAll().Where(a => a.ImpTbId == impTbId)
              .SearchByInputDto(input, out reordCount)
              .Select(a => new ImpTbFieldOutput
              {
                  Id = a.Id,
                  FieldCode = a.FieldCode,
                  FieldName = a.FieldName,
                  DataType = a.DataType,
                  Remark = a.Remark,
                  CreateTime = a.CreateTime,
                  RegularName = a.Regular.Name
              })
              .ToList();
            input.Page.TotalCount = reordCount;
            return new ImpTbFieldSearchOutput() { Datas = dataList, Page = input.Page };
        }
        /// <summary>
        /// 新增或者删除
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(ImpTbFieldInput input)
        {
            var data = new ImpTbField();
            var list = new List<ImpTbField>();
            if (input.Id == 0)
            {
                list = _impTbFieldRepository
                    .GetAllList(a => (a.FieldName == input.FieldName
                    || a.FieldCode.ToLower() == input.FieldCode.ToLower()) && a.ImpTbId == input.ImpTbId);
                input.CreateTime = DateTime.Now;
                data = input.MapTo<ImpTbField>();
            }
            else
            {
                list = _impTbFieldRepository
                     .GetAllList(a => (a.FieldName == input.FieldName
                     || a.FieldCode.ToLower() == input.FieldCode.ToLower()) && a.Id != input.Id && a.ImpTbId == input.ImpTbId);
                var item = _impTbFieldRepository.Get(input.Id);
                input.CreateTime = item.CreateTime;
                data = input.MapTo<ImpTbFieldInput, ImpTbField>(item);
            }
            var defaultFieldList = _defaultFieldRepository
                            .GetAllList(a => a.FieldName == input.FieldName
                            || a.FieldCode.ToLower() == input.FieldCode.ToLower());

            if (list.Count > 0 || defaultFieldList.Count > 0) "字段重复".ErrorMsg();
            if (!Regex.IsMatch(input.FieldCode, @"^[a-zA-Z][a-zA-Z_]*$"))
                "字段编码只能输入字母和下滑线的组合".ErrorMsg();
            if (input.RegularId != 0)
            {
                var regular = _regularRepository.Get(input.RegularId);
                data.Regular = regular;
            }
            _impTbFieldRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 获取所有下拉数据库类型列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _impTbRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.CnTableName, Value = x.Id.ToString() })
                .ToList();
            return dataList;
        }
        /// <summary>
        /// 获取单个数据模型
        /// </summary>
        /// <param name="id">需要修改的Id</param>
        /// <returns></returns>
        public ImpTbFieldInput Get(long id)
        {
            var item = _impTbFieldRepository.Get(id);
            var data = item.MapTo<ImpTbFieldInput>();
            return data;
        }
        /// <summary>
        /// 删除指定的项
        /// </summary>
        /// <param name="input">需要删除的Id</param>
        public void Del(EntityDto<long> input)
        {
            _impTbFieldRepository.Delete(a => a.Id == input.Id);
        }
    }
}
