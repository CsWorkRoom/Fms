using Abp.Domain.Repositories;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Users;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Threading.Tasks;
using Easyman.Managers;
using EasyMan.Dtos;
using EasyMan;
using Abp.AutoMapper;
using System.Globalization;
using Abp.Domain.Uow;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Castle.Core;
using System.Linq.Expressions;
using EasyMan.EasyQuery;
using Abp.UI;

namespace Easyman.Sys
{
    public class ModulesAppService : EasymanAppServiceBase, IModulesAppService
    {
        #region 初始化

        private readonly IRepository<Module, long> _moduleRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly UrlHelper _urlHelper;
        private readonly ModuleManager _moduleManage;

        public ModulesAppService(ModuleManager moduleManage, IRepository<Module, long> moduleRepository, IAuthorizationService authorizationService)
        {
            _moduleRepository = moduleRepository;
            _authorizationService = authorizationService;
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            _moduleManage = moduleManage;
        }

        #endregion

        #region 公有方法
        public IEnumerable<MenuItem> GetNavigationByCurrentUser()
        {
            var user = GetCurrentUserAsync().Result;
            var parentMenu = _moduleRepository.GetAll().Where(x => x.ParentId == null).Select(x => new MenuItem()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Icon = x.Icon,
                Url = x.Url,
                ParentId = x.ParentId
            }).FirstOrDefault();
            var currentNav = new Module();
            var menuList = new List<MenuItem>();
            return GetChildMenu(parentMenu, currentNav, user).ToList();
        }

