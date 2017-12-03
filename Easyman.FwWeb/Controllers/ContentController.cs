using Easyman.Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Easyman.Base.Content;
using Easyman.Base.Content.Dto;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using Newtonsoft.Json;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Authorization.Roles;
using Easyman.Content;
using Easyman.Content.Dto;
using Easyman.Domain;
using EasyMan.Dtos;

namespace Easyman.FwWeb.Controllers
{
    public class ContentController : EasyManController
    {
        private readonly IUserAppService _userService;
        private readonly IModulesAppService _modulesService;
        private readonly IRoleAppService _roleAppService;
        private readonly RoleStore _roleStore;
        private readonly IContentAppService _contentAppService;
       
        private readonly IRepository<ContentReply, long> _contentReplyRepository;

        private readonly IContentTypeAppService _contentTypeAppService;
        private readonly IDefineAppService _defineAppService;
        private readonly IDistrictAppService _districtAppService;
        private readonly IPushwayAppService _pushwayAppService;
        private readonly IReplyPraiseAppService _replyPraiseAppService;

        private readonly IRepository<ContentReplyFile, long> _contentReplyFileRepository;

        public ContentController(IUserAppService userService, IModulesAppService modulesService, 
            IRoleAppService roleAppService, IContentAppService contentAppService, RoleStore roleStore, 
             IRepository<ContentReply, long> contentReplyRepository,
             IContentTypeAppService contentTypeAppService,
             IDefineAppService defineAppService,
            IDistrictAppService districtAppService,
            IPushwayAppService pushwayAppService,
             IReplyPraiseAppService replyPraiseAppService,
             IRepository<ContentReplyFile, long> contentReplyFileRepository

            )

        {
            _userService = userService;
            _modulesService = modulesService;
            _roleAppService = roleAppService;
            _contentAppService = contentAppService;
            _roleStore = roleStore;

            _contentReplyRepository = contentReplyRepository;
            _districtAppService = districtAppService;
            _pushwayAppService = pushwayAppService;
            _defineAppService = defineAppService;
            _contentTypeAppService = contentTypeAppService;
            _replyPraiseAppService = replyPraiseAppService;

            _contentReplyFileRepository = contentReplyFileRepository;
        }

