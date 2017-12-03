using Easyman.Common.Mvc;
using System.Web.Mvc;
using Easyman.Import;
using Easyman.Dto;
using System.Collections.Generic;
using Easyman.Service;
using System.IO;
using System.Web;
using Easyman.Sys;
using System.Linq;
using Abp.UI;

namespace Easyman.FwWeb.Controllers
{
    public class ImportController : EasyManController
    {
        #region 初始化

        private readonly IPreDataTypeAppService _preDataTypeAppService;
        private readonly IDbTypeAppService _dbTypeAppService;
        private readonly IRegularAppService _regularAppService;
        private readonly IDefaultFieldAppService _defaultFieldAppService;
        private readonly IImpTbAppService _impTbAppService;
        private readonly IImpTbFieldAppService _impTbFieldAppService;
        private readonly IImpTypeAppService _impTypeAppService;
        private readonly IDbServerAppService _dbServerAppService;
        private readonly IModulesAppService _modulesAppService;
        private readonly IImportLogAppService _importLogAppService;
        private readonly IDbServerAppService _dbServerService;

        public ImportController(IPreDataTypeAppService preDataTypeAppService, IDbTypeAppService dbTypeAppService, 
            IRegularAppService regularAppService, IDefaultFieldAppService defaultFieldAppService,
            IImpTbAppService impTbAppService, IImpTbFieldAppService impTbFieldAppService,
            IImpTypeAppService impTypeAppService, IDbServerAppService dbServerAppService,
            IModulesAppService modulesAppService, IImportLogAppService importLogAppService, IDbServerAppService dbServerService)
        {
            _preDataTypeAppService = preDataTypeAppService;
            _dbTypeAppService = dbTypeAppService;
            _regularAppService = regularAppService;
            _defaultFieldAppService = defaultFieldAppService;
            _impTbAppService = impTbAppService;
            _impTbFieldAppService = impTbFieldAppService;
            _impTypeAppService = impTypeAppService;
            _dbServerAppService = dbServerAppService;
            _modulesAppService = modulesAppService;
            _importLogAppService = importLogAppService;
            _dbServerService = dbServerService;
        }

        #endregion

        #region 数据库类型

        public ActionResult DbTypePage()
        {
            return View("Easyman.FwWeb.Views.Import.DbTypePage");
        }

        public ActionResult EditDbType(long? id)
        {
            var data = new DbTypeInput();
            if (id != null)
                data = _dbTypeAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Import.EditDbType", data);
        }
        #endregion

        #region 数据类型

        public ActionResult PreDataTypePage()
        {
            return View("Easyman.FwWeb.Views.Import.PreDataTypePage");
        }

        public ActionResult EditPreDataType(long? id)
        {
            ViewBag.DbType = _dbTypeAppService.GetDropDownList();
            var data = new PreDataTypeInput();
            if (id != null)
                data = _preDataTypeAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Import.EditPreDataType", data);
        }

        #endregion

        #region 正则类型

        public ActionResult RegularPage()
        {
            return View("Easyman.FwWeb.Views.Import.RegularPage");
        }

        public ActionResult EditRegular(long? id)
        {
            var data = new RegularInput();
            if (id != null)
                data = _regularAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Import.EditRegular", data);
        }

        #endregion

        #region 内置字段

        public ActionResult DefaultFieldPage()
        {
            return View("Easyman.FwWeb.Views.Import.DefaultFieldPage");
        }

        public ActionResult EditDefaultField(long? id)
        {
            ViewBag.DbType = _dbTypeAppService.GetDropDownList();
            var data = new DefaultFieldInput();
            if (id != null)
                data = _defaultFieldAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Import.EditDefaultField", data);
        }

        #endregion

        #region 外导表分类

        public ActionResult ImpTypePage()
        {
            return View("Easyman.FwWeb.Views.Import.ImpTypePage");
        }

        public ActionResult EditImpType(long? id)
        {
            var data = new ImpTypeInput();
            if (id != null)
                data = _impTypeAppService.Get(id.Value);
            return View("Easyman.FwWeb.Views.Import.EditImpType", data);
        }

        #endregion

        #region 表配置

        public ActionResult ImpTbPage(long? id)
        {
            return View("Easyman.FwWeb.Views.Import.ImpTbPage");
        }