        public NavigationSerachOutput GetNavsSearch(NavigationSerachInput input)
        {
            var parentSearch = input.SearchList.FirstOrDefault(f => f.Name == "ParentName");

            if (parentSearch != null)
            {
                input.SearchList.Remove(parentSearch);
                parentSearch.Name = "Parent.Name";
                input.SearchList.Add(parentSearch);
            }

            var applicationType = new SearchFilter
            {
                Name = "ApplicationType",
                Value = "APP",
                Operator = OperatorType.NotEqual,
                TypeString = "string",
                TValue = "APP"
            };
            input.SearchList.Add(applicationType);

            var rowCount = 0;
            var navs = _moduleManage.Query.SearchByInputDto(input, out rowCount);
            var outPut = new NavigationSerachOutput
            {
                Datas = navs.ToList().Select(s => s.MapTo<NavigationOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        public NavigationSerachOutput GetAppMenuSearch(NavigationSerachInput input)
        {
            var parentSearch = input.SearchList.FirstOrDefault(f => f.Name == "ParentName");

            if (parentSearch != null)
            {
                input.SearchList.Remove(parentSearch);
                parentSearch.Name = "Parent.Name";
                input.SearchList.Add(parentSearch);
            }

            var applicationType = new SearchFilter
            {
                Name = "ApplicationType",
                Value = "APP",
                Operator = OperatorType.Equal,
                TypeString = "string",
                TValue = "APP"
            };
            input.SearchList.Add(applicationType);

            var rowCount = 0;
            var navs = _moduleManage.Query.SearchByInputDto(input, out rowCount);
            var outPut = new NavigationSerachOutput
            {
                Datas = navs.ToList().Select(s => s.MapTo<NavigationOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        public Task<IEnumerable<object>> GetNavTreeJson(string applicationType = "")
        {
            return _moduleManage.GetNavTreeList(applicationType);
        }

        public Module GetNavigation(int id)
        {
            try
            {
                return _moduleRepository.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        public Module GetNavigation(Expression<Func<Module, bool>> predicate)
        {
            return _moduleRepository.FirstOrDefault(predicate);
        }

        public Task< List<Module>> GetModuleList(Expression<Func<Module, bool>> predicate)
        {
            return _moduleRepository.GetAllListAsync(predicate);
        }

        public Module GetModuleByUrlAndCode(string url)
        {
            var urlPaArr = url.Split(new[] { '?', '&' });
            var codes = urlPaArr.FirstOrDefault(p => p.ToLower().Contains("code"));
            var rootUrl = urlPaArr[0];

            //p => p.Url.ToLower().Contains(\"" + urlPaArr[0] + "\")&&p.Url.Split(new[] { '?', '&' }).Skip(1).Intersect(" + urlPaArr.Skip(1) + ").Any(p=>p.ToLower().Contains(\"code\"))
            //return _moduleRepository.GetAllList(p => p.Url.ToLower().Contains(rootUrl) &&p.Url.Split(new[] { '?', '&' }).Skip(1).Intersect(urlPaArr.Skip(1)).Any(x=>x.ToLower().Contains("code")));
            var mdList = _moduleRepository.GetAllList(p => p.Url.Contains(rootUrl + "?" + codes));

            int num = 0;
            Module module = null;

            if (mdList != null && mdList.Count > 0)
            {
                if (mdList.Count > 1)
                {
                    foreach (var md in mdList)
                    {
                        int curNum = md.Url.Split(new[] { '?', '&' }).Skip(1).Intersect(urlPaArr.Skip(1)).Count();
                        if (curNum > num)
                        {
                            num = curNum;
                            module = md;
                        }
                    }
                }
                else
                {
                    module = mdList[0];
                }
            }

            return module;
        }

        public List<ModuleEvent> GetModuleEventList(Expression<Func<ModuleEvent, bool>> predicate)
        {
            return _moduleManage.GetModuleEventList(predicate);
        }

        public IEnumerable<RoleModuleEvent> GetRoleModuleEvent(long eventId)
        {
            return _moduleManage.GetRoleModuleEvent(eventId);
        }

        public Task<IEnumerable<object>> GetNavTreeJsonByRoleId(long roleId)
        {
            var navRoles = _moduleManage.GetNavigationRolesByRoleId(roleId);

            var navigationRoles = navRoles as RoleModule[] ?? navRoles.ToArray();
            var navIds = navigationRoles.Any() ? navigationRoles.Select(s => s.ModuleId.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => a + "," + b) : "";

            return _moduleManage.GetNavTreeList(null, navIds);
        }

        public Task<IEnumerable<object>> GetNavTreeJsonByRoleIdForModule(long roleId)
        {
            var navRoles = _moduleManage.GetNavigationRolesByRoleId(roleId);
            var roleModuleEvent = _moduleManage.GetEventModileEventId(roleId).ToList();

            var navigationRoles = navRoles as RoleModule[] ?? navRoles.ToArray();
            var navIds = navigationRoles.Any() ? navigationRoles.Select(s => s.ModuleId.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => a + "," + b) : "";
            return _moduleManage.GetNavTreeListForModule(roleModuleEvent, navIds);
        }

        [UnitOfWork]
        public void SaveNavigationEdit(NavigationInput input)
        {
            var nav = _moduleManage.GetNavigation(input.Id) ?? new Module();
            nav.Name = input.Name;
            nav.Icon = input.Icon;
            nav.Code = input.Code;
            nav.ParentId = input.ParentId;
            nav.Url = input.Url;
            nav.ShowOrder = input.ShowOrder;
            nav.TenantId = input.TenantId.HasValue ? input.TenantId.Value : 1;
            nav.RoleModule = nav.RoleModule ?? new List<RoleModule>();
            nav.IsUse = input.IsUse;//是否启用

            var navId = _moduleManage.SaveOrUpdateNavigation(nav);
            _moduleManage.SetRoles(navId, input.RoleIds);
        }

        [UnitOfWork]
        public void SaveAppMenuEdit(NavigationInput input)
        {
            var nav = _moduleManage.GetNavigation(input.Id) ?? new Module();
            nav.Name = input.Name;
            nav.Icon = input.Icon;
            nav.Code = input.Code;
            nav.ApplicationType = "APP";
            nav.ParentId = input.ParentId;
            nav.Url = input.Url;
            nav.ShowOrder = input.ShowOrder;
            nav.TenantId = input.TenantId.HasValue ? input.TenantId.Value : 1;
            nav.RoleModule = nav.RoleModule ?? new List<RoleModule>();
            nav.IsUse = input.IsUse;//是否启用

            var navId = _moduleManage.SaveOrUpdateNavigation(nav);
            _moduleManage.SetRoles(navId, input.RoleIds);
        }

        public void DeletePost(EntityDto<long> input)
        {
            try
            {
                _moduleRepository.Delete(x => x.Id == input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
        /// <summary>
        /// 校验是否具有页面权限
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool ValidateUrlRole(string url)
        {
            url = url.ToLower().Replace("//","/");//转换为小写
            var curRoles = GetCurUser().Roles.Select(p => p.RoleId);//当前用户角色

            #region 先验证模块权限
            Expression<Func<Module, bool>> expA = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url.ToLower().Contains(\"" + url + "\")");
            Expression<Func<Module, bool>> exp = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url.ToLower().Contains(\"" + url.Split('?')[0] + "\")");
            //Expression<Func<Module, bool>> exp = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url==\"" + curUrl+"\"");

            var moduleList = new List<Module>();
            moduleList = GetModuleList(expA).GetAwaiter().GetResult();
            if (moduleList == null || moduleList.Count == 0)
            {
                moduleList= GetModuleList(exp).GetAwaiter().GetResult();
                if (moduleList != null && moduleList.Count > 0)
                {
                    var inArr = url.Split(new[] { '?', '&' });
                    foreach (var md in moduleList)
                    {
                        var curArr = md.Url.ToLower().Split(new[] { '?', '&' });
                        if (inArr[0] == curArr[0] && inArr.Intersect(curArr).Count() == curArr.Length)
                        {
                            if (curRoles.Intersect(md.RoleModule.Select(p => p.RoleId)).Count() > 0)
                            {
                                return true;//用户角色和模版角色有交集时返回真
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var md in moduleList)
                {
                    //严格校验模块页（url路径及参数后缀必须一致）
                    if (url == md.Url.Substring(md.Url.Length - url.Length).ToLower())
                    {
                        if (curRoles.Intersect(md.RoleModule.Select(p => p.RoleId)).Count() > 0)
                        {
                            return true;//用户角色和模版角色有交集时返回真
                        }
                    }
                }
            }

             
            #endregion

            #region 再验证事件权限
            //url = url.Split('?')[0];
            Expression<Func<ModuleEvent, bool>> mdEvexp = StringToLambda.LambdaParser.Parse<Func<ModuleEvent, bool>>("p=>p.Url.ToLower().Contains(\"" + url.Split('?')[0] + "\")");
            var mdEvList = GetModuleEventList(mdEvexp);
            if (mdEvList != null && mdEvList.Count > 0)
            {
                var inArr = url.Split(new[] { '?', '&' });

                foreach (var mdEv in mdEvList)
                {
                    var curEvUrl = mdEv.Url.ToLower().Replace("~/", "");
                    if (curEvUrl.IndexOf('/') == 0)
                        curEvUrl = curEvUrl.Substring(1);
                    var curArr = curEvUrl.Split(new[] { '?', '&' });
                    if (curArr[0] == inArr[0] && inArr.Intersect(curArr).Count() == curArr.Length)
                    {
                        if (curRoles.Intersect(GetRoleModuleEvent(mdEv.Id).Select(p => Convert.ToInt32(p.RoleId))).Count() > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            #endregion

            return false;
        }
        /// <summary>
        /// 根据url获取归属的module
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Module GetModuleByUrl(string url)
        {
            var module = new Module();
            url = url.ToLower();//转换为小写

            Expression<Func<Module, bool>> exp = StringToLambda.LambdaParser.Parse<Func<Module, bool>>("p=>p.Url.ToLower().Contains(\"" + url + "\")");
            var moduleList = GetModuleList(exp).GetAwaiter().GetResult();

            if (moduleList != null && moduleList.Count > 0)
            {
                var inArr = url.Split(new[] { '?', '&' });
                foreach (var md in moduleList)
                {
                    var curArr = md.Url.ToLower().Split(new[] { '?', '&' });
                    if (inArr.Length == curArr.Length && inArr.Intersect(curArr).Count() == inArr.Length)
                    {
                        module = md;
                    }
                }
            }
            return module;
        }
        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public Users.User GetCurUser()
        {
            return GetCurrentUserAsync().Result;
        }

        #endregion 

        #region 私有方法

        private IEnumerable<MenuItem> GetChildMenu(MenuItem parent, Module current, User currUser)
        {
            var navList = _moduleRepository.GetAll()
                           .Where(a => a.Parent != null && a.Parent.Id == parent.Id&&(a.IsUse==null||(a.IsUse!=null&& a.IsUse.Value)))
                           .OrderBy(a => a.ShowOrder).OrderBy(a => a.ShowOrder).MapTo<List<MenuItem>>().ToList();


            return navList
                .Where(nav => _authorizationService.TryCheckAccess(Rolession.For(nav.Code), currUser))
                .Select(nav =>
                {
                    var childMenus = GetChildMenu(nav, current, currUser).ToList();
                    return new MenuItem
                    {
                        Id = nav.Id,
                        ParentId = nav.ParentId,
                        Items = childMenus,
                        Name = nav.Name,
                        Code = nav.Code,
                        Icon = nav.Icon,
                        Url = string.IsNullOrWhiteSpace(nav.Url) ? "" : _urlHelper.Content(nav.Url),
                        Selected = nav.Id == current.Id || childMenus.Any(a => a.Selected)
                    };
                });
        }

        #endregion
    }
}
