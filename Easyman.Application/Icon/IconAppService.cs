using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization;
using Easyman.Authorization;
using System.Data;
using Abp.Application.Services.Dto;
using System;

namespace Easyman.Sys
{
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class IconAppService : EasymanAppServiceBase, IIconAppService
    {
        #region 初始化

        private readonly IRepository<Icon,long> _iconRepository;
        private readonly IRepository<IconType, long> _iconTypeRepository;

        public IconAppService(IRepository<Icon,long> iconRepository,IRepository<IconType,long> iconTypeRepository)
        {
            _iconRepository = iconRepository;
            _iconTypeRepository = iconTypeRepository;
        }

        #endregion


        #region 图标
        /// <summary>
        /// 获取所有图标
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAllIcons()
        {

            var icons = _iconRepository.GetAll().Select(x => new IconModel { Type = x.DisplyName, Value = x.ClassName });
            var result = icons.GroupBy(g => g.Type).Select(s => new { Key = s.Key, Icons = s });
            return result.ToList();

        }

        /// <summary>
        /// 获取所有图标类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAllTypeIcons()
        {
            var icons = _iconTypeRepository.GetAll().Select(x => new IconTypeModel { Id = x.Id, Value = x.Name });
            //var result = icons.GroupBy(g => g.Id).Select(s => new { Key = s.Key, Icons = s });
            return icons.ToList();
        }
        /// <summary>
        /// 根据图标类型获取图标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAllIconsId()
        {
            //修改，根据图标类型获取图标

            var icons = _iconRepository.GetAll().Select(x => new IconModels { Id=x.Id, Type = x.DisplyName, Value = x.ClassName ,TypeId=x.IconTypeId});
            //var result = icons.GroupBy(g => g.Type).Select(s => new { Key = s.Key, Icons = s });
            return icons.ToList();
        }
        /// <summary>
        /// 根据图标类型获取图标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<object> GetTypeIcons(long id)
        {
            //修改，根据图标类型获取图标


            var icons = _iconRepository.GetAll().Where(y => y.IconTypeId == id).Select(x => new IconModel { Type = x.DisplyName, Value = x.ClassName });
            var result = icons.GroupBy(g => g.Type).Select(s => new { Key = s.Key, Icons = s });
            return result.ToList();
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public IconSearchOutput GetAll(IconSearchInput input)
        {
            int reordCount;
            var dataList = _iconRepository.GetAll()
                .SearchByInputDto(input, out reordCount).ToList().MapTo<List<IconOutput>>();

            var outData = new IconSearchOutput();
            input.Page.TotalCount = reordCount;
            outData.Datas = dataList;
            outData.Page = input.Page;
            return outData;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(IconInput input)
        {
            var data = input.MapTo<Icon>();
            _iconRepository.InsertOrUpdate(data);
        }
        /// <summary>
        /// 获取单个数据模型
        /// </summary>
        /// <param name="id">需要修改的Id</param>
        /// <returns></returns>
        public IconInput Get(long id)
        {
            var item = _iconRepository.FirstOrDefault(a=>a.Id == id);
            var data = item.MapTo<IconInput>();
            return data;
        }
        /// <summary>
        /// 删除指定的项
        /// </summary>
        /// <param name="input">需要删除的Id</param>
        public void Del(EntityDto<long> input)
        {
            _iconRepository.Delete(a => a.Id == input.Id);
        }

        /// <summary>
        /// 获取类型下拉,有请选择
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetIconType()
        {
            //var type= _iconTypeRepository.GetAll().Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();
            //return type;
            List<SelectListItem> DataList = new List<SelectListItem>();
            DataList.Add(new SelectListItem { Text = "--请选择--", Value = "0" });
            DataList.AddRange(_iconTypeRepository.GetAll().Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList());
            return DataList;


        }
        #endregion

        #region 图标类别
        /// <summary>
        /// 图标类别查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IconTypeSearchOutput SearchIconType(IconTypeSearchInput input)
        {
            int rowCount;
            var data = _iconTypeRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new IconTypeSearchOutput
            {
                Datas = data.ToList().Select(s =>
                {
                    var temp = s.MapTo<IconTypeOutput>();

                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }
        /// <summary>
        /// 类别新增或修改
        /// </summary>
        /// <param name="input"></param>
        public void UpdateOrInserIconType(IconTypeInput input)
        {
           
            var type = _iconTypeRepository.GetAll().FirstOrDefault(a => a.Id == input.Id) ?? new IconType();
            if (_iconTypeRepository.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }

            type.Name = input.Name;
            type.Remark = input.Remark;
            var res= _iconTypeRepository.InsertOrUpdate(type);
            if (res == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            

        }



        /// <summary>
        /// 内容定义树
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetDefineTreeJson()
        {
            var define = await _iconTypeRepository.GetAllListAsync();

            var defineNodes = define.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                iconSkin = "menu"
            }).ToList();

            return defineNodes;
        }

        /// <summary>
        /// 根据ID查询类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IconTypeInput GetIconType(long id)
        {
            try
            {
                
                return _iconTypeRepository.Get(id).MapTo<IconTypeInput>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("查询出错，对象或已被删除！");
            }

        }

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="input"></param>
        public void DelIconType(EntityDto<long> input)
        {
            try
            {
                var type = _iconTypeRepository.Get(input.Id);
                var content = _iconRepository.GetAll().Count(a => a.IconTypeId == input.Id);
                if (content > 0)
                {
                    throw new UserFriendlyException("删除出错，图标类型正在被使用，请先删除图标，在执行此删除操作！");

                }
                else
                {
                    _iconTypeRepository.Delete(type);

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作出错，对象或已被删除！");
            }




        }

        /// <summary>
        /// 查询所有的类型
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetTypes()
        {
            return _iconTypeRepository.GetAll().Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();

        }
        #endregion
    }
}