        public ActionResult EditImpTb(long? id)
        {
            ViewBag.ImpType = _impTypeAppService.GetDropDownList();
            ViewBag.DbServer = _dbServerAppService.GetDropDownList();
            ViewBag.DbType = _dbTypeAppService.GetDropDownList();
            ViewBag.Rule = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "以日期后缀创建表",Value ="1" },
                new SelectListItem() { Text = "以指定表名创建表",Value ="2" },
                new SelectListItem() { Text = "以用户后缀创建表",Value ="3" },
                new SelectListItem() { Text = "自定义后缀创建表",Value ="4" }
            };
            var sql = "";
            var data = new ImpTbInput();
            if (id != null)
            {
                data = _impTbAppService.Get(id.Value);
                data.DefaultFields = _impTbAppService.GetIds(id.Value);
                try
                {
                    var dbServer = _dbServerAppService.GetDbServer(data.DbServerId);
                    var dbType = _dbTypeAppService.Get(a => a.Name == dbServer.DbTypeName);
                    data.DbTypeId = dbType.Id;
                }
                catch { }
                sql = data.Sql;
            }
            ViewBag.Sql = sql;
            return View("Easyman.FwWeb.Views.Import.EditImpTb", data);

        }

        #region 读取执行库对应的数据库类别
        [HttpPost]
        public string GetDBType(long id)
        {
            var server = _dbServerService.GetDbServer(id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(server.DbTypeId);
        }
        #endregion

        [HttpPost]
        public string GetJson(long id)
        {
            var dataList = _defaultFieldAppService.GetJsonObject(id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dataList);
        }

        [HttpPost]
        public string SaveSqlScript(long id)
        {
            var state = _impTbAppService.CreateSqlScript(id);
            var obj = new { state = state, msg = state ? "保存成功" : "保存失败" };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        #endregion

        #region 表归属字段配置

        public ActionResult ImpTbFieldPage(long id)
        {
            ViewBag.ImpTbId = id;
            ViewBag.DbTypeId = _impTbAppService.GetDbTypeId(id);
            return View("Easyman.FwWeb.Views.Import.ImpTbFieldPage");
        }

        public ActionResult EditImpTbField(long? id, long impTbId, long dbTypeId)
        {
            ViewBag.DataType = _preDataTypeAppService.GetDropDownList();
            List<SelectListItem> SelectList = _regularAppService.GetDropDownList();
            #region 添加默认值
            for (int i = 0; i < SelectList.Count; i++)
            {
                if (SelectList[i].Text.Trim().IndexOf("任意字符") >= 0)
                {
                    SelectList[i].Selected = true;
                    SelectListItem selectItme = SelectList[i];
                    SelectList.RemoveAt(i);
                    selectItme.Selected = true;
                    SelectList.Insert(0, selectItme);
                    break;
                }
            }
            #endregion
            ViewBag.Regular = SelectList;
            ViewBag.dbTypeId = dbTypeId;
            var data = new ImpTbFieldInput();
            if (id != null) { 
                data = _impTbFieldAppService.Get(id.Value);}
            data.ImpTbId = impTbId;
            return View("Easyman.FwWeb.Views.Import.EditImpTbField", data);
        }

        #endregion

        #region 公共调用模块

        public ActionResult CommonImport(string importCode, string fileName)
        {
            var model = new ImportLogInput() { Code = importCode, ModuleCode = fileName };
            var list = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "在线导入", Value = "在线导入" },
                new SelectListItem() { Text = "离线导入", Value = "离线导入" }
            };
            ViewBag.Offline = list;

           // var module = _modulesAppService.GetNavigation(a => a.Code == fileName);
            var format = "xls,csv";
            var radioList = new List<string>();
            if (fileName != null)
            {
                foreach (var item in format.Split(','))
                {
                    radioList.Add(fileName + "." + item);
                };
            }
            ViewBag.Radio = radioList;
            return View("Easyman.FwWeb.Views.Import.CommonImport", model);
        }

        public ActionResult UploadDown(string code, string name)
        {
            var fileName = string.Empty;
            var stringWriter = _impTbAppService.GetbytesByCode(code, out fileName);
            fileName += ".csv";
            fileName = string.IsNullOrEmpty(name) ? fileName : name;
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.BinaryWrite(stringWriter);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        #endregion
    }
}
