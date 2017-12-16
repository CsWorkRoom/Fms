using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easyman.Service
{
    /// <summary>
    /// 全局变量管理
    /// </summary>
    public class GlobalVarAppService : EasymanAppServiceBase, IGlobalVarAppService
    {
        #region 初始化
        /// <summary>
        /// 全局变量仓储
        /// </summary>
        private readonly IRepository<GlobalVar, long> _globalVarRepository;

        /// <summary>
        /// 构造函数（注入仓储）
        /// </summary>
        /// <param name="globalVarRepository"></param>
        public GlobalVarAppService(IRepository<GlobalVar, long> globalVarRepository)
        {
            _globalVarRepository = globalVarRepository;
        }
        #endregion

        #region 公共接口
        /// <summary>
        /// 根据id返回一个全局变量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GlobalVarModel GetGlobalVar(long id)
        {
            var global = _globalVarRepository.Get(id);
            if (global != null)
            {
                return AutoMapper.Mapper.Map<GlobalVarModel>(global);
            }
            return null;
        }
        /// <summary>
        /// 根据name返回一个全局变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GlobalVarModel GetGlobalVar(string name)
        {
            var global = _globalVarRepository.FirstOrDefault(p => p.Name == name);
            if (global != null)
            {
                return AutoMapper.Mapper.Map<GlobalVarModel>(global);
            }
            return null;
        }
        /// <summary>
        /// 新增或修改一个全局变量
        /// </summary>
        /// <param name="gloVar"></param>
        public void SaveGlobalVar(GlobalVarModel gloVar)
        {
           // var global = AutoMapper.Mapper.Map<GlobalVar>(gloVar);
            try
            {
                var type = _globalVarRepository.GetAll().FirstOrDefault(x => x.Id == gloVar.Id) ?? new GlobalVar();
                type = Fun.ClassToCopy(gloVar, type, (new string[] { "Id" }).ToList());
                _globalVarRepository.InsertOrUpdate(type);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("更新失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 根据id删除一个全局变量
        /// </summary>
        /// <param name="input"></param>
        public void DeleteGlobalVar(EntityDto<long> input)
        {
            try
            {
                _globalVarRepository.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 根据name删除一个全局变量
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGlobalVar(string name)
        {
            var global = _globalVarRepository.FirstOrDefault(p => p.Name == name);
            if (global != null)
            {
                try
                {
                    _globalVarRepository.Delete(global);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException("删除全局变量【" + name + "】失败：" + ex.Message);
                }
            }
            else
            {
                throw new UserFriendlyException("未找到全局变量名【" + name + "】");
            }
        }
        /// <summary>
        /// 获取所有全局变量
        /// </summary>
        /// <returns></returns>
        public IList<GlobalVarModel> GetGlobalVarList()
        {
            var globalList= _globalVarRepository.GetAll();
            if(globalList!=null&&globalList.Count()>0)
            {
                return AutoMapper.Mapper.Map<List<GlobalVarModel>>(globalList);
            }
            return null;
        }
        #endregion
    }
}
