using Abp.AutoMapper;
using Abp.Domain.Uow;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using EasyMan;
using EasyMan.Export;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web;
using Abp.Domain.Repositories;
using Abp.UI;
using System.Collections.Generic;

namespace Easyman.FwWeb.Controllers
{
    public class ScriptNodeController : EasyManController
    {
        #region 初始化

        private readonly IScriptNodeTypeAppService _scriptNodeTypeService;
        private readonly IScriptNodeAppService _scriptNodeService;

        private readonly IRepository<Easyman.Domain.ScriptNodeCase, long> _scriptNodeCase;



        public ScriptNodeController(IScriptNodeTypeAppService scriptNodeTypeService, IScriptNodeAppService scriptNodeService,
            IRepository<Easyman.Domain.ScriptNodeCase, long> scriptNodeCase)
        {

            _scriptNodeTypeService = scriptNodeTypeService;
            _scriptNodeService = scriptNodeService;
            _scriptNodeCase = scriptNodeCase;
        }

        #endregion


        #region 节点类型

        public ActionResult InsertScriptNodeType()
        {
            var model = new ScriptNodeTypeInput();
            return View("Easyman.FwWeb.Views.ScriptNode.EditScriptNodeType", model);
        }

        public ActionResult EditScriptNodeType(long scriptNodeTypeId)
        {
            var type = _scriptNodeTypeService.GetScriptNodeType(scriptNodeTypeId);
            //var model = type == null ? new ScriptNodeTypeInput() : new ScriptNodeTypeInput() { Id = type.Id, Name = type.Name, Remark = type.Remark };

            var model = type == null ? new ScriptNodeTypeInput() : AutoMapper.Mapper.Map<ScriptNodeTypeInput>(AutoMapper.Mapper.Map<ScriptNodeType>(type));
            return View("Easyman.FwWeb.Views.ScriptNode.EditScriptNodeType", model);
        }

        public ActionResult ScriptNodeType()
        {
            return View("Easyman.FwWeb.Views.ScriptNode.ScriptNodeType");
        }

        #endregion

        #region 节点管理

        public ActionResult InsertScriptNode(string TaskSpecific)
        {
            var model = new ScriptNodeInput();
            model.TaskSpecific = TaskSpecific;
            return View("Easyman.FwWeb.Views.ScriptNode.EditScriptNode", model);
        }

        public ActionResult EditScriptNode(long scriptNodeId)
        {
            var node = _scriptNodeService.GetScriptNode(scriptNodeId);
            var model = node == null ? new ScriptNodeInput() : AutoMapper.Mapper.Map<ScriptNodeInput>(AutoMapper.Mapper.Map<ScriptNode>(node));
            return View("Easyman.FwWeb.Views.ScriptNode.EditScriptNode", model);
        }

        public ActionResult ScriptNode()
        {
            return View("Easyman.FwWeb.Views.ScriptNode.ScriptNode");
        }

        #endregion

        
        /// <summary>
        /// 编译后脚本内容显示页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CompileScript(long id)
        {
            //EM_SCRIPT_NODE_CASE表id
            //根据id查询脚本节点实例需要的数据是：COMPILE_CONTENT字段
            //var model= _scriptNodeService.GetScriptNode(id);
            var temp = _scriptNodeCase.FirstOrDefault(x => x.Id==id);
            ScriptNodeCaseModel model = new ScriptNodeCaseModel();
            model.Id = temp.Id;
            model.CompileContent = temp.CompileContent;
            return View("Easyman.FwWeb.Views.ScriptNode.CompileScript", model);

        }
        public void CompileScriptExport(long? id)
        {
            var temp = _scriptNodeCase.FirstOrDefault(x => x.Id == id);
            ScriptNodeCaseModel model = new ScriptNodeCaseModel();
            model.Id = temp.Id;
            model.CompileContent = temp.CompileContent;
            CompileScriptExports(model);
        }
        /// <summary>
        /// 编译后脚本内容下载
        /// </summary>
        /// <param name="id"></param>
        private static void CompileScriptExports(ScriptNodeCaseModel temp)
        {
            //var temp = _scriptNodeCase.FirstOrDefault(x => x.Id == id);
            System.IO.StringWriter sw = new System.IO.StringWriter();
            sw.WriteLine(temp.CompileContent);
           
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.Charset = "GB2312";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=编译后脚本内容.txt");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            System.Web.HttpContext.Current.Response.ContentType = "text/plain";//设置输出文件类型为txt文件。 

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.HttpContext.Current.Response.Write(sw.ToString());
            System.Web.HttpContext.Current.Response.End();
        }

       
        /// <summary>
        /// 根据任务ID下载任务日志
        /// </summary>
        /// <param name="input"></param>
        public void selectScript(long id)
        {

            //ScriptNodeCaseLogModel
            //根据任务ID查询任务实例日志
            //          string sql = @"SELECT A.ID,a.SCRIPT_NODE_CASE_ID,a.log_time,a.log_msg,a.sql_msg,C.NAME
            //FROM EM_SCRIPT_NODE_CASE_LOG A
            //    LEFT JOIN EM_SCRIPT_NODE_CASE B ON A.SCRIPT_NODE_CASE_ID = B.ID
            //   LEFT JOIN EM_SCRIPT_NODE C ON B.SCRIPT_NODE_ID=C.ID";
            //          if (id != 0)
            //          {
            //              sql += "  WHERE A.SCRIPT_NODE_CASE_ID=" + id;
            //              sql += "  ORDER BY A.ID";
            //          }
            //          //执行SQL获取数据
            //          DataTable dt = new DataTable();
            //          dt = DbHelper.ExecuteGetTable(sql);
            //          //下载文件
            //          Exports(dt);
            try
            {
                if (id != 0)
                {
                    //GetScriptNodeCaseLog
                    var data = _scriptNodeService.GetScriptNodeCaseLog(id);
                    var dataId = _scriptNodeCase.Get(id);
                    var dataName = _scriptNodeService.GetScriptNode((long)dataId.ScriptNodeId).Name;
                    Exports(data, dataName);

                }
            }catch(Exception ex)
            {
                throw new UserFriendlyException("操作失败！");
            }


        }
        private static void Exports(List<ScriptNodeCaseLog> tb,string name)
         {

            System.IO.StringWriter sw = new System.IO.StringWriter();
            for (int i = 0; i < tb.Count; i++)
            {

                var biaoshi = tb[i].Id;
                var jiedianid = tb[i].ScriptNodeCaseId;
                var shijian = tb[i].LogTime;
                var xinxi = tb[i].LogMsg;
                var sqlrizhi = tb[i].SqlMsg;
                var mingcheng = name;
               
                sw.WriteLine("【" + shijian + "】标识【" + biaoshi +"】脚本节点实例ID【"+jiedianid+"】节点名称【"+mingcheng+"】\t");
                sw.WriteLine("日志信息：" + xinxi + "\t");
                //sqlrizhi空显示null
                if (sqlrizhi == "" || sqlrizhi==null) { sw.WriteLine("SQL日志：null\t"); }
                else { sw.WriteLine("SQL日志：" + sqlrizhi + "\t"); }
               
                sw.WriteLine("\n");
            }
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.Charset = "GB2312";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=任务实例日志.txt");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            System.Web.HttpContext.Current.Response.ContentType = "text/plain";//设置输出文件类型为txt文件。 

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.HttpContext.Current.Response.Write(sw.ToString());
            System.Web.HttpContext.Current.Response.End();
        }

    }
}