        /// <summary>
        /// 内容定义列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Easyman.FwWeb.Views.Content.Define");
        }
        /// <summary>
        /// 内容定义添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ContentDefineInput model = new ContentDefineInput();
            return View("Easyman.FwWeb.Views.Content.AddDefine", model);
        }
        /// <summary>
        /// 内容定义编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult EditContentDefine(int navId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var define = _defineAppService.GetContentDefine(navId);
                var model = define == null ? new ContentDefineInput() : define.MapTo<ContentDefineInput>();
                return View("Easyman.FwWeb.Views.Content.AddDefine", model);
            }
        }

        public ActionResult ContentTypeIndex()
        {
            return View("Easyman.FwWeb.Views.Content.ContentType");
        }

        /// <summary>
        /// 内容类别添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddContentType()
        {
            ContentTypeInput model = new ContentTypeInput();
            ViewData["Define"] = _defineAppService.GetDefine();
            model.IsEdit = false;
            return View("Easyman.FwWeb.Views.Content.AddContentType", model);
        }

        /// <summary>
        /// 内容类别编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult EditContentType(int navId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var define = _contentTypeAppService.GetContentType(navId);
                ViewData["Define"] = _defineAppService.GetDefine();
                var model = define ?? new ContentTypeInput();
                if (define != null) model.PIdType = define.ParentId;
                model.IsEdit = true;
                return View("Easyman.FwWeb.Views.Content.AddContentType", model);
            }
        }

        public ActionResult GetDefineTree(int id, int? conntentTypeId)
        {
            if (conntentTypeId == null)
                conntentTypeId = 0;
            var result = _contentTypeAppService.GetContentTypeParentTreeJson(id, conntentTypeId.Value);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据功能定义ID获取用户，角色，组织权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDefineConfigCheck(int id)
        {

            var result = _defineAppService.GetContentDefine(id);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContentIndex()
        {
            return View("Easyman.FwWeb.Views.Content.ContentIndex");
        }
        /// <summary>
        /// 新增内容
        /// </summary>
        /// <returns></returns>
        public ActionResult AddContentIndex()
        {
            ContentIndexInput model = new ContentIndexInput();
            ViewData["Define"] = _defineAppService.GetDefine();
            model.Id = 0;
            #region 角色权限

            //指定角色权限展示
            var role = _roleStore.Query.ToList();
            List<RoleTemp> list = new List<RoleTemp>();
            foreach (var item in role)
            {
                RoleTemp temp = new RoleTemp();
                temp.Id = item.Id;
                temp.Name = item.DisplayName;
                temp.IsCheck = false;
                list.Add(temp);
            }
            model.Role = list;
            //限制角色权限展示
            var roleno = _roleStore.Query.ToList();
            List<RoleTemp> listNo = new List<RoleTemp>();
            foreach (var item in roleno)
            {
                RoleTemp temp = new RoleTemp();
                temp.Id = item.Id;
                temp.Name = item.DisplayName;
                temp.IsCheck = false;
                listNo.Add(temp);
            }
            model.RoleNo = list;
            #endregion

            #region 组织权限

            //指定组织权限展示
            var district = _districtAppService.GetAllDistrcit();
            List<DistrictTemp> dislist = new List<DistrictTemp>();
            foreach (var item in district)
            {
                DistrictTemp temp = new DistrictTemp();
                temp.Id = item.Id;
                temp.Name = item.Name;
                temp.IsCheck = false;
                dislist.Add(temp);
            }
            model.District = dislist;
            //限制组织权限展示
            var districtno = _districtAppService.GetAllDistrcit();
            List<DistrictTemp> dislistNo = new List<DistrictTemp>();
            foreach (var item in districtno)
            {
                DistrictTemp temp = new DistrictTemp();
                temp.Id = item.Id;
                temp.Name = item.Name;
                temp.IsCheck = false;
                dislistNo.Add(temp);
            }
            model.DistrictNo = dislistNo;
            #endregion
            //推送模式
            var push = _pushwayAppService.GetPushWay();
            List<PushTemp> Plist = new List<PushTemp>();
            foreach (var item in push)
            {
                PushTemp p = new PushTemp();
                p.Id = item.Id;
                p.Name = item.Name;
                p.IsCheck = false;
                Plist.Add(p);
            }
            model.Push = Plist;
            //新增的时候默认全选
            model.IsAllUser = true;
            model.IsAllRole = true;
            model.IsAllDistrict = true;
            return View("Easyman.FwWeb.Views.Content.AddContentIndex", model);
        }
        /// <summary>
        /// 编辑内容
        /// </summary>
        /// <param name="navId"></param>
        /// <returns></returns>
        public ActionResult EditContentIndex(int navId)
        {
            ViewData["Define"] = _defineAppService.GetDefine();
            var content = _contentAppService.GetContent(navId);

            #region 角色权限处理
            //指定角色
            var role = content.RoleListId.Split(',');
            var roleList = _roleStore.Query.ToList();
            List<RoleTemp> list = new List<RoleTemp>();
            foreach (var item in roleList)
            {
                RoleTemp temp = new RoleTemp();
                temp.Id = item.Id;
                temp.Name = item.DisplayName;
                    foreach (var r in role)
                    {
                    if(r != "") {
                        var rId = Convert.ToInt32(r);
                        if (rId == item.Id)
                            temp.IsCheck = true;
                    }
                }
                list.Add(temp);
            }
            content.Role = list;
            //限制角色
            var roleno = content.RoleListIdNo.Split(',');
            var roleListNo = _roleStore.Query.ToList();
            List<RoleTemp> listNo = new List<RoleTemp>();
            foreach (var item in roleListNo)
            {
                RoleTemp temp = new RoleTemp();
                temp.Id = item.Id;
                temp.Name = item.DisplayName;               
                foreach (var r in roleno)
                {
                    if (r != "")
                    {
                        var rId = Convert.ToInt32(r);
                        if (rId == item.Id)
                            temp.IsCheck = true;
                    }
                }

                listNo.Add(temp);
            }
            content.RoleNo = listNo;
            #endregion

            #region 组织权限处理
            //指定组织
            var district = content.DistrictListId.Split(',');
            var districtList = _districtAppService.GetAllDistrcit();
            List<DistrictTemp> dislist = new List<DistrictTemp>();
            foreach (var item in districtList)
            {
                DistrictTemp temp = new DistrictTemp();
                temp.Id = item.Id;
                temp.Name = item.Name;
                foreach (var r in district)
                {
                    if (r != "")
                    {
                        var rId = Convert.ToInt32(r);
                        if (rId == item.Id)
                            temp.IsCheck = true;
                    }
                }
                dislist.Add(temp);
            }
            content.District = dislist;
            //限制组织
            var districtno = content.DistrictListIdNo.Split(',');
            var districtListNo = _districtAppService.GetAllDistrcit();
            List<DistrictTemp> dislistNo = new List<DistrictTemp>();
            foreach (var item in districtListNo)
            {
                DistrictTemp temp = new DistrictTemp();
                temp.Id = item.Id;
                temp.Name = item.Name;
                foreach (var r in districtno)
                {
                    if (r != "")
                    {
                        var rId = Convert.ToInt32(r);
                        if (rId == item.Id)
                            temp.IsCheck = true;
                    }
                }

                dislistNo.Add(temp);
            }
            content.DistrictNo = dislistNo;
            #endregion
            #region 上传附件
            
          
            #endregion
            //推送模式
            var push = _pushwayAppService.GetPushWay();
            List<PushTemp> Plist = new List<PushTemp>();
            foreach (var item in push)
            {
                var pushId = Convert.ToInt32(item.Id);
                PushTemp p = new PushTemp();
                p.Id = item.Id;
                p.Name = item.Name;
                p.IsCheck = _pushwayAppService.IsPushWay(pushId, content.Id);
                Plist.Add(p);
            }
            content.Push = Plist;
           
            content.IsEdit = true;
            return View("Easyman.FwWeb.Views.Content.AddContentIndex", content);
        }

        public ActionResult GetUserByRoleId(int roleId)
        {
            var result = _contentAppService.GetUserByRoleId(roleId);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserByName(string uName)
        {
            var result = _contentAppService.GetUserByName(uName);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserBUserId(int uId)
        {
            var result = _contentAppService.GetUserByUserId(uId);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetUserByNameList(string name)
        {
            var result = _contentAppService.GetUserByNameList(name);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看内容
        /// </summary>
        /// <param name="navId"></param>
        /// <returns></returns>
        public ActionResult ContentInfo(int navId)
        {
            var model = _contentAppService.GetContent(navId);
            model.Reply = _replyPraiseAppService.GetContentReply(navId);

            //if (model.Reply.Count > 12)
            //{
            //    model.IsAllReplyNumber = true;
            //    model.ReplyNumber = model.Reply.Take(12).ToList();
            //}
            //else
            //{
            //    model.IsAllReplyNumber = false;
            //    model.ReplyNumber = null;
            //}

            //根据评论，查询出评论的集合

            foreach (var item in model.Reply)
            {
                if (item.ChildContentReply != null)
                    foreach (var tem in item.ChildContentReply)
                    {
                        if (tem.ParentId != null)
                        {
                            var reply = _contentReplyRepository.FirstOrDefault(a => a.Id == tem.ParentId);
                            tem.ParentName = reply.ReplyUser.Name;
                        }
                        tem.IsReolyOrLike = _replyPraiseAppService.IsReplyOrLike(Convert.ToInt32(tem.Id));
                    }
            }
            _replyPraiseAppService.ContentReadLog(navId);
            if (model.CreatorUserId != null)
            {
                long uId = (long)model.CreatorUserId;
                var user = _userService.GetUser(uId);
                model.CreateName = user.Name;
            }
            model.ReadContent = _replyPraiseAppService.ContentReadContent(navId);
            model.ReplyCount = _replyPraiseAppService.ReplyContent(navId);
            model.ContentPraiseCount = _replyPraiseAppService.ContentPraiseCount(navId);
            model.IsOrLike = _replyPraiseAppService.IsOrLike(navId);
            var lonigId = AbpSession.UserId.Value;
            model.LonigName = _userService.GetUser(lonigId).Name;
            return View("Easyman.FwWeb.Views.Content.ContentInfo", model);
        }

       /// <summary>
       /// 发表评论
       /// </summary>
       /// <param name="contentId"></param>
       /// <param name="replyInfo"></param>
       /// <param name="fileIds"></param>
       /// <param name="replyId"></param>
       /// <returns></returns>
        public ActionResult CareatReply(int contentId, string replyInfo,string fileIds, int? replyId)
        {
            var result = _replyPraiseAppService.CerateReply(contentId, replyInfo, fileIds, replyId);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 内容点赞
        /// </summary>
        /// <returns></returns>
        public ActionResult ContentPraise(int contentId)
        {
            //var num = 0;
            try
            {
                _replyPraiseAppService.ContentPraise(contentId);
                //num = _contentAppService.ContentPraiseCount(contentId);
            }
            catch (Exception)
            {
                return Json(new { data = "no" }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { data = "ok", PraiseNum = num }, JsonRequestBehavior.AllowGet);
            return Json(new { data = "ok" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetContentPraise(int contentId)
        {
            var num = 0;
            var isOrLike = false;
            try
            {
                num = _replyPraiseAppService.ContentPraiseCount(contentId);
                isOrLike = _replyPraiseAppService.IsOrLike(contentId);
            }
            catch (Exception)
            {
                return Json(new { data = "no" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "ok", PraiseNum = num, isOrLike = isOrLike }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 评论点赞
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplyPraise(int replyId)
        {
            //var num = 0;
            try
            {
                _replyPraiseAppService.ReplyPraise(replyId);
                //num = _contentAppService.ReplyPraiseCount(replyId);
            }
            catch (Exception)
            {
                return Json(new { data = "no" }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { data = "ok", PraiseNum = num }, JsonRequestBehavior.AllowGet);
            return Json(new { data = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReplyPraise(int replyId)
        {
            var num = 0;
            var isOrLike = false;
            try
            {
                num = _replyPraiseAppService.ReplyPraiseCount(replyId);
                isOrLike = _replyPraiseAppService.IsReplyOrLike(replyId);
            }
            catch (Exception)
            {
                return Json(new { data = "no" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "ok", PraiseNum = num, isOrLike = isOrLike }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewIndex(string code = "")
        {
            ContentIndexSearchInput model = new ContentIndexSearchInput {Code = code};
            return View("Easyman.FwWeb.Views.Content.NewContentIndex", model);
        }

        public ActionResult NewIndexSearch(int pageIndex, string searchName = "", string code = "")
        {
            ContentIndexSearchInput input = new ContentIndexSearchInput
            {
                Page = new Pager()
                {
                    PageIndex = pageIndex,
                    PageSize = 10
                },
                Order = new Order()
                {
                    Name = "Id",
                    Type = "Asc"
                }
            };
            input.SearchName = searchName;
            input.Code = code;
            var content = _contentAppService.NewSearchContent(input);
            
            return PartialView("Easyman.FwWeb.Views.Content.NewContentSearch", content);
        }

        public string GetTotalPage(int pageIndex, string searchName = "", string code = "")
        {
            ContentIndexSearchInput input = new ContentIndexSearchInput
            {
                Page = new Pager()
                {
                    PageIndex = 1,
                    PageSize = 10
                },
                Order = new Order()
                {
                    Name = "Id",
                    Type = "Asc"
                }
            };
            input.SearchName = searchName;
            input.Code = code;
            var content = _contentAppService.NewSearchContent(input);
            var obj = new { num = content.Page.TotalPage };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        #region 推送模式


        public ActionResult AddPushWay()
        {
            var data = new PushWayInput();
          
            return View("Easyman.FwWeb.Views.Content.AddPushWay", data);
        }

        //推送模式新增或者编辑
        public ActionResult EditPushWay(long? navId)
        {
            var data = new PushWayInput();
            if (navId != null)
            {
                data = _pushwayAppService.GetPushway(navId.Value);
            }
            return View("Easyman.FwWeb.Views.Content.AddPushWay", data);
        }
        #endregion
    }
}
