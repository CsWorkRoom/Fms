using Abp.Domain.Repositories;
using Easyman.Domain;
using EasyMan;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Easyman.Managers
{
    public class ModuleManager : EasyManDomainService<Module,long>
    {
        #region 初始化

        private readonly IRepository<Module,long> _moduleRepository;
        private readonly IRepository<RoleModule, long> _rolemoduleRepository;
        private readonly IRepository<Analysis,long> _analysisRepository;
        private readonly IRepository<ModuleEvent, long> _moduleeventRepository;
        private readonly IRepository<RoleModuleEvent, long> _rolemoduleeventRepository;

        public ModuleManager(IRepository<Module, long> moduleRepository,
        IRepository<RoleModule, long> rolemoduleRepository,
        IRepository<Analysis, long> analysisRepository,
        IRepository<ModuleEvent, long> moduleeventRepository,
        IRepository<RoleModuleEvent, long> rolemoduleeventRepository)
            : base(moduleRepository)
        {
            _moduleRepository = moduleRepository;
            _rolemoduleRepository = rolemoduleRepository;
            _analysisRepository = analysisRepository;
            _moduleeventRepository = moduleeventRepository;
            _rolemoduleeventRepository = rolemoduleeventRepository;
        }

        #endregion

        #region 公有方法

        public async Task<IEnumerable<object>> GetNavTreeList(string applicationType, string chekedIds = "")
        {
            var checkIdList = chekedIds.Split(',');
            var navs = await _moduleRepository.GetAllListAsync();

            if (string.IsNullOrEmpty(applicationType))
            {
                applicationType = null;
            }

            var navNodes = navs.Where(x => x.ApplicationType == applicationType).Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                Checked = checkIdList.Contains(s.Id.ToString(CultureInfo.InvariantCulture)),
                pId = s.ParentId,
                iconSkin = s.ChildModule.Any() ? "root" : "menu"
            }).ToList();

            return navNodes;
        }

        public async Task<IEnumerable<object>> GetNavTreeListForModule(List<RoleModuleEvent> rmeList,string chekedIds = "")
        {
            var checkIdList = chekedIds.Split(',');
            var navs = await _moduleRepository.GetAllListAsync();

            var navNodes = navs.Select(s => new treeNode
            {
                id = s.Id.ToString(),
                name = s.Name,
                Checked = checkIdList.Contains(s.Id.ToString(CultureInfo.InvariantCulture)),
                pId = s.ParentId.ToString(),
                iconSkin = "root",
                isParent= true,
                url=s.Url
            }).ToList();

            var analysList = _analysisRepository.GetAll().ToList();
            var moduleEventList = _moduleeventRepository.GetAll().ToList();
            List<treeNode> eventList = new List<treeNode>();

            foreach (var nav in navNodes)
            {
                if (nav.url!=null)
                {
                    var temp = GetEventByUrl(nav.url, analysList, moduleEventList).Select(s => new treeNode
                    {
                        id = "event"+s.Id,
                        name = s.EventName,
                        Checked = rmeList.Where(x=>x.EventId==s.Id&&x.ModuleId==Convert.ToInt64( nav.id)).Count()>0,
                        pId = nav.id,
                        iconSkin = "menu",
                        isParent = false,
                        url = ""
                    });
                    if (temp.Any())
                        eventList.AddRange(temp);
                }
            }

            navNodes.AddRange(eventList);

            return navNodes;
        }

        public Module GetNavigation(long id)
        {
            return _moduleRepository.FirstOrDefault(a => a.Id == id);
        }

        public Module GetNavigation(string code)
        {
            return _moduleRepository.FirstOrDefault(a => a.Code == code);
        }

        public List<ModuleEvent> GetModuleEventList(Expression<Func<ModuleEvent, bool>> exp)
        {
            return _moduleeventRepository.GetAllList(exp);
        }

        public ModuleEvent SaveModuleEvent(ModuleEvent mdEv)
        {
            return _moduleeventRepository.InsertOrUpdate(mdEv);
        }

        public async Task<Module> GetNavigationAsync(int id)
        {
            return await _moduleRepository.FirstOrDefaultAsync(f => f.Id == id);
        }

        public IEnumerable<RoleModule> GetNavigationRolesByRoleId(long roleId)
        {
            return _rolemoduleRepository.GetAll().Where(w => w.RoleId == roleId);
        }
        public IEnumerable<RoleModuleEvent> GetEventModileEventId(long roleId)
        {
            return _rolemoduleeventRepository.GetAll().Where(x => x.RoleId == roleId);
        }

        public IEnumerable<RoleModuleEvent> GetRoleModuleEvent(long eventId)
        {
            return _rolemoduleeventRepository.GetAll().Where(x => x.EventId == eventId);
        }

        public long SaveOrUpdateNavigation(Module navigation)
        {
            if (_moduleRepository.GetAll().Any(a => a.Id != navigation.Id && a.Code == navigation.Code))
            {
                throw new Exception("编码重复");
            }
            else
            {
                //新增或者更新菜单
                var id = _moduleRepository.InsertOrUpdateAndGetId(navigation);
                CurrentUnitOfWork.SaveChanges();

                navigation.PathId = navigation.Parent == null
                    ? "-{0}-".FormatWith(navigation.Id)
                    : navigation.Parent.PathId+ id + "-";

                _moduleRepository.Update(navigation);
                CurrentUnitOfWork.SaveChanges();


                return id;
            }
        }

        public void SetRoles(long navId, string roleIds)
        {
            var nav = GetNavigation(navId);
            var roleIdList = roleIds.HasValue() ? roleIds.Split(',') : new string[] { };


            foreach (var navRole in nav.RoleModule.ToList())
            {
                var role = navRole.Role;

                if (roleIdList.All(roleId => roleId != role.Id.ToString(CultureInfo.InvariantCulture)))
                {
                    _rolemoduleRepository.Delete(navRole);
                }
            }


            //Add to added roles
            foreach (var roleId in roleIdList)
            {
                var all = nav.RoleModule.All(ur => ur.RoleId != roleId.ToInt32(0));
                if (all)
                {
                    _rolemoduleRepository.Insert(new RoleModule(navId, roleId.ToInt32(0)));
                }
            }
        }

        public void SetEvent(int roleId, string eventIds)
        {
            try
            {
                var oldEventRoles = _rolemoduleeventRepository.GetAllList(w => w.RoleId == roleId);
                var newEventIdList = eventIds.HasValue() ?
                    eventIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) :
                    new string[0];

                foreach (var navEvent in oldEventRoles)
                {
                    if (newEventIdList.Select(x =>new {moduleId= x.Split('|')[1].ToInt32(),eventId= x.Split('|')[0].ToInt32() } ).Where(p => p.eventId == navEvent.EventId&&p.moduleId== navEvent.ModuleId).Count()==0)
                    {
                        _rolemoduleeventRepository.Delete(navEvent);
                    }
                }
                foreach (var eventId in newEventIdList)
                {
                    if (oldEventRoles.Where(a => a.EventId == eventId.Split('|')[0].ToInt32()&&a.ModuleId== eventId.Split('|')[1].ToInt32()).Count()==0)
                    {
                        _rolemoduleeventRepository.Insert(new RoleModuleEvent()
                        {
                            ModuleId = eventId.Split('|')[1].ToInt32(),
                            RoleId = roleId,
                            EventId = eventId.Split('|')[0].ToInt32()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 删除一条事件EM_MODULE_EVENT
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="code"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public bool DeleteEvent(long analysisId,string code,long sourceId)
        {
            try
            {
                var oldmdEvList = _moduleeventRepository.GetAllList(p => p.SourceTableId == sourceId &&
                 p.AnalysisId == analysisId &&
                 p.Code == code);
                foreach (var mdEv in oldmdEvList)
                {
                    var oldRoleMdEvList = _rolemoduleeventRepository.GetAllList(p => p.EventId == mdEv.Id);
                    foreach (var roleEv in oldRoleMdEvList)
                    {
                        _rolemoduleeventRepository.Delete(roleEv);
                    }
                    _moduleeventRepository.Delete(mdEv);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SaveEvent(ModuleEvent ev)
        {
            _moduleeventRepository.InsertOrUpdate(ev);
        }

        #endregion

        #region 私有方法

        private IEnumerable<ModuleEvent> GetEventByUrl(string url,List<Analysis> analys, List<ModuleEvent> modulement)
        {
            var data = new UrlFormat(url);
            var analysis = analys.FirstOrDefault(x => x.Url == data.Url);
            if (analysis == null)
                return new List<ModuleEvent>();
            var moduleEvent = modulement.Where(x => x.AnalysisId == analysis.Id && x.Code == data.Code);
            return moduleEvent;
        }

        private class treeNode
        {
            public string id { get; set; }
            public string name { get; set; }
            public bool Checked { get; set; }
            public string pId { get; set; }
            public string iconSkin { get; set; }
            public string url { get; set; }
            public bool isParent { get; set; }
        }

        public class UrlFormat
        {
            public string Url { get; set; }

            public string Code { get; set; }

            public UrlFormat(string action)
            {
                string[] splArr= { "?", "&" };
                var arr = action.Split(splArr, StringSplitOptions.RemoveEmptyEntries);
                this.Url = arr[0];
                this.Code = arr.Count()>1? arr[1].Split('=')[1]:null;

                //this.Url = action.Split('?')[0]; 
                //this.Code = action.Split('?').Count() > 1 ? (action.Split('?').Count()> 1 ? action.Split('?')[1].Split('=')[1] : null) : null;
            }
        }

        #endregion

    }
}
