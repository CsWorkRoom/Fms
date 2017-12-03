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
    /// 功能定义管理
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class DefineAppService : EasymanAppServiceBase, IDefineAppService
    {
        #region 初始化
        private readonly IRepository<Define, long> _defineRepository;
        private readonly IRepository<DefineConfig, long> _defineConfigRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ContentType, long> _contentTypeRepository;
        private readonly IRepository<Domain.Content, long> _contentRepository;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defineRepository"></param>
        /// <param name="defineConfigRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="contentTypeRepository"></param>
        /// <param name="contentRepository"></param>
        public DefineAppService(IRepository<Define, long> defineRepository,  IRepository<DefineConfig, long> defineConfigRepository,
            IRepository<User, long> userRepository, IRepository<ContentType, long> contentTypeRepository,
            IRepository<Domain.Content, long> contentRepository)
        {
            _defineRepository = defineRepository;
            _defineConfigRepository = defineConfigRepository;
            _userRepository = userRepository;
            _contentTypeRepository = contentTypeRepository;
            _contentRepository = contentRepository;

        }
        #endregion

        #region 内容定义
        /// <summary>
        /// 获取内容定义下拉树
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetDefine()
        {
            return _defineRepository.GetAll().Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();

        }
        /// <summary>
        /// 添加内容定义
        /// </summary>
        /// <param name="define"></param>
        /// <returns></returns>
        public void AddContentDefine(Define define)
        {
            try
            {
                _defineRepository.Insert(define);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("添加失败！失败原因：" + ex.Message);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ContentDefineSearchOutput Search(ContentDefineSearchInput input)
        {
            int rowCount;
            var data = _defineRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new ContentDefineSearchOutput
            {
                Datas = data.ToList().Select(s =>
                {
                    var temp = s.MapTo<ContentDefineOutput>();
                    if (s.CreatorUserId != null)
                        temp.CreateUId = _userRepository.FirstOrDefault((long)s.CreatorUserId).Name;
                    if (s.LastModifierUserId != null)
                        temp.UpdateUId = _userRepository.FirstOrDefault((long)s.LastModifierUserId).Name;
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }

        /// <summary>
        /// 内容定义插入及更新
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void UpdateOrInserContentDefine(ContentDefineInput input)
        {
            var define = _defineRepository.GetAll().FirstOrDefault(a => a.Id == input.Id) ?? new Define();
            long dId = 0;
            var currentUserId = AbpSession.UserId;
            var user = _userRepository.FirstOrDefault(a => a.Id == currentUserId);
            define.Name = input.Name;
            define.Code = input.Code;
            var defineCount = _defineRepository.GetAll().Count(a => a.Name == input.Name);

            var defineCount1 = _defineRepository.GetAll().Count(a => a.Code == input.Code);
            if (input.Id == 0)
            {
                if (defineCount >= 1)
                    throw new UserFriendlyException("内容定义名称已存在");
                if (defineCount1 >= 1)//新增
                    throw new UserFriendlyException("内容定义Code已存在");
                dId = _defineRepository.InsertAndGetId(define);
                if (dId != 0)
                {
                    //1 表示true，0表示false
                    DefineConfig config = new DefineConfig
                    {
                        DefineId = dId,
                        CreateTime = DateTime.Now,
                        IsDelete = input.IsDelete,
                        IsLike = input.IsLike,
                        IsReoly = input.IsReoly,
                        IsReolyFile = input.IsReolyFile,
                        IsReolyFloor = input.IsReolyFloor,
                        IsReolyFloorFile = input.IsReolyFloorFile,
                        IsShare = input.IsShare,
                        IsChenkUser = input.IsChenkUser,
                        IsChenkRole = input.IsChenkRole,
                        IsChenkDistrict = input.IsChenkDistrict,
                        IsContentFile=input.IsContentFile,
                        IsText = input.IsText
                    };
                    var cId = _defineConfigRepository.InsertAndGetId(config);
                }
            }
            else
            {
                if (defineCount >= 2)
                    throw new UserFriendlyException("内容定义名称已存在");
                if (defineCount1 >= 2)//编辑
                    throw new UserFriendlyException("内容定义Code已存在");
                _defineRepository.Update(define);
                var config = _defineConfigRepository.GetAll().SingleOrDefault(a => a.DefineId == define.Id);
                if (config != null)
                {
                    config.IsDelete = input.IsDelete;
                    config.IsLike = input.IsLike;
                    config.IsReoly = input.IsReoly;
                    config.IsReolyFile = input.IsReolyFile;
                    config.IsReolyFloor = input.IsReolyFloor;
                    config.IsReolyFloorFile = input.IsReolyFloorFile;
                    config.IsShare = input.IsShare;
                    config.IsChenkUser = input.IsChenkUser;
                    config.IsChenkRole = input.IsChenkRole;
                    config.IsChenkDistrict = input.IsChenkDistrict;
                    config.IsText = input.IsText;
                    config.IsContentFile = input.IsContentFile;
                    _defineConfigRepository.Update(config);
                }
            }

        }


      

        /// <summary>
        /// 查询类容定义基础信息及配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentDefineInput GetContentDefine(long id)
        {
            try
            {
                var define = _defineRepository.Get(id);
                var defineConfig = _defineConfigRepository.FirstOrDefault(a => a.DefineId == id);
                ContentDefineInput ccm = new ContentDefineInput
                {
                    Id = Convert.ToInt32(define.Id),
                    Name = define.Name,
                    Code = define.Code,
                    IsDelete = Convert.ToBoolean(defineConfig.IsDelete),
                    IsLike = Convert.ToBoolean(defineConfig.IsLike),
                    IsContentFile = Convert.ToBoolean(defineConfig.IsContentFile),
                    IsReoly = Convert.ToBoolean(defineConfig.IsReoly),
                    IsReolyFile = Convert.ToBoolean(defineConfig.IsReolyFile),
                    IsReolyFloor = Convert.ToBoolean(defineConfig.IsReolyFloor),
                    IsReolyFloorFile = Convert.ToBoolean(defineConfig.IsReolyFloorFile),
                    IsShare = Convert.ToBoolean(defineConfig.IsShare),
                    IsChenkUser=Convert.ToBoolean(defineConfig.IsChenkUser),
                    IsChenkRole = Convert.ToBoolean(defineConfig.IsChenkRole),
                    IsChenkDistrict = Convert.ToBoolean(defineConfig.IsChenkDistrict),
                   
                    IsText = Convert.ToBoolean(defineConfig.IsText)
                };
                return ccm;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("查询出错，请联系管理员" + ex.Message);
            }
        }

        /// <summary>
        /// 内容定义删除
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void DelContentDefine(EntityDto<long> input)
        {
            try
            {
                var define = _defineRepository.GetAll().FirstOrDefault(a => a.Id == input.Id);
                var config = _defineConfigRepository.GetAll().SingleOrDefault(a => a.DefineId == define.Id);
                //根据功能定义ID查询出对应在使用的类型
                var contentType = _contentTypeRepository.GetAll().Where(a=>a.DefineId==input.Id);
                var typeContent = contentType.Count(a => a.DefineId == define.Id);
                foreach (var item in contentType)
                {
                    var content = _contentRepository.GetAll().Count(a => a.DefineTypeId == item.Id);
                    if (content > 0)
                        throw new UserFriendlyException("删除出错，该条内容定义正在被使用，请先删除后，在执行此删除操作！");
                }
                if (typeContent > 0)
                    throw new UserFriendlyException("删除出错，该条内容正在被使用，请先删除后，在执行此删除操作！");

                _defineConfigRepository.Delete(config);
                _defineRepository.Delete(define);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除出错，请联系管理员" + ex.Message);
            }
        }


        /// <summary>
        /// 内容定义树
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetDefineTreeJson()
        {
            var define = await _defineRepository.GetAllListAsync();

            var defineNodes = define.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                iconSkin = "menu"
            }).ToList();

            return defineNodes;
        }

        #endregion









    }
}
