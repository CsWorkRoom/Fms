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
    /// 内容管理
    /// </summary>
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class ContentAppService : EasymanAppServiceBase, IContentAppService
    {
        #region 初始化
        private readonly IRepository<Define, long> _defineRepository;
        private readonly IRepository<DefineConfig, long> _defineConfigRepository;
       
        private readonly IRepository<ContentType, long> _contentTypeRepository;
        private readonly IRepository<Domain.Content, long> _contentRepository;
        private readonly IRepository<ContentTag, long> _contentTagRepository;
        private readonly IRepository<ContentRefTag, long> _contentRefTagRepository;
        private readonly IRepository<ContentUser, long> _contentUserRepository;
        private readonly IRepository<ContentRole, long> _contentRoleRepository;
        //组织关联表
        private readonly IRepository<ContentDistrict, long> _contentDistrictRepository;
        private readonly IRepository<District, long> _districtRepository;
        //全选记录表
        private readonly IRepository<ContentCheck, long> _contentCheckRepository;
        private readonly IRepository<ContentFile, long> _contentFileRepository;
        private readonly IRepository<Files, long> _filesRepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<ContentPushWay, long> _contentPushWayRepository;

        private readonly IRepository<ContentReadLog, long> _contentReadLogRepository;
        private readonly IRepository<ContentPraiseLog, long> _contentPraiseLogRepository;
        private readonly IRepository<ContentReply, long> _contentReplyRepository;
        private readonly IRepository<ReplyPraiseLog, long> _replyPraiseLogRepository;

        private readonly IReplyPraiseAppService _replyPraiseAppService;

        public ContentAppService(
            IRepository<Define, long> defineRepository,
            IRepository<User, long> userRepository, 
            IRepository<Domain.Content, long> contentRepository,
            IRepository<DefineConfig, long> defineConfigRepository, 
            IRepository<ContentType, long> contentTypeRepository,
            RoleManager roleManager,
            IRepository<UserRole, long> userRoleRepository, 
            IRepository<ContentTag, long> contentTagRepository,
            IRepository<ContentRefTag, long> contentRefTagRepository,
            IRepository<ContentUser, long> contentUserRepository,
            IRepository<ContentRole, long> contentRoleRepository,
            IRepository<ContentDistrict, long> contentDistrictRepository,
            IRepository<District, long> districtRepository,
            IRepository<ContentCheck, long> contentCheckRepository,
            IRepository<ContentFile, long> contentFileRepository, 
            IRepository<Files, long> fileRepository,
            IRepository<ContentPushWay, long> contentPushWayRepository,
            IRepository<ContentReadLog, long> contentReadLogRepository,
            IRepository<ContentPraiseLog, long> contentPraiseLogRepository,
            IRepository<ContentReply, long> contentReplyRepository,
            IRepository<ReplyPraiseLog, long> replyPraiseLogRepository,
            IReplyPraiseAppService replyPraiseAppService
            )
        {
            _defineRepository = defineRepository;
            _userRepository = userRepository;
            _contentRepository = contentRepository;
            _defineConfigRepository = defineConfigRepository;
          
            _contentTypeRepository = contentTypeRepository;
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
            _contentTagRepository = contentTagRepository;
            _contentRefTagRepository = contentRefTagRepository;
            _contentUserRepository = contentUserRepository;
            _contentRoleRepository = contentRoleRepository;
            _districtRepository = districtRepository;
            _contentDistrictRepository =contentDistrictRepository;
            _contentCheckRepository = contentCheckRepository;
            _contentFileRepository = contentFileRepository;
            _filesRepository = fileRepository;
            _contentPushWayRepository = contentPushWayRepository;

            _contentReadLogRepository = contentReadLogRepository;
            _contentPraiseLogRepository = contentPraiseLogRepository;
            _contentReplyRepository = contentReplyRepository;
            _replyPraiseLogRepository = replyPraiseLogRepository;
            _replyPraiseAppService = replyPraiseAppService;
        }
        #endregion
        
        #region 基础方法
        public ContentIndexSearchOutput SearchContent(ContentIndexSearchInput input)
        {
            int rowCount;
            var data = _contentRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new ContentIndexSearchOutput
            {
                Datas = data.ToList().Select(s =>
                {
                    var temp = s.MapTo<ContentIndexOutput>();
                    var tmp = _userRepository.FirstOrDefault(Convert.ToInt32(s.CreatorUserId));
                    temp.CreateName = tmp.Name;
                    temp.CreateTime = tmp.CreationTime;
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }

        public ContentIndexSearchOutput NewSearchContent(ContentIndexSearchInput input)
        {
            int rowCount;
            var data = new List<Domain.Content>();
            var userId = AbpSession.UserId;
            List<Domain.Content> list = new List<Domain.Content>();
            var content = _contentRepository.GetAll().Where(a => a.IsUse == true);//查询出全部被启用的内容
            if (input.Code != "")
            {
                var define = _defineRepository.GetAll().SingleOrDefault(a => a.Code == input.Code);
                var type = _contentTypeRepository.GetAll().Where(a => a.DefineId == define.Id).Select(a => a.Id).ToList();
                var typeStr = string.Join(",", type);
                content = content.Where(a => typeStr.Contains(a.DefineTypeId.ToString()));
                //content = from u in content
                //        where type.Contains(u.DefineType)
                //        select u;
            }

            var contentUser = _contentUserRepository.GetAll().Where(a => a.UserId == userId);
            var limitUList = _contentUserRepository.GetAll().Where(a => a.UserId == userId && a.IsAllow == false);//被限制的用户
            var role = _userRoleRepository.GetAll().Where(a => a.UserId == userId);
            var roleList = _contentRoleRepository.GetAll();
            var userList = _contentUserRepository.GetAll();
            var like = _contentPraiseLogRepository.GetAll();
            List<Domain.Content> listRole = new List<Domain.Content>();
            foreach (var item in role)
            {
                //当前登录人所在角色的内容
                foreach (var c in content)
                {
                    foreach (var crole in roleList)
                    {
                        if (crole.RoleId == item.RoleId && crole.ContentId == c.Id)
                        {
                            var rem = userList.SingleOrDefault(a => a.ContentId == c.Id && a.UserId == item.UserId);
                            if (rem == null)
                                listRole.Add(c);
                            //if(rem!= null && rem.IsProhibit != 1)
                            //    listRole.Add(c);
                        }

                    }
                }
            }

            List<Domain.Content> listUser = new List<Domain.Content>();
            foreach (var user in contentUser)
            {
                if (user.IsAllow != true)
                {
                    var contents = content.SingleOrDefault(a => a.Id == user.ContentId && !a.IsDeleted);
                    if (contents != null)
                        listUser.Add(contents);
                }
            }

            List<Domain.Content> listAll = new List<Domain.Content>();//全部用户访问的内容
            List<Domain.Content> limitList = new List<Domain.Content>();//当前用户被限制的内容
            foreach (var item in content)
            {
                foreach (var li in limitUList)
                {
                    if (li.ContentId == item.Id)
                        limitList.Add(item);
                }
            }
            foreach (var item in content)
            {
                var rem = limitList.Where(a => a.Id == item.Id);//排除被限制访问部分
                var limitRole = roleList.Where(a => a.ContentId == item.Id);//排除部分角色访问部分
                var limiUser = userList.Where(a => a.ContentId == item.Id);//排除部分用户访问部分
                if (!rem.Any() && !item.IsDeleted && !limitRole.Any() && !limiUser.Any())
                    listAll.Add(item);
            }
            list.AddRange(listAll);
            if (listRole.Count != 0)//仅选择角色时的内容
                list.AddRange(listRole);
            if (listUser.Count != 0)//仅部分用户时的内容
                list.AddRange(listUser);

            if (list.Any())
                data = input.SearchName == "" ? list.AsQueryable().SearchByInputDto(input, out rowCount).ToList() :
                   list.AsQueryable().Where(a => a.Title.Contains(input.SearchName)).SearchByInputDto(input, out rowCount).ToList();
            else
                data = input.SearchName == "" ? content.SearchByInputDto(input, out rowCount).ToList() :
                   content.Where(a => a.Title.Contains(input.SearchName)).SearchByInputDto(input, out rowCount).ToList();
            var output = new ContentIndexSearchOutput
            {
                Datas = data.Select(s =>
                {
                    var temp = s.MapTo<ContentIndexOutput>();
                    var tmp = _userRepository.FirstOrDefault(Convert.ToInt32(s.CreatorUserId));
                    temp.CreateName = tmp.Name;
                    temp.CreateTime = tmp.CreationTime;
                    temp.ReadContent = _replyPraiseAppService.ContentReadContent(Convert.ToInt32(s.Id));
                    temp.ReplyCount = _replyPraiseAppService.ReplyContent(Convert.ToInt32(s.Id));
                    temp.ContentPraiseCount = _replyPraiseAppService.ContentPraiseCount(Convert.ToInt32(s.Id));
                    var tem = like.Where(a => a.ContentId == s.Id && a.UserId == AbpSession.UserId);
                    if (tem.Any())
                        temp.IsOrLike = true;
                    else
                        temp.IsOrLike = false;
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }

        /// <summary>
        /// 内容插入及更新
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public void UpdateOrInserContentIndex(ContentIndexInput input)
        {
            if (input.ContentTypeId == 0)
            {
                //提示：请选择内容类型
                throw new UserFriendlyException("请选择内容类别！");
            }
            var type = _contentRepository.GetAll().FirstOrDefault(a => a.Id == input.Id) ?? new Domain.Content();
            type.DefineId = input.DefineId;
            type.DefineTypeId = input.ContentTypeId;
            type.Title = input.Title;
            type.Summary = input.Summary;
            type.Info = input.Info;
            type.IsUrgent = input.IsUrgent;
            type.IsUse = input.IsUse;

            if (input.Id == 0)
            {
                var cId = _contentRepository.InsertAndGetId(type);
                type.Id = cId;
                ContentTag tag = new ContentTag { Info = input.TagName };
                var tagId = _contentTagRepository.InsertAndGetId(tag);
                ContentRefTag refTag = new ContentRefTag
                {
                    ContentId = cId,
                    TagId = tagId
                };
                _contentRefTagRepository.Insert(refTag);

            }
            else
            {
                _contentRepository.Update(type);
                var tag = _contentRefTagRepository.GetAll().Where(a => a.ContentId == type.Id);
                foreach (var item in tag)
                {
                    var contentTag = _contentTagRepository.Get(item.TagId);
                    contentTag.Info = input.TagName;
                    _contentTagRepository.Update(contentTag);
                }
            }

            //推送模式
            var pushList = input.PushId.Split(',');
            var pustType = _contentPushWayRepository.GetAll().Where(a => a.ContentId == type.Id);
            //删除原来的推送方式
            foreach (var item in pustType)
            {
                _contentPushWayRepository.Delete(item);
            }
            //新增推送模式
            foreach (var item in pushList)
            {
                if (item != "")
                {
                    var pushId = item.ToInt32();

                    ContentPushWay push = new ContentPushWay
                    {
                        ContentId = type.Id,
                        PushWayId = pushId
                    };
                    _contentPushWayRepository.Insert(push);
                }
            }
            #region 全选配置记录表

            var check = _contentCheckRepository.GetAll().FirstOrDefault(a => a.ContentId == type.Id);
            //有内容修改，无内容就新增
            if (check != null)
            {
                check.ContentId = type.Id;
                check.IsCheckUser = input.IsAllUser;
                check.IsCheckRole = input.IsAllRole;
                check.IsCheckDistrict = input.IsAllDistrict;

                _contentCheckRepository.Update(check);
            }
            else
            {
                ContentCheck permiss = new ContentCheck
                {
                    ContentId = type.Id,
                    IsCheckUser = input.IsAllUser,
                    IsCheckRole = input.IsAllRole,
                    IsCheckDistrict = input.IsAllDistrict,
                };
                _contentCheckRepository.Insert(permiss);
            }
            #endregion

            //根据内容功能定义配置，进行权限处理
            var config = _defineConfigRepository.Get(type.DefineId);
            if (config != null)
            {
                //1 true表示有权限
                if (config.IsChenkUser == true)
                {
                    #region 用户权限处理
                    //指定用户集合
                    var userIdList = input.UserListId.Split(',');
                    //限制用户集合
                    var limitUser = input.UserNameLimitList.Split(',');
                    //删除用户权限
                    var uRef = _contentUserRepository.GetAll().Where(a => a.ContentId == type.Id);
                    foreach (var u in uRef)
                    {
                        _contentUserRepository.Delete(u);
                    }
                    //新增用户权限
                    //新增指定用户
                    foreach (var item in userIdList)
                    {
                        if (item != "")
                        {
                            ContentUser refUser = new ContentUser();
                            refUser.ContentId = type.Id;
                            refUser.UserId = item.ToInt32();
                            refUser.IsAllow = true;
                            _contentUserRepository.Insert(refUser);
                        }
                    }
                    //添加限制名单
                    foreach (var item in limitUser)
                    {
                        if (item != "")
                        {
                            ContentUser refUser = new ContentUser();
                            refUser.ContentId = type.Id;
                            refUser.UserId = item.ToInt32();
                            refUser.IsAllow = false;
                            _contentUserRepository.Insert(refUser);
                        }
                    }
                    #endregion
                }
                if (config.IsChenkRole == true)
                {
                    #region 角色权限处理
                    //指定角色
                    var roleList = input.RoleListId.Split(',');
                    //删除角色
                    var roles = _contentRoleRepository.GetAll().Where(a => a.ContentId == type.Id);
                    foreach (var r in roles)
                    {
                        _contentRoleRepository.Delete(r);
                    }
                    //新增指定角色
                    foreach (var item in roleList)
                    {
                        if (item != "")
                        {
                            ContentRole refRole = new ContentRole
                            {
                                ContentId = type.Id,
                                IsAllow = true,
                                RoleId = item.ToInt32()
                            };
                            _contentRoleRepository.Insert(refRole);
                        }

                    }
                    //限制角色
                    var roleListNo = input.RoleListIdNo.Split(',');
                    //新增限制角色
                    foreach (var item in roleListNo)
                    {
                        if (item != "")
                        {
                            ContentRole refRole = new ContentRole
                            {
                                ContentId = type.Id,
                                IsAllow = false,
                                RoleId = item.ToInt32()
                            };
                            _contentRoleRepository.Insert(refRole);
                        }

                    }
                    #endregion
                }
                if (config.IsChenkDistrict == true)
                {
                    #region 组织权限处理

                    //删除组织权限表数据
                    var districts = _contentDistrictRepository.GetAll().Where(a => a.ContentId == type.Id);
                    foreach (var r in districts)
                    {
                        _contentDistrictRepository.Delete(r);
                    }
                    //指定组织
                    var disList = input.DistrictListId.Split(',');
                    //新增指定组织
                    foreach (var item in disList)
                    {
                        if (item != "")
                        {
                            ContentDistrict refDis = new ContentDistrict
                            {
                                ContentId = type.Id,
                                IsAllow = true,
                                DistrictId = item.ToInt32()
                            };
                            _contentDistrictRepository.Insert(refDis);
                        }

                    }
                    //限制组织
                    var disListNo = input.DistrictListIdNo.Split(',');
                    //新增限制组织
                    foreach (var item in disListNo)
                    {
                        if (item != "")
                        {
                            ContentDistrict refDis = new ContentDistrict
                            {
                                ContentId = type.Id,
                                IsAllow = false,
                                DistrictId = item.ToInt32()
                            };
                            _contentDistrictRepository.Insert(refDis);
                        }

                    }
                    #endregion
                }
                //内容上传资料权限
                if (config.IsContentFile == true)
                {
                    var listFile = input.FileId.Split(',');
                    var contFile = _contentFileRepository.GetAll().Where(a => a.ContentId == type.Id);
                    //删除原来的内容上传文件
                    foreach (var item in contFile)
                    {
                        _contentFileRepository.Delete(item);
                    }
                    //新增上传文件
                    if (listFile.Count() > 0)
                    {
                        foreach (var item in listFile)
                        {
                            if (item != "" && item != "删除")
                            {
                                var fileId = item.ToInt32();
                                if (fileId != 0)
                                {
                                    ContentFile files = new ContentFile
                                    {
                                        ContentId = type.Id,
                                        FileId = fileId,
                                    };
                                    _contentFileRepository.Insert(files);
                                }
                            }
                        }
                    }
                }
            }



        }

        /// <summary>
        /// 根据ID查询一条内容信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentIndexInput GetContent(long id)
        {
            try
            {
                var content = _contentRepository.Get(id);
                ContentIndexInput model = new ContentIndexInput();
                model.Id = Convert.ToInt32(content.Id);
                model.Title = content.Title;
                model.Info = content.Info;
                model.IsUrgent = Convert.ToBoolean(content.IsUrgent);
                model.IsUse = Convert.ToBoolean(content.IsUse);
                model.Summary = content.Summary;
                model.CreateTime = content.CreationTime;
                model.CreatorUserId = content.CreatorUserId;
                model.ContentTypeId = Convert.ToInt32(content.DefineTypeId);
                var defineType = _contentTypeRepository.Get(content.DefineTypeId);
                model.DefineId = Convert.ToInt32(defineType.DefineId);
                //获取内容权限
                var defineConfig = _defineConfigRepository.GetAll().SingleOrDefault(a => a.DefineId == model.DefineId);
                if (defineConfig != null) model.IsDelete = Convert.ToBoolean(defineConfig.IsDelete);
                if (defineConfig != null) model.IsLike = Convert.ToBoolean(defineConfig.IsLike);
                if (defineConfig != null) model.IsReoly = Convert.ToBoolean(defineConfig.IsReoly);
                if (defineConfig != null) model.IsReolyFile = Convert.ToBoolean(defineConfig.IsReolyFile);
                if (defineConfig != null) model.IsReolyFloor = Convert.ToBoolean(defineConfig.IsReolyFloor);
                if (defineConfig != null) model.IsReolyFloorFile = Convert.ToBoolean(defineConfig.IsReolyFloorFile);
                if (defineConfig != null) model.IsShare = Convert.ToBoolean(defineConfig.IsShare);
                if (defineConfig != null) model.IsText = Convert.ToBoolean(defineConfig.IsText);
                if (defineConfig != null) model.IsCheckUser = Convert.ToBoolean(defineConfig.IsChenkUser);
                if (defineConfig != null) model.IsCheckRole = Convert.ToBoolean(defineConfig.IsChenkRole);
                if (defineConfig != null) model.IsCheckDistrict = Convert.ToBoolean(defineConfig.IsChenkDistrict);
                if (defineConfig != null) model.IsContentFile = Convert.ToBoolean(defineConfig.IsContentFile);
                #region 抛弃
                //if (defineConfig != null)
                //{
                //    if (defineConfig.IsChenkUser.ToString() != "")
                //    {
                //        model.IsCheckUser = Convert.ToBoolean(defineConfig.IsChenkUser);
                //    }
                //    else
                //    {
                //        //为空时，默认没有权限
                //        model.IsCheckUser = false;
                //    }
                //}
                //if (defineConfig != null)
                //{
                //    if (defineConfig.IsChenkRole.ToString() != "")
                //    {
                //        model.IsCheckRole = Convert.ToBoolean(defineConfig.IsChenkRole);
                //    }
                //    else
                //    {
                //        //为空时，默认没有权限
                //        model.IsCheckRole = false;
                //    }
                //}
                //if (defineConfig != null)
                //{
                //    if (defineConfig.IsChenkDistrict.ToString() != "")
                //    {
                //        model.IsCheckDistrict = Convert.ToBoolean(defineConfig.IsChenkDistrict);
                //    }
                //    else
                //    {
                //        //为空时，默认没有权限
                //        model.IsCheckDistrict = false;
                //    }
                //}
                #endregion
                var tag = _contentRefTagRepository.GetAll().SingleOrDefault(a => a.ContentId == content.Id);
                if (tag != null) model.TagName = tag.Tag.Info;

                //获取全选配置记录
                var check = _contentCheckRepository.GetAll().FirstOrDefault(x => x.ContentId == content.Id);
                if (check != null)
                {
                    model.IsAllUser = Convert.ToBoolean(check.IsCheckUser);
                    model.IsAllRole = Convert.ToBoolean(check.IsCheckRole);
                    model.IsAllDistrict = Convert.ToBoolean(check.IsCheckDistrict);
                }
                else
                {
                    //为空时，默认全选
                    model.IsAllUser = true;
                    model.IsAllRole = true;
                    model.IsAllDistrict = true;
                }

                if (model.IsCheckUser)
                {
                    #region 用户权限
                    //先查询指定用户
                    var userLists = _contentUserRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == true);

                    var uI = 0;
                    var userList = "";
                    var userNameList = "";

                    foreach (var u in userLists)
                    {
                        uI++;
                        if (uI == 1)
                        {
                            userList += u.UserId;
                            userNameList += _userRepository.Get(u.UserId).Name;
                        }
                        else
                        {
                            userList += "," + u.UserId;
                            userNameList += "," + _userRepository.Get(u.UserId).Name;
                        }
                    }
                    //指定用户,用户ID和用户姓名
                    model.UserListId = userList;
                    model.UserListName = userNameList;
                    //查询限制用户
                    var userListsNo = _contentUserRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == false);
                    var userI = 0;
                    var userListNo = "";
                    var userNameListNo = "";
                    foreach (var u in userListsNo)
                    {
                        userI++;
                        if (userI == 1)
                        {
                            userListNo += u.UserId;
                            userNameListNo += _userRepository.Get(u.UserId).Name;
                        }
                        else
                        {
                            userListNo += "," + u.UserId;
                            userNameListNo += "," + _userRepository.Get(u.UserId).Name;

                        }
                    }
                    //限制用户,用户ID和用户姓名
                    model.UserNameLimitList = userListNo;
                    model.UserListNameNo = userNameListNo;
                    #endregion
                }
                else
                {
                    model.UserListId = "";
                    model.UserListName = "";

                    model.UserNameLimitList = "";
                    model.UserListNameNo = "";
                }
                if (model.IsCheckRole)
                {
                    #region 角色权限
                    //查询指定角色
                    var role = _contentRoleRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == true);
                    var roleI = 0;
                    var roleList = "";
                    foreach (var item in role)
                    {
                        roleI++;
                        if (roleI == 1)
                            roleList += item.RoleId;
                        else
                            roleList += "," + item.RoleId;
                    }
                    model.RoleListId = roleList;
                    //查询限制角色
                    var roleNo = _contentRoleRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == false);
                    var roleINo = 0;
                    var roleListNo = "";
                    foreach (var item in roleNo)
                    {
                        roleINo++;
                        if (roleINo == 1)
                            roleListNo += item.RoleId;
                        else
                            roleListNo += "," + item.RoleId;
                    }
                    model.RoleListIdNo = roleListNo;
                    #endregion
                }
                else
                {
                    model.RoleListId = "";
                    model.RoleListIdNo = "";
                }
                if (model.IsCheckDistrict)
                {
                    #region 组织权限
                    //查询指定组织
                    var district = _contentDistrictRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == true);
                    var disI = 0;
                    var disList = "";
                    foreach (var item in district)
                    {
                        disI++;
                        if (disI == 1)
                            disList += item.DistrictId;
                        else
                            disList += "," + item.DistrictId;
                    }
                    model.DistrictListId = disList;
                    //查询限制组织
                    var districtNo = _contentDistrictRepository.GetAll().Where(a => a.ContentId == id && a.IsAllow == false);
                    var disNo = 0;
                    var disListNo = "";
                    foreach (var item in districtNo)
                    {
                        disNo++;
                        if (disNo == 1)
                            disListNo += item.DistrictId;
                        else
                            disListNo += "," + item.DistrictId;
                    }
                    model.DistrictListIdNo = disListNo;
                    #endregion

                }
                else
                {
                    model.DistrictListId = "";
                    model.DistrictListIdNo = "";
                }
                if (model.IsContentFile)
                {
                    #region 附件上传
                    //查询内容上传文件
                    var file = _contentFileRepository.GetAll().Where(a => a.ContentId == id);
                    var fileI = 0;
                    var fileList = "";
                    List<FileTemp> filesList = new List<FileTemp>();
                    foreach (var item in file)
                    {
                        fileI++;
                        if (fileI == 1)
                        {
                            fileList += item.FileId;

                        }
                        else
                        {
                            fileList += "," + item.FileId;
                        }
                        var contentfile = _filesRepository.Get(item.FileId);
                        var filetp = new FileTemp();
                        filetp.Id = contentfile.Id;
                        filetp.Name = contentfile.TrueName;
                        filetp.Length = contentfile.Length;
                        if (contentfile.Length > 1024 * 1024)
                        {
                            filetp.LengthKb = ((double)(contentfile.Length * 100 / (1024 * 1024)) / 100) + "MB";
                        }
                        else
                        {
                            filetp.LengthKb = ((double)(contentfile.Length * 100 / 1024) / 100) + "KB";
                        }
                        filetp.Uptime = contentfile.UploadTime;
                        filetp.Upurl = contentfile.Url;

                        filesList.Add(filetp);
                    }
                    model.FileId = fileList;
                    model.Files = filesList;
                    #endregion
                }
                else
                {
                    model.FileId = "";
                    model.Files = null;
                }
                if (model.IsReoly)
                {
                    #region 评论
                    //查询内容评论
                    var realyList = _contentReplyRepository.GetAll().Where(a => a.ContentId == id).ToList();
                    var realyI = 0;
                    var realyIdList = "";

                    foreach (var item in realyList)
                    {
                        realyI++;
                        if (realyI == 1)
                        {
                            realyIdList += item.Id;

                        }
                        else
                        {
                            realyIdList += "," + item.Id;
                        }

                    }
                    model.RealyLIstId = realyIdList;
                    #endregion
                }
                else
                {
                    model.RealyLIstId = "";
                }



                return model;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("查询出错，请联系管理员" + ex.Message);
            }
        }

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="input"></param>
        public void DelContentIndex(EntityDto<long> input)
        {

            try
            {
                _contentRepository.Delete(input.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("删除失败！" + ex.Message);
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FirstSearchContent()
        {
            var content = _contentRepository.GetAll().Where(a => a.IsUse == true).FirstOrDefault();//查询出全部被启用的内容
            var outCondent= content.MapTo<ContentIndexOutput>();
            return outCondent.Title;
        }
        /// <summary>
        /// 获取内容，用户未读的，最新的3条
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetNewContents()
        {
           
            //查询所有，启用的内容
            var contAll = _contentRepository.GetAll().Where(x => x.IsUse == true).OrderByDescending(a => a.CreationTime).ToList();
            //使用权限进行筛选，找出用户有权限的内容

            //循环contAll,加入符合权限的项
            List<Domain.Content> contAllContent = new List<Domain.Content>();
            foreach (var temt in contAll)
            {
                    var isAllow = GetIsAllow(temt.Id);
                    //判断权限
                    if (isAllow)
                    {
                        contAllContent.Add(temt);
                    } 
            }


            //2:根据用户ID查询出阅读记录ContentReadLog，AbpSession.UserId.Value;
            var userID = AbpSession.UserId.Value;//用户ID
            var contUser = _contentReadLogRepository.GetAll().Where(x => x.UserId == userID).Distinct().ToList();
            //把阅读过的内容ID放入list集合
            List<long> listContent = new List<long>();
            for (int i = 0; i < contUser.Count(); i++)
            {
                if (!listContent.Contains(contUser[i].ContentId))
                {
                    listContent.Add(contUser[i].ContentId);
                }
            }

            //保存未读记录
            List<ContentModelNoRead> listUserModel = new List<ContentModelNoRead>();
            //3：判断contAll中的内容ID,取出没有阅读记录的数据
            foreach (var item in contAllContent)
            {
                if (!listContent.Contains(item.Id))
                {
                    //加入listUserModel未读记录
                    ContentModelNoRead model3 = new ContentModelNoRead();
                    model3.id = item.Id;
                    model3.title = item.Title;
                    model3.type = _contentTypeRepository.Get(item.DefineTypeId).Name;
                    model3.createTime = item.CreationTime;
                    model3.is_user = 0;
                    listUserModel.Add(model3);
                }
            }

            if (listUserModel.Count() == 0)
            {
                //没有未阅读记录,返回最新的三条
                var listNewModel = contAllContent.Select(x => new ContentModelNoRead { id = x.Id, title = x.Title, type = _contentTypeRepository.Get(x.DefineTypeId).Name, createTime = x.CreationTime, is_user = 1 });
                return listNewModel.ToList().Take(3);
            }
            else if (listUserModel.Count() <= 3)
            {
                //返回未读记录
                return listUserModel;
            }
            else
            {
                //对listUserModel进行排序，返回最新的3条
                return listUserModel.OrderByDescending(a => a.createTime).Take(3);
            }
            #region 丢弃
            ////获取3条最新内容(没有阅读过的)
            //var content = _contentRepository.GetAll().Where(a => a.IsUse == true).OrderByDescending(a => a.CreationTime);//查询出全部被启用的内容,降序

            ////获取3条未读记录，如果未读记录没有，则取最新3条

            ////保存未读记录
            //List<ContentModelNoRead> listUserModel = new List<ContentModelNoRead>();


            ////1：根据用户ID查询出阅读记录ContentReadLog，AbpSession.UserId.Value;
            //var userID = AbpSession.UserId.Value;
            //var contUser = _contentReadLogRepository.GetAll().Where(x => x.UserId == userID).Distinct().ToList();
            ////把阅读过的内容ID放入list集合
            //List<long> listContent = new List<long>();
            //for (int i = 0; i < contUser.Count(); i++)
            //{
            //    if (!listContent.Contains(contUser[i].ContentId))
            //    {
            //        listContent.Add(contUser[i].ContentId);
            //    }
            //}

            ////2：查询所有启用的内容
            //var contAll= _contentRepository.GetAll().Where(x=>x.IsUse==true).OrderByDescending(a => a.CreationTime);

            ////3：判断contAll中的内容ID,取出没有阅读记录的数据

            //foreach (var item in contAll)
            //{
            //    if (!listContent.Contains(item.Id))
            //    {
            //        //加入listUserModel未读记录
            //        ContentModelNoRead model3 = new ContentModelNoRead();
            //        model3.id = item.Id;
            //        model3.title = item.Title;
            //        model3.createTime = item.CreationTime;
            //        model3.type = _contentTypeRepository.Get(item.DefineTypeId).Name;
            //        model3.is_user = 0;
            //        listUserModel.Add(model3);
            //    }               
            //}

            //if (listUserModel.Count() == 0)
            //{
            //    //没有未阅读记录,返回最新的三条
            //    var listNewModel = contAll.Select(x => new ContentModelNoRead { id = x.Id, title = x.Title, type = _contentTypeRepository.Get(x.DefineTypeId).Name, createTime = x.CreationTime, is_user = 1 });
            //    if (listNewModel.Count() > 3)
            //    {
            //        return listNewModel.ToList().Take(3);
            //    }
            //    else
            //    {
            //        return listNewModel;
            //    }

            //}
            //else if (listUserModel.Count() <= 3)
            //{
            //    //返回未读记录
            //    return listUserModel;
            //}
            //else 
            //{
            //    //对listUserModel进行排序，返回最新的3条
            //    return listUserModel.OrderByDescending(a => a.createTime).Take(3);
            //}
            #endregion

        }

        /// <summary>
        /// 根据内容ID判断登录用户是否有权限阅读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetIsAllow(long id)
        {           
            //内容
            var content = _contentRepository.Get(id);
            //功能配置
            var config = _defineConfigRepository.GetAll().FirstOrDefault(x => x.DefineId == content.DefineId);
            if (config.IsChenkUser != true && config.IsChenkRole != true && config.IsChenkDistrict != true)
            {
                //功能没有配置权限，默认该功能定义下数据，所有人可见
                return true;
            }
            else
            {
                //是否全选记录表
                var CheckAll = _contentCheckRepository.GetAll().FirstOrDefault(x => x.ContentId == content.Id); 
                var userID = AbpSession.UserId.Value;//用户ID
                var userDis = _userRepository.GetAll().FirstOrDefault(x => x.Id == userID).DistrictId;//用户组织ID
                var userRole = _userRoleRepository.GetAll().Where(x => x.UserId == userID).ToList(); //用户角色ID集合
                //是否有权限
                var isUserCheck = true;
                var isRollCheck = true;
                var isDisCheck = true;
                #region 用户权限
                if (config.IsChenkUser == true)
                {
                    //1：判断是否是限制用户
                    var userNo = _contentUserRepository.GetAll().FirstOrDefault(x => x.UserId == userID && x.ContentId == content.Id && x.IsAllow == false);
                    if (userNo != null)
                    {
                        isUserCheck = false;
                    }
                    else
                    {
                        //默认是否全选
                        if (CheckAll.IsCheckUser == false)
                        {
                            //2：判断是否是指定用户
                            var userYes = _contentUserRepository.GetAll().FirstOrDefault(x => x.UserId == userID && x.ContentId == content.Id && x.IsAllow == true);
                            if (userYes == null)
                            {
                                isUserCheck = false;
                            }
                        }
                        else { }

                    }
                }
                #endregion
                #region 角色权限
                if (config.IsChenkRole == true)
                {
                    //1：限制角色集合
                    var roleNo = _contentRoleRepository.GetAll().Where(x => x.ContentId == content.Id && x.IsAllow == false).ToList();
                    if (roleNo.Count > 0)
                    {
                        //限制角色ID集合
                        List<long> roleNoId = new List<long>();
                        for (int i = 0; i < roleNo.Count; i++)
                        {
                            roleNoId.Add(roleNo[i].RoleId);
                        }
                        foreach (var r in userRole)
                        {
                            if (roleNoId.Contains(r.RoleId))
                            {
                                //用户角色，在限制角色中
                                isRollCheck = false;
                                break;
                            }
                        }
                    }
                    if (isRollCheck == true)
                    {
                        //用户角色没有在限制角色中。在判断是否全选和指定
                        //2：判断是否全选
                        if (CheckAll.IsCheckRole == false)
                        {
                            //2：没有全选，判断是否是指定角色
                            var checkAllYes = false;
                            var roleYes = _contentRoleRepository.GetAll().Where(x => x.ContentId == content.Id && x.IsAllow == true).ToList();
                            if (roleYes.Count > 0)
                            {
                                //指定角色ID集合
                                List<long> roleYesId = new List<long>();
                                for (int i = 0; i < roleYes.Count; i++)
                                {
                                    roleYesId.Add(roleYes[i].RoleId);
                                }
                                foreach (var r in userRole)
                                {
                                    if (roleYesId.Contains(r.RoleId))
                                    {
                                        //用户角色，在指定角色中
                                        checkAllYes = true;
                                        break;
                                    }
                                }
                            }
                            if (checkAllYes == false)
                            {
                                isRollCheck = false;//本条内容没有全选角色，没有指定用户角色，没有角色权限
                            }
                        }
                        else { }
                    }
                   
                }
                #endregion
                #region 组织权限
                if (config.IsChenkDistrict == true)
                {
                    //1：判断是否是限制组织
                    var disNo = _contentDistrictRepository.GetAll().FirstOrDefault(x => x.DistrictId == userDis && x.ContentId == content.Id && x.IsAllow == false);
                    if (disNo != null)
                    {
                        //用户组织是限制组织
                        isDisCheck = false;
                    }
                    else
                    {
                        //默认是否全选
                        if (CheckAll.IsCheckDistrict == false)
                        {
                            //2：判断是否是指定组织
                            var disYes = _contentDistrictRepository.GetAll().FirstOrDefault(x => x.DistrictId == userDis && x.ContentId == content.Id && x.IsAllow == true);
                            if (disYes == null)
                            {
                                isDisCheck = false;
                            }

                        }
                        else { }

                    }
                }
                #endregion
                if(isUserCheck==true && isRollCheck==true && isDisCheck == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 根据传入的功能定义CODE获取内容，用户未读的，最新的3条
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDefineNewContents(string codes)
        {
            if (codes == "" || codes == null)
            {            
                return null;
            }
            //获取参数
            string[] codeArr = codes.Split(',');
            //根据功能定义CODE查询功能定义ID，ID加入list集合
            //把功能定义ID放入list集合
            List<long> listDefine = new List<long>();
            for (int i = 0; i < codeArr.Count(); i++)
            {
                string code = codeArr[i];
                var define = _defineRepository.GetAll().FirstOrDefault(x => x.Code == code);
                if (define != null)
                {
                    listDefine.Add(define.Id);
                }                              
            }

            //1:根据功能定义，查询出对应的数据
            //查询所有，启用的内容
            var contAll = _contentRepository.GetAll().Where(x => x.IsUse == true).OrderByDescending(a => a.CreationTime).ToList();
            //使用权限进行筛选，找出用户有权限的内容

            //循环contAll,加入符合功能定义和权限的项
            List<Domain.Content> contAllContent = new List<Domain.Content>();
            foreach (var temt in contAll)
            {
                //功能定义符合
                if (listDefine.Contains(temt.DefineId))
                {
                  
                    var isAllow = GetIsAllow(temt.Id);
                    //判断权限
                    if (isAllow)
                    {
                        contAllContent.Add(temt);
                    }
                }
            }


            //2:根据用户ID查询出阅读记录ContentReadLog，AbpSession.UserId.Value;
            var userID = AbpSession.UserId.Value;//用户ID
            var contUser = _contentReadLogRepository.GetAll().Where(x => x.UserId == userID).Distinct().ToList();
            //把阅读过的内容ID放入list集合
            List<long> listContent = new List<long>();
            for (int i = 0; i < contUser.Count(); i++)
            {
                if (!listContent.Contains(contUser[i].ContentId))
                {
                    listContent.Add(contUser[i].ContentId);
                }
            }

            //保存未读记录
            List<ContentModelNoRead> listUserModel = new List<ContentModelNoRead>();
            //3：判断contAll中的内容ID,取出没有阅读记录的数据
            foreach (var item in contAllContent)
            {
                if (!listContent.Contains(item.Id))
                {
                    //加入listUserModel未读记录
                    ContentModelNoRead model3 = new ContentModelNoRead();
                    model3.id = item.Id;
                    model3.title = item.Title;
                    model3.type = _contentTypeRepository.Get(item.DefineTypeId).Name;
                    model3.createTime = item.CreationTime;
                    model3.is_user = 0;
                    listUserModel.Add(model3);
                }
            }

            if (listUserModel.Count() == 0)
            {
                //没有未阅读记录,返回最新的三条
                var listNewModel= contAll.Select(x => new ContentModelNoRead { id = x.Id, title = x.Title,type= _contentTypeRepository.Get(x.DefineTypeId).Name, createTime = x.CreationTime, is_user = 1 });
                return listNewModel.ToList().Take(3);
            }
            else if (listUserModel.Count() <= 3)
            {
                //返回未读记录
                return listUserModel;
            }
            else if (listUserModel.Count() > 3)
            {
                //对listUserModel进行排序，返回最新的3条
                return listUserModel.OrderByDescending(a => a.createTime).Take(3);
            }
            else
            {
                //没有未阅读记录,返回最新的三条
                var listNewModel = contAll.Select(x => new ContentModelNoRead { id = x.Id, title = x.Title, type = _contentTypeRepository.Get(x.DefineTypeId).Name, createTime = x.CreationTime, is_user = 1 });
                return listNewModel.ToList().Take(3);
            }          
        }
        /// <summary>
        /// 根据文件ID查询一条文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileTemp GetContentsFile(long id)
        {
            var contFile = _filesRepository.Get(id);
            FileTemp file = new Dto.FileTemp();
            file.Id = contFile.Id;
            file.Length = contFile.Length;
            file.Name = contFile.TrueName;
            file.Uptime = contFile.UploadTime;
            file.Upurl = contFile.Url;
            return file;
        }
        /// <summary>
        /// 根据文件ID集合查询文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<object> GetContentsFileIds(string id)
        {
            var fileIdlist = id.Split(',');
            List<FileTemp> fileList = new List<FileTemp>();
            foreach (var item in fileIdlist)
            {
                if (item != "")
                {
                    var fileId = item.ToInt32();
                    var filecont = _filesRepository.Get(fileId);

                    FileTemp fileTp = new FileTemp
                    {
                        Id = filecont.Id,
                        Name=filecont.TrueName,
                        Length=filecont.Length,
                        Uptime=filecont.UploadTime,
                        Upurl=filecont.Url
                    };
                    fileList.Add(fileTp);
                }
            }
           
            return fileList;
        }

       

        /// <summary>
        /// 根据角色Id查询用户
        /// 
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public object GetUserByRoleId(int roleId)
        {
            
            //var role = await _roleManager.FindByIdAsync(roleId);
            //var user = _userRepository.GetAll();
            var userRole = _userRoleRepository.GetAll().Where(a => a.RoleId == roleId);
            List<UserTemp> roleList = new List<UserTemp>();

            //传入内容ID
            //var contentId = cid;
            ////查询该内容ID下指定用户ID集合，判断这个用户是否被选中
            //var contentUser = _contentUserRepository.GetAll().Where(a => a.ContentId == contentId && a.IsProhibit==0);
            //List<long> userList = new List<long>();
            //foreach(var item in contentUser)
            //{
            //    if (!userList.Contains(item.UserId))
            //    {
            //        userList.Add(item.UserId);
            //    }
            //}

            foreach (var item in userRole)
            {
                UserTemp ro = new UserTemp();
                var user = _userRepository.Get(item.UserId);
                ro.UserId = user.Id;
                ro.UserName = user.Name;
                ro.RoleId = item.RoleId;
                //if (userList.Contains(user.Id))
                //{
                //    ro.IsCheck = true;
                //}
                roleList.Add(ro);
            }
            return roleList;
        }

        public object GetUserByName(string uName)
        {
            var user = _userRepository.GetAll().FirstOrDefault(a => a.Name == uName);
            UserTemp u = new UserTemp();
            if (user != null)
            {
                u.UserId = user.Id;
                u.UserName = user.Name;
            }
            return u;
        }

        /// <summary>
        /// 根据用户名，或者工号模糊查询用户
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        public object GetUserByNameList(string uName)
        {
            var user = _userRepository.GetAll().Where(a => a.Name.Contains(uName));
            List<UserTemp> u = new List<UserTemp>();
            foreach (var item in user)
            {
                UserTemp ut = new UserTemp
                {
                    UserId = item.Id,
                    UserName = item.Name
                };
                u.Add(ut);
            }
            //加入工号模糊查询
            var login = _userRepository.GetAll().Where(x=>x.Surname.Contains(uName));
            foreach(var items in login)
            {
                UserTemp ul = new UserTemp
                {
                    UserId = items.Id,
                    UserName = items.Name
                };
                u.Add(ul);
            }
            return u;
        }

       

        public List<ContentRole> GetAllContentRole()
        {
            return _contentRoleRepository.GetAll().ToList();
        }

        public object GetUserByUserId(int uId)
        {
            var user = _userRepository.Get(uId);
            UserTemp ro = new UserTemp
            {
                UserId = user.Id,
                UserName = user.Name
            };
            return ro;
        }


       /// <summary>
       /// 获取指定组织树,根据内容ID确认组织是否被选中
       /// </summary>
       /// <returns></returns>
        public object GetDistrictParentTreeJson(long cid)
        {
            //查询出该内容关联的组织
            List<long> listDis = new List<long>();
            if (cid != 0)
            {
                var conDis = _contentDistrictRepository.GetAll().Where(x => x.ContentId == cid && x.IsAllow==true).ToList();
                for(int i=0;i<conDis.Count();i++)
                {
                    listDis.Add(conDis[i].DistrictId);
                }
            }
            var navDistrict = _districtRepository.GetAll().Where(a=>a.IsUse==true);
            var districtNodes = navDistrict.Select(s => new
            {
                id=s.Id,
                name=s.Name,
                open=false,
                Checked= listDis.Contains(s.Id),
                pId=s.ParentId

            }).ToList();
          
            return districtNodes;
        }

        /// <summary>
        /// 获取限制组织树,根据内容ID确认组织是否被选中
        /// </summary>
        /// <returns></returns>
        public object GetDistrictParentTreeJsonNo(long cid)
        {
            //查询出该内容关联的组织限制
            List<long> listDis = new List<long>();
            if (cid != 0)
            {
                var conDis = _contentDistrictRepository.GetAll().Where(x => x.ContentId == cid && x.IsAllow == false).ToList();
                for (int i = 0; i < conDis.Count(); i++)
                {
                    listDis.Add(conDis[i].DistrictId);
                }
            }
            var navDistrict = _districtRepository.GetAll().Where(a => a.IsUse == true);
            var districtNodes = navDistrict.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                Checked = listDis.Contains(s.Id),
                pId = s.ParentId

            }).ToList();

            return districtNodes;
        }





        #endregion

        #region APP方法

        /// <summary>
        /// 获取某定义下的所有内容类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ApiKeyValueBean> GetAllContentType(ApiRequestEntityBean request)
        {
            if (string.IsNullOrEmpty(request.id))
            {
                return null;
            }

            // 获取功能定义
            var defineInfo = _defineRepository.GetAll().FirstOrDefault(d => d.Code == request.id);

            if (defineInfo == null)
            {
                return null;
            }

            var keyValueList = new List<ApiKeyValueBean>();
            var typeList = _contentTypeRepository.GetAll().Where(c => c.DefineId == defineInfo.Id)
                .OrderBy(o => o.ShowOrder).ToList();

            foreach (var type in typeList)
            {
                var keyValue = new ApiKeyValueBean
                {
                    key = type.Id.ToString(),
                    value = type.Name
                };

                keyValueList.Add(keyValue);
            }

            return keyValueList;
        }

        /// <summary>
        /// 获取某定义下所有内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiPagingDataBean<ApiContentBean> GetAllContent(ApiRequestPageBean request)
        {
            if (request.currentPage <= 0)
            {
                return null;
            }

            var currentPage = request.currentPage;
            var pageSize = request.pageSize;
            var currentUserId = AbpSession.UserId;
            var retList = new List<ApiContentBean>();

            long typeId;
            string title;
            var typePara = request.searchKey.SingleOrDefault(s => s.key == "typeId");
            var titlePara = request.searchKey.SingleOrDefault(s => s.key == "TITLE");

            var contentQuery = _contentRepository.GetAll().Where(c => c.IsUse == true && !c.IsDeleted);

            if (typePara != null)
            {
                typeId = Convert.ToInt64(typePara.value);
                contentQuery = contentQuery.Where(c => c.DefineTypeId == typeId);
            }

            if (titlePara != null)
            {
                title = titlePara.value;
                contentQuery = contentQuery.Where(c => c.Title == title);
            }

            // 获取类型下所有ContentId
            var bannedContentIds = new List<long>();
            var userRoles = _userRoleRepository.GetAll().Where(u => u.UserId == currentUserId).Select(s => s.RoleId).ToList();
            var allContentIds = contentQuery.Select(s => s.Id).ToList();

            // 可看用户
            var userContentList = _contentUserRepository.GetAll().Where(u => allContentIds.Contains(u.ContentId)).ToList();
            var bannedUserContentIds = userContentList.Where(w => w.UserId == currentUserId && w.IsAllow == true)
                .Select(s => s.ContentId).Distinct().ToList();

            // 可看角色
            var roleContentList = _contentRoleRepository.GetAll().Where(r => allContentIds.Contains(r.ContentId)).ToList();
            var bannedRoleContentIds = roleContentList.Where(w => userRoles.Contains(w.RoleId) && w.IsAllow == true)
                .Select(s => s.ContentId).Distinct().ToList();

            bannedContentIds.AddRange(bannedUserContentIds);
            bannedContentIds.AddRange(bannedRoleContentIds);
            bannedContentIds = bannedContentIds.Distinct().ToList();

            var allContentList = contentQuery.Where(c => !bannedContentIds.Contains(c.Id))
                .OrderByDescending(o => o.CreationTime);

            var pagedContentList = allContentList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            foreach (var content in pagedContentList)
            {
                var bean = new ApiContentBean
                {
                    ID = content.Id,
                    DEFINE_TYPE_ID = content.DefineTypeId,
                    TITLE = content.Title,
                    SUMMARY = content.Summary,
                    INFO = content.Info,
                    IMAGE = content.Image,
                    PUBLISHER = _userRepository.FirstOrDefault(u => u.Id == content.CreatorUserId).UserName,
                    IS_IMPORT = content.IsImport,
                    IS_URGENT = content.IsUrgent,
                    CREATETIME = content.CreationTime,
                    ReadCount = _replyPraiseAppService.ContentReadContent(Convert.ToInt32(content.Id)),
                    ReviewCount = _replyPraiseAppService.ReplyContent(Convert.ToInt32(content.Id)),
                    ContentLikeCount = _replyPraiseAppService.ContentPraiseCount(Convert.ToInt32(content.Id))
                };

                var defineId = _contentTypeRepository.Get(content.DefineTypeId).DefineId;
                var defineConfig = _defineConfigRepository.GetAll().FirstOrDefault(c => c.DefineId == defineId);

                if (defineConfig != null)
                {
                    bean.IsDeleteAllowed = Convert.ToBoolean(defineConfig.IsDelete);
                    bean.IsLikeAllowed = Convert.ToBoolean(defineConfig.IsLike);
                    bean.IsReplyAllowed = Convert.ToBoolean(defineConfig.IsReoly);
                    bean.IsReplyFileAllowed = Convert.ToBoolean(defineConfig.IsReolyFile);
                    bean.IsReplyFloorAllowed = Convert.ToBoolean(defineConfig.IsReolyFloor);
                    bean.IsReplyFloorFileAllowed = Convert.ToBoolean(defineConfig.IsReolyFloorFile);
                    bean.IsShareAllowed = Convert.ToBoolean(defineConfig.IsShare);
                    bean.IsTextAllowed = Convert.ToBoolean(defineConfig.IsText);
                }

                retList.Add(bean);
            }

            var pageResult = new ApiPagingDataBean<ApiContentBean>
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalCount = allContentList.Count(),
                totalPage = allContentList.Count() % pageSize == 0 
                ? (allContentList.Count() / pageSize) : (allContentList.Count() / pageSize + 1),
                data = retList
            };

            return pageResult;
        }

        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiContentBean GetContentDetail(ApiRequestEntityBean request)
        {
            if (string.IsNullOrEmpty(request.id))
            {
                return null;
            }

            var contentId = Convert.ToInt64(request.id);
            var detail = _contentRepository.GetAll().FirstOrDefault(c => c.Id == contentId);

            if (detail != null)
            {
                // 增加阅读次数
                _replyPraiseAppService.ContentReadLog(Convert.ToInt32(request.id));

                var contentBean = new ApiContentBean
                {
                    ID = detail.Id,
                    DEFINE_TYPE_ID = detail.DefineTypeId,
                    TITLE = detail.Title,
                    SUMMARY = detail.Summary,
                    INFO = detail.Info,
                    IMAGE = detail.Image,
                    PUBLISHER = _userRepository.FirstOrDefault(u => u.Id == detail.CreatorUserId).UserName,
                    IS_IMPORT = detail.IsImport,
                    IS_URGENT = detail.IsUrgent,
                    CREATETIME = detail.CreationTime,
                    ReadCount = _replyPraiseAppService.ContentReadContent(Convert.ToInt32(detail.Id)),
                    ReviewCount = _replyPraiseAppService.ReplyContent(Convert.ToInt32(detail.Id)),
                    ContentLikeCount = _replyPraiseAppService.ContentPraiseCount(Convert.ToInt32(detail.Id))
                };

                var defineId = _contentTypeRepository.Get(detail.DefineTypeId).DefineId;
                var defineConfig = _defineConfigRepository.GetAll().FirstOrDefault(c => c.DefineId == defineId);

                if (defineConfig != null)
                {
                    contentBean.IsDeleteAllowed = Convert.ToBoolean(defineConfig.IsDelete);
                    contentBean.IsLikeAllowed = Convert.ToBoolean(defineConfig.IsLike);
                    contentBean.IsReplyAllowed = Convert.ToBoolean(defineConfig.IsReoly);
                    contentBean.IsReplyFileAllowed = Convert.ToBoolean(defineConfig.IsReolyFile);
                    contentBean.IsReplyFloorAllowed = Convert.ToBoolean(defineConfig.IsReolyFloor);
                    contentBean.IsReplyFloorFileAllowed = Convert.ToBoolean(defineConfig.IsReolyFloorFile);
                    contentBean.IsShareAllowed = Convert.ToBoolean(defineConfig.IsShare);
                    contentBean.IsTextAllowed = Convert.ToBoolean(defineConfig.IsText);
                }

                return contentBean;
            }

            return null;
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiPagingDataBean<ApiContentReviewBean> GetContentReviewList(ApiRequestPageBean request)
        {
            if (string.IsNullOrEmpty(request.id))
            {
                return null;
            }

            var contentId = Convert.ToInt64(request.id);
            var reviewId = Convert.ToInt64(request.id);
            var currentPage = request.currentPage;
            var pageSize = request.pageSize;

            var allReview = _contentReplyRepository.GetAll().Where(r => r.ContentId == contentId);
            var allReviewBean = new List<ApiContentReviewBean>();
            var topReviewList = allReview.Where(r => r.ParentId == 0).OrderByDescending(o => o.CreationTime)
                .Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            foreach (var review in topReviewList)
            {
                var subReviewCount = allReview.Where(a => a.ParentId == review.Id)
                    .ToList().Count;

                var reviewBean = new ApiContentReviewBean
                {
                    ID = review.Id,
                    CONTENT_ID = review.ContentId,
                    PARENT_ID = (long)review.ParentId,
                    INFO = review.Info,
                    REPLY_UID = review.ReolyUId,
                    ReplyUserName = review.ReplyUser.UserName,
                    CreateTime = review.CreationTime,
                    //ChildReview = GetChildReview(review.Id, allReview),
                    ChildReview = GetTopThreeReviewComment(review.Id, allReview),
                    ChildReviewNum = subReviewCount
                };

                allReviewBean.Add(reviewBean);
            }

            var pageResult = new ApiPagingDataBean<ApiContentReviewBean>
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalCount = allReview.Count(),
                totalPage = allReview.Count() % pageSize == 0 ? (allReview.Count() / pageSize) : (allReview.Count() / pageSize + 1),
                data = allReviewBean
            };

            return pageResult;
        }

        /// <summary>
        /// 分页获取评论的回复
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiPagingDataBean<ApiContentReviewBean> GetPageReviewComment(ApiRequestPageBean request)
        {
            if (request.currentPage <= 0 || string.IsNullOrEmpty(request.id))
            {
                return null;
            }

            var reviewId = Convert.ToInt64(request.id);
            var currentPage = request.currentPage;
            var pageSize = request.pageSize;
            var currentUserId = AbpSession.UserId;
            var reviewList = new List<ApiContentReviewBean>();

            // 暂不取第三层的回复
            var allReview = _contentReplyRepository.GetAll().Where(c => c.ParentId == reviewId)
                .OrderByDescending(o => o.CreationTime);

            var pagedReviews = allReview.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            foreach (var review in pagedReviews)
            {
                var subReviewCount = allReview.Where(a => a.ParentId == review.Id)
                    .ToList().Count;

                var reviewBean = new ApiContentReviewBean
                {
                    ID = review.Id,
                    CONTENT_ID = review.ContentId,
                    PARENT_ID = (long)review.ParentId,
                    INFO = review.Info,
                    REPLY_UID = review.ReolyUId,
                    ReplyUserName = review.ReplyUser.UserName,
                    CreateTime = review.CreationTime
                };

                reviewList.Add(reviewBean);
            }

            var pageResult = new ApiPagingDataBean<ApiContentReviewBean>
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalCount = allReview.Count(),
                totalPage = allReview.Count() % pageSize == 0 ? (allReview.Count() / pageSize) : (allReview.Count() / pageSize + 1),
                data = reviewList
            };

            return pageResult;
        }

        /// <summary>
        /// 获取子评论
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="allReview"></param>
        /// <returns></returns>
        private List<ApiContentReviewBean> GetChildReview(long reviewId, IQueryable<ContentReply> allReview)
        {
            var reviewList = new List<ApiContentReviewBean>();
            var reviews = allReview.Where(a => a.ParentId == reviewId).OrderByDescending(o => o.CreationTime);

            foreach (var childReview in reviews)
            {
                var bean = new ApiContentReviewBean
                {
                    ID = childReview.Id,
                    CONTENT_ID = childReview.ContentId,
                    PARENT_ID = (long)childReview.ParentId,
                    INFO = childReview.Info,
                    REPLY_UID = childReview.ReolyUId,
                    ReplyUserName = childReview.ReplyUser.UserName,
                    CreateTime = childReview.CreationTime,
                    ChildReview = GetChildReview(childReview.Id, allReview)
                };

                reviewList.Add(bean);
            }

            return reviewList;
        }

        /// <summary>
        /// 获取前三条回复
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="allReview"></param>
        /// <returns></returns>
        private List<ApiContentReviewBean> GetTopThreeReviewComment(long reviewId, IQueryable<ContentReply> allReview)
        {
            var reviewList = new List<ApiContentReviewBean>();
            var reviews = allReview.Where(a => a.ParentId == reviewId)
                .OrderByDescending(o => o.CreationTime).Take(3).ToList();

            foreach (var childReview in reviews)
            {
                var bean = new ApiContentReviewBean
                {
                    ID = childReview.Id,
                    CONTENT_ID = childReview.ContentId,
                    PARENT_ID = (long)childReview.ParentId,
                    INFO = childReview.Info,
                    REPLY_UID = childReview.ReolyUId,
                    ReplyUserName = childReview.ReplyUser.UserName,
                    CreateTime = childReview.CreationTime,
                };

                reviewList.Add(bean);
            }

            return reviewList;
        }

        /// <summary>
        /// 保存内容评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiContentReviewBean SaveContentReview(ApiRequestSaveEntityBean<ApiContentReviewBean> request)
        {
            var review = request.entity;
            var saveView = new ContentReply
            {
                ContentId = review.CONTENT_ID,
                ParentId = review.PARENT_ID,
                Info = review.INFO,
                ReolyUId = review.REPLY_UID,
                CreationTime = DateTime.Now,
            };

            review.ID = _contentReplyRepository.InsertOrUpdateAndGetId(saveView);
            review.CreateTime = saveView.CreationTime;

            return review;
        }

        /// <summary>
        /// 删除内容评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiErrorBean DeleteContentReview(ApiRequestEntityBean request)
        {
            var errorInfo = new ApiErrorBean();

            if (string.IsNullOrEmpty(request.id))
            {
                errorInfo.isError = true;
                errorInfo.message = "删除失败";

                return errorInfo;
            }

            var reviewId = Convert.ToInt64(request.id);
            var reviewInfo = _contentReplyRepository.FirstOrDefault(r => r.Id == reviewId);

            if (reviewInfo != null)
            {
                if (reviewInfo.ReolyUId.Equals(AbpSession.UserId) || reviewInfo.ReplyUser.UserName.Equals("admin"))
                {
                    _contentReplyRepository.Delete(reviewInfo);
                }
                else
                {
                    errorInfo.isError = true;
                    errorInfo.message = "无权限删除当前评论";

                    return errorInfo;
                }
            }
            else
            {
                errorInfo.isError = true;
                errorInfo.message = "评论不存在";

                return errorInfo;
            }

            errorInfo.isError = false;
            errorInfo.message = "删除成功";

            return errorInfo;
        }

        /// <summary>
        /// 点赞内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiErrorBean LikeContent(ApiRequestEntityBean request)
        {
            var errorInfo = new ApiErrorBean();

            if (string.IsNullOrEmpty(request.id))
            {
                errorInfo.isError = true;
                errorInfo.message = "点赞失败";

                return errorInfo;
            }

            var contentId = Convert.ToInt32(request.id);
            var count = _replyPraiseAppService.ContentPraise(contentId);

            errorInfo.isError = false;
            errorInfo.message = count.ToString();

            return errorInfo;
        }

        /// <summary>
        /// 点赞评论
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiErrorBean LikeContentReview(ApiRequestEntityBean request)
        {
            var errorInfo = new ApiErrorBean();

            if (string.IsNullOrEmpty(request.id))
            {
                errorInfo.isError = true;
                errorInfo.message = "评论点赞失败";

                return errorInfo;
            }

            var reviewId = Convert.ToInt32(request.id);
            _replyPraiseAppService.ReplyPraise(reviewId);

            var praiseLog = _replyPraiseLogRepository.GetAll().Where(a => a.ContentReplyId == reviewId);

            errorInfo.isError = false;
            errorInfo.message = praiseLog.Count().ToString();

            return errorInfo;
        }

        #endregion

        

    }
}
