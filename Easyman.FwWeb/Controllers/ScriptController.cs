using Easyman.Service;
using Easyman.Common.Mvc;
using System.Web.Mvc;
using Easyman.Dto;
using Newtonsoft.Json;

namespace Easyman.FwWeb.Controllers
{
    public class ScriptController:EasyManController
    {
        #region 初始化    
        private readonly IScriptTypeAppService _scriptTypeService;
        private readonly IScriptAppService _scriptService;
        private readonly IScriptNodeTypeAppService _scriptNodeTypeService;
        public ScriptController(IScriptTypeAppService scriptTypeService, IScriptAppService scriptService, IScriptNodeTypeAppService scriptNodeTypeService)
        {
            _scriptTypeService = scriptTypeService;
            _scriptService = scriptService;
            _scriptNodeTypeService = scriptNodeTypeService;
        }

        #endregion

        #region 脚本类型
        /// <summary>
        /// 初始化报表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScriptTypePage()
        {
            return View("Easyman.FwWeb.Views.Script.ScriptTypePage");
        }
        /// <summary>
        /// 编辑页面初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult EditScriptType(long? ScriptTypeId)
        {
            var model = new ScriptTypeInput();
            if (ScriptTypeId != null)
            {
                model = _scriptTypeService.SingScriptType(ScriptTypeId.Value);
            }
            return View("Easyman.FwWeb.Views.Script.EditScriptType", model);
        }

        #endregion

        #region 脚本流
        /// <summary>
        /// 报表初始化页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScriptPage()
        {
            return View("Easyman.FwWeb.Views.Script.ScriptPage");
        }
        /// <summary>
        /// 编辑页面初始化
        /// </summary>
        /// <param name="ScriptId"></param>
        /// <returns></returns>
        public ActionResult EditScript(long? ScriptId)
        {
            var Res = new EditScript();
            ViewBag.ScriptTypeList = _scriptTypeService.GetAllScriptTypeList();
            ViewBag.ScriptNodeTypeList = JsonConvert.SerializeObject(_scriptNodeTypeService.GetAllScriptNodeType());
            if (ScriptId != null)
            {
                Res = _scriptService.SingScript(ScriptId.Value);
            }
            return View("Easyman.FwWeb.Views.Script.EditScript", Res);
        }

        #endregion

        #region 脚本流实例
        /// <summary>
        /// 脚本流实例初始化
        /// </summary>
        public ActionResult ExampleScript()
        {
            return View("Easyman.FwWeb.Views.Script.ExampleScript");
        }
        /// <summary>
        /// 查看一条脚本流实例
        /// </summary>
        /// <param name="ScriptId">脚本流ID</param>
        ///  <param name="ExampleId">脚本流实例ID</param>
        /// <returns></returns>
        public ActionResult SelectScriptExample(long? ScriptId = null, long? ExampleId = null)
        {
            var model = new InitExampleScript();
            if (ScriptId != null || ExampleId != null)
            {
                model = _scriptService.GetSignExampleScript(ScriptId, ExampleId);
            }
            return View("Easyman.FwWeb.Views.Script.SelectScriptExample", model);
        }
        #endregion

    }
}
