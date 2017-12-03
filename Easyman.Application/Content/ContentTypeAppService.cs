using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.Base.Content.Dto;
using Easyman.Content.Dto;
using Easyman.Domain;
using Easyman.Users;
using EasyMan;
using EasyMan.Dtos;
using Newtonsoft.Json;
using Abp.Application.Services.Dto;

namespace Easyman.Content
{
    /// <summary>
    /// 内容类型管理
    /// </summary>
    public class ContentTypeAppService : EasymanAppServiceBase, IContentTypeAppService
    {
        #region 初始化
        private readonly IRepository<Define, long> _defineRepository;     
        private readonly IRepository<ContentType, long> _contentTypeRepository;
        private readonly IRepository<Domain.Content, long> _contentRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defineRepository"></param>
        /// <param name="contentTypeRepository"></param>
        /// <param name="contentRepository"></param>
        public ContentTypeAppService(IRepository<Define, long> defineRepository,IRepository<ContentType, long> contentTypeRepository ,
             IRepository<Domain.Content, long> contentRepository)
        {
            _defineRepository = defineRepository;
            _contentTypeRepository = contentTypeRepository;
            _contentRepository = contentRepository;
        }
        #endregion

        

        #region 内容类别
        /// <summary>
        /// 类容类别查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ContentTypeSearchOutput SearchContentType(ContentTypeSearchInput input)
        {
            int rowCount;
            var data = _contentTypeRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new ContentTypeSearchOutput
            {
                Datas = data.ToList().Select(s =>
                {
                    var temp = s.MapTo<ContentTypeOutput>();
                    temp.DefineName = _defineRepository.Get(s.DefineId).Name;
                    if (s.ParentId != 0)
                        temp.ParentName = _contentTypeRepository.Get(s.ParentId) == null ? "" : _contentTypeRepository.Get(s.ParentId).Name;
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }
        /// <summary>
        /// 内容类别新增或修改
        /// </summary>
        /// <param name="input"></param>
        public void UpdateOrInserContentType(ContentTypeInput input)
        {
            var type = _contentTypeRepository.GetAll().FirstOrDefault(a => a.Id == input.Id) ?? new ContentType();
            // var temp = _contentTypeRepository.GetAll().FirstOrDefault(a => a.Id == input.ParentId);
            var typeCount = _contentTypeRepository.GetAll().Count(a => a.Name == input.Name);
            if (input.DefineId == 0)
                throw new UserFriendlyException("请选择内容定义");
            //if (typeCount >= 1 && input.Id == 0)
            //    throw new UserFriendlyException("内容类别名称已存在");
            //if (typeCount >= 2 && input.Id != 0)
            //    throw new UserFriendlyException("内容类别名称已存在");
            if (_contentTypeRepository.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            type.DefineId = input.DefineId;
            type.CreationTime = DateTime.Now;
            type.Name = input.Name;
            if (input.Id == 0)
            {
                if (input.ParentId != 0)//新增
                    type.ParentId = input.ParentId;
            }
            else
            {
                var state = false;
                var result = _contentTypeRepository.GetAll().Where(a => a.ParentId == input.Id);
                foreach (var item in result)
                {
                    if (item.Id == input.ParentId)
                        state = true;
                }
                if (!state && input.Id != input.ParentId)
                    type.ParentId = input.ParentId;
                else
                    throw new UserFriendlyException("保存失败,不能嵌套循环！上级不能选择自身！");
                //if (temp != null && (input.ParentId != type.ParentId && temp.Id != type.Id))//编辑
                //    type.ParentId = input.ParentId;
                //else if (temp == null)
                //{
                //    type.ParentId = input.ParentId;
                //}
            }
            type.ShowOrder = input.ShowOrder;
            _contentTypeRepository.InsertOrUpdate(type);
        }

        /// <summary>
        /// 根据内容定义Id 获取内容类别树
        /// </summary>
        /// <returns></returns>
        public object GetContentTypeParentTreeJson(int id, int conntentTypeId)
        {
            var contentType = _contentTypeRepository.GetAll().Where(a => a.DefineId == id);
            var firstOrDefault = contentType.FirstOrDefault(a => a.Id == conntentTypeId);
            var checkedId = 1;
            if (firstOrDefault != null)
            {
                checkedId = Convert.ToInt32(firstOrDefault.ParentId);
            }
            var contentTypeNodes = contentType.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                Checked = checkedId,
                pId = s.ParentId,
                iconSkin = s.ChildContentType.Any() ? "root" : "menu"
            }).ToList();

            return contentTypeNodes;
        }

      

        /// <summary>
        /// 根据ID查询内容类别及配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentTypeInput GetContentType(long id)
        {
            try
            {
                return _contentTypeRepository.Get(id).MapTo<ContentTypeInput>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        /// <summary>
        /// 删除内容类别
        /// </summary>
        /// <param name="input"></param>
        public void DelContentType(EntityDto<long> input)
        {
            try
            {
                var type = _contentTypeRepository.Get(input.Id);
                var parent = _contentTypeRepository.GetAll().Where(a => a.ParentId == type.Id);
                var content = _contentRepository.GetAll().Count(a => a.DefineTypeId == input.Id);
                if (content > 0)
                {
                    throw new UserFriendlyException("删除出错，该条内容正在被使用，请先删除后，在执行此删除操作！");
                }
                if (parent.Any())
                    throw new UserFriendlyException("该节点存在子节点，请先删除子节点！");
                _contentTypeRepository.Delete(type);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        
        #endregion

        

        

        

    }
}
