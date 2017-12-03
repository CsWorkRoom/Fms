using Easyman.Dto;
using Abp.Domain.Repositories;
using Easyman.Domain;
using System.Linq;
using EasyMan;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using System;
using Abp.UI;

namespace Easyman.Service
{
    public class ScriptTypeAppService : EasymanAppServiceBase, IScriptTypeAppService
    {
        #region 初始化
        private readonly IRepository<ScriptType,long> _scriptType;

        public ScriptTypeAppService(IRepository<ScriptType,long> scriptType)
        {
            _scriptType = scriptType;
        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 获取全部(带分页)脚本类型
        /// </summary>
        /// <returns></returns>
        public ScriptTypeSearchOutput GetAllScriptType(ScriptTypeSearchInput input)
        {
            int reordCount;
            var ScriptType = _scriptType.GetAll().SearchByInputDto(input, out reordCount);
            var ScriptTypeRes = new ScriptTypeSearchOutput();
            //转换
            ScriptTypeRes.Datas = ScriptType.ToList().Select(x => x.MapTo<ScriptTypeOutput>());
            Pager page = new Pager();
            page = input.Page;
            page.TotalCount = reordCount;
            ScriptTypeRes.Page = page;

            return ScriptTypeRes;
        }

        /// <summary>
        /// 获取全部  用于页面下拉
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllScriptTypeList()
        {
            List<SelectListItem> DataList = new List<SelectListItem>();
            var ScriptTypeList = _scriptType.GetAllList().MapTo<List<ScriptTypeOutput>>();
            DataList.AddRange(ScriptTypeList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }));
            return DataList;
        }
        /// <summary>
        /// 新增/修改脚本类型
        /// </summary>
        /// <param name="input"></param>
        public void editScriptType(ScriptTypeInput input)
        {
            if (_scriptType.GetAll().Any(x => x.Name == input.Name && x.Id != input.Id))
            {
                throw new System.Exception("已有类型：" + input.Name);
            }

            var model = input.MapTo<ScriptType>();
            var res = _scriptType.InsertOrUpdate(model);
            if (res==null) {
                throw new System.Exception("保存失败！");
            }
            //model.Id = input.Id;
            //model.Name = input.Name;
            //model.Remark = input.Remark;
            //if (input.Id == 0)//新增
            //{
            //    _scriptType.Insert(model);
            //}
            //else//修改
            //{
            //    _scriptType.Update(model);
            //}
        }

        /// <summary>
        /// 删除一条脚本类型
        /// </summary>
        /// <param name="input"></param>
        public void DelScriptType(EntityDto<long> input)
        {
            try
            {
                _scriptType.Delete(x => x.Id == input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        /// <summary>
        /// 查询一条脚本类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ScriptTypeInput SingScriptType(long Id)
        {
            try
            {
                return _scriptType.Get(Id).MapTo<ScriptTypeInput>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }


        #endregion

        #region 私有方法

        #endregion
    }
}
