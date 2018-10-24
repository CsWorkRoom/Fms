using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Common;
using Easyman.Common.Mvc;
using Easyman.Dto;
using Easyman.Service;
using Easyman.Sys;
using Easyman.Users;
using Easyman.Web.Models;
using EasyMan;
using EasyMan.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Easyman.Web.App_Start;
using System.IO;
using System.Configuration;
using System.Web;

namespace Easyman.Web.Controllers
{
    public class FileController : EasyManController
    {
        #region 初始化 

        private readonly IFileFormatAppService _FileFormatAppService;
        private readonly IComputerAppService _ComputerAppService;
        private readonly IFolderAppService _FolderAppService;
        private readonly IFileAppService _FileAppService;
        private readonly IMonitFileAppService _MonitFileAppService;
        private readonly IFileUploadAppService _FileUploadAppService;
        private readonly IUserAppService _UserAppService;

        public FileController(IFileFormatAppService FileFormatAppService,
            IComputerAppService ComputerAppService,
            IFolderAppService FolderAppService,
            IFileAppService FileAppService,
            IMonitFileAppService MonitFileAppService,
            IFileUploadAppService FileUploadAppService,
            IUserAppService UserAppService)
        {
            _FileFormatAppService = FileFormatAppService;
            _ComputerAppService = ComputerAppService;
            _FolderAppService = FolderAppService;
            _FileAppService = FileAppService;
            _MonitFileAppService = MonitFileAppService;
            _FileUploadAppService = FileUploadAppService;
            _UserAppService = UserAppService;
        }

        #endregion


        #region 文件格式管理
        public ActionResult EditFileFormat(long? id)
        {
            if (id == null || id == 0)
            {
                return View(new FileFormatModel());
            }
            var entObj = _FileFormatAppService.GetFileFormat(id.Value);
            return View(entObj);
        }
        #endregion

        #region  被监控目录

        public ActionResult ShowMonitFile()
        {
            //using (StreamReader reader = new StreamReader("D:\\FileUpLoad\\2018-01-23\\2f8d3a964ea2356965a6884e58e648f1.txt", System.Text.Encoding.GetEncoding("UTF-8")))
            //{
            //    string text = reader.ReadToEnd();
            //    Aspose.Words.Document doc = new Aspose.Words.Document();
            //    Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);
            //    builder.Write(text);
            //    string outpdfpath = "D:\\FileUpLoad\\2018-01-23\\2f8d3a964ea2356965a6884e58e648f1.pdf";
            //    doc.Save(outpdfpath, Aspose.Words.SaveFormat.Pdf);
            //}
            var user = _FileAppService.GetCurrentUser();
            ViewData["userName"] = user.Name;
            return View();
        }

        //根据当前用户获取其管辖的终端及目录
        //此处不考虑异步，一次性加载
        public ActionResult GetNodesByCurUser()
        {

            #region 根据当前用户获取所辖终端列表
            string sqlComputer = string.Format(@"
                     SELECT A.*
                      FROM FM_COMPUTER A,
                           (    SELECT ID
                                  FROM EM_DISTRICT
                            CONNECT BY PRIOR ID = PARENT_ID
                            START WITH ID = (SELECT DISTRICT_ID
                                               FROM ABP_USERS
                                              WHERE ID = {0})) B
                     WHERE A.DISTRICT_ID = B.ID", CurrUserId());
            #endregion

            List<TreeNode> nodeList = new List<TreeNode>();//初始化node

            DataTable comDt = DbHelper.ExecuteGetTable(sqlComputer);
            if (comDt != null && comDt.Rows.Count > 0)
            {
                //拼装终端的nodes
                foreach (DataRow row in comDt.Rows)
                {
                    TreeNode node = new TreeNode();
                    node.id = "computer_" + row["ID"].ToString().Trim();//computer_ + 编号
                    node.name = row["NAME"].ToString();//节点名称
                    node.title = "(" + row["CODE"].ToString() + ")" + row["NAME"].ToString() + "[" + row["IP"].ToString() + "]";//节点说明
                    node.pId = "0";//父节点
                    node.nodeType = "computer";
                    node.iconSkin = "pIcon01";//房子
                    node.isParent = true;
                    nodeList.Add(node);//添加节点
                    //获取共享目录List
                    var folderList = _FolderAppService.GetFolderListByComputer(Convert.ToInt64(row["ID"].ToString().Trim()));
                    if (folderList != null && folderList.Count > 0)
                    {
                        var nodeListFolder = folderList.Select(p => new TreeNode
                        {
                            id = "folder_" + p.Id,
                            name = p.Name,
                            title = p.Name,
                            pId = "computer_" + row["ID"].ToString().Trim(),
                            nodeType = "folder",//共享目录
                            iconSkin = "pIcon010",
                            isParent = true
                        });
                        nodeList.AddRange(nodeListFolder);//添加共享目录

                        //根据共享目录获取文件夹及文件
                        foreach (var folder in folderList)
                        {
                            #region 原方式
                            //var fileList = _FileAppService.GetCurFileListByFolder(folder.Id);
                            //if (fileList != null && fileList.Count > 0)
                            //{
                            //    var nodeListFile = fileList.Select(p => new TreeNode
                            //    {
                            //        id = "file_" + p.Id,
                            //        name = p.Name,
                            //        title = p.Name,
                            //        pId = p.ParentId == null ? ("folder_" + folder.Id) : ("file_" + p.ParentId),
                            //        nodeType = "file"
                            //    });
                            //    nodeList.AddRange(nodeListFile);//添加文件列表
                            //}
                            #endregion

                            #region 获取文件夹及文件 fileSql
                            string fileSql = string.Format(@"SELECT A.ID,
                                   A.CLIENT_PATH,
                                   A.COMPUTER_ID,
                                   A.FILE_FORMAT_ID,
                                   A.FILE_LIBRARY_ID,
                                   A.FOLDER_ID,
                                   A.FOLDER_VERSION_ID,
                                   A.MD5,
                                   A.NAME,
                                   A.PARENT_ID,
                                   A.RELY_MONIT_FILE_ID,
                                   A.REMARK,
                                   A.SERVER_PATH,
                                   A.STATUS,
                                   B.IS_FOLDER,
                                   B.NAME FORMAT_NAME,
                                   B.ICON FILE_ICON,
                                   C.NAME MD5_NAME,
                                   C.""SIZE"" FILE_SIZE
                              FROM FM_MONIT_FILE A
                                   LEFT JOIN FM_FILE_FORMAT B ON(A.FILE_FORMAT_ID = B.ID)
                                   LEFT JOIN FM_FILE_LIBRARY C ON(A.FILE_LIBRARY_ID = C.ID)
                             WHERE A.FOLDER_VERSION_ID = (SELECT MAX(K.ID)
                                                            FROM FM_FOLDER_VERSION K
                                                           WHERE K.FOLDER_ID = {0})
                                   AND A.FOLDER_ID={0}", folder.Id);
                            #endregion

                            DataTable fileDt = DbHelper.ExecuteGetTable(fileSql);
                            if (fileDt != null && fileDt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in fileDt.Rows)
                                {
                                    TreeNode fileNode = new TreeNode();
                                    fileNode.id = "file_" + dr["ID"].ToString().Trim();
                                    fileNode.nodeType = "file";//文件
                                    fileNode.name = dr["NAME"].ToString();
                                    fileNode.title = dr["NAME"].ToString();
                                    fileNode.pId = string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) ? ("folder_" + folder.Id) : ("file_" + dr["PARENT_ID"].ToString().Trim());
                                    fileNode.isFolder = dr["IS_FOLDER"].ToString() == "1" ? true : false;
                                    fileNode.isParent = fileNode.isFolder.Value;//文件夹才有子项
                                    fileNode.iconSkin = fileNode.isParent ? "pIcon010" : "";
                                    nodeList.Add(fileNode);
                                }
                            }
                        }
                    }
                }
            }
            return Json(nodeList);//返回节点列表json
        }

        public string GetNodes(string parentIds = null)
        {
            List<TreeNode> nodeList = new List<TreeNode>();//初始化node
            if (string.IsNullOrEmpty(parentIds))
            {
                #region 根据当前用户获取所辖终端列表
                string sqlComputer = string.Format(@"
                     SELECT A.*
                      FROM FM_COMPUTER A,
                           (    SELECT ID
                                  FROM EM_DISTRICT
                            CONNECT BY PRIOR ID = PARENT_ID
                            START WITH ID = (SELECT DISTRICT_ID
                                               FROM ABP_USERS
                                              WHERE ID = {0})) B
                     WHERE A.DISTRICT_ID = B.ID", CurrUserId());
                #endregion
                //List<TreeNode> nodeList = new List<TreeNode>();//初始化node

                DataTable comDt = DbHelper.ExecuteGetTable(sqlComputer);
                if (comDt != null && comDt.Rows.Count > 0)
                {
                    //拼装终端的nodes
                    foreach (DataRow row in comDt.Rows)
                    {
                        TreeNode node = new TreeNode();
                        node.id = "computer_" + row["ID"].ToString().Trim();//computer_ + 编号
                        node.name = row["NAME"].ToString();//节点名称
                        node.title = "(" + row["CODE"].ToString() + ")" + row["NAME"].ToString() + "[" + row["IP"].ToString() + "]";//节点说明
                        //node.pId = "0";//父节点
                        node.nodeType = "computer";
                        node.iconSkin = "pIcon01";//房子
                        node.isParent = true;
                        nodeList.Add(node);//添加节点

                    }
                }
            }
            else
            {
                var arr = parentIds.Split('_');
                switch (arr[0])
                {
                    case "computer":
                        //获取共享目录List
                        var folderList = _FolderAppService.GetFolderListByComputer(Convert.ToInt64(arr[1].Trim()));
                        if (folderList != null && folderList.Count > 0)
                        {
                            var nodeListFolder = folderList.Select(p => new TreeNode
                            {
                                id = "folder_" + p.Id,
                                name = p.Name,
                                title = p.Name,
                                //pId = "computer_" + arr[1].Trim(),
                                nodeType = "folder",//共享目录
                                iconSkin = "pIcon010",
                                isParent = true
                            });
                            nodeList.AddRange(nodeListFolder);//添加共享目录
                        }
                        break;
                    case "folder":
                        #region 获取共享文件夹下级文件 fileSql
                        string fileSql = string.Format(@"
                                        SELECT A.ID,
                                               A.CLIENT_PATH,
                                               A.COMPUTER_ID,
                                               A.FILE_FORMAT_ID,
                                               A.FILE_LIBRARY_ID,
                                               A.FOLDER_ID,
                                               A.FOLDER_VERSION_ID,
                                               A.MD5,
                                               A.NAME,
                                               A.PARENT_ID,
                                               A.RELY_MONIT_FILE_ID,
                                               A.REMARK,
                                               A.SERVER_PATH,
                                               A.STATUS,
                                               B.IS_FOLDER,
                                               B.NAME FORMAT_NAME,
                                               B.ICON FILE_ICON,
                                               C.NAME MD5_NAME,
                                               C.""SIZE"" FILE_SIZE
                                          FROM FM_MONIT_FILE A
                                               LEFT JOIN FM_FILE_FORMAT B ON (A.FILE_FORMAT_ID = B.ID)
                                               LEFT JOIN FM_FILE_LIBRARY C ON (A.FILE_LIBRARY_ID = C.ID)
                                         WHERE     A.FOLDER_VERSION_ID = (SELECT MAX (K.ID)
                                                                            FROM FM_FOLDER_VERSION K
                                                                           WHERE K.FOLDER_ID = {0} AND K.END_TIME IS NOT NULL)
                                               AND A.FOLDER_ID={0}
                                               AND PARENT_ID IS NULL", arr[1]);
                        #endregion

                        DataTable fileDt = DbHelper.ExecuteGetTable(fileSql);
                        if (fileDt != null && fileDt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in fileDt.Rows)
                            {
                                TreeNode fileNode = new TreeNode();
                                fileNode.id = "file_" + dr["ID"].ToString().Trim();
                                fileNode.nodeType = "file";//文件
                                fileNode.name = dr["NAME"].ToString();
                                fileNode.title = dr["NAME"].ToString();
                                //fileNode.pId = string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) ? ("folder_" + folder.Id) : ("file_" + dr["PARENT_ID"].ToString().Trim());
                                fileNode.isFolder = dr["IS_FOLDER"].ToString() == "1" ? true : false;
                                fileNode.isParent = fileNode.isFolder.Value;//文件夹才有子项
                                fileNode.iconSkin = fileNode.isParent ? "pIcon010" : "";
                                nodeList.Add(fileNode);
                            }
                        }
                        break;
                    case "file":
                        #region 获取文件夹的下级文件 fileSql
                        string fileSql1 = string.Format(@"SELECT A.ID,
                                       A.CLIENT_PATH,
                                       A.COMPUTER_ID,
                                       A.FILE_FORMAT_ID,
                                       A.FILE_LIBRARY_ID,
                                       A.FOLDER_ID,
                                       A.FOLDER_VERSION_ID,
                                       A.MD5,
                                       A.NAME,
                                       A.PARENT_ID,
                                       A.RELY_MONIT_FILE_ID,
                                       A.REMARK,
                                       A.SERVER_PATH,
                                       A.STATUS,
                                       B.IS_FOLDER,
                                       B.NAME FORMAT_NAME,
                                       B.ICON FILE_ICON,
                                       C.NAME MD5_NAME,
                                       C.""SIZE"" FILE_SIZE
                                  FROM FM_MONIT_FILE A
                                       LEFT JOIN FM_FILE_FORMAT B ON (A.FILE_FORMAT_ID = B.ID)
                                       LEFT JOIN FM_FILE_LIBRARY C ON (A.FILE_LIBRARY_ID = C.ID)
                                 WHERE PARENT_ID = {0}", arr[1]);
                        #endregion

                        DataTable fileDt1 = DbHelper.ExecuteGetTable(fileSql1);
                        if (fileDt1 != null && fileDt1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in fileDt1.Rows)
                            {
                                TreeNode fileNode = new TreeNode();
                                fileNode.id = "file_" + dr["ID"].ToString().Trim();
                                fileNode.nodeType = "file";//文件
                                fileNode.name = dr["NAME"].ToString();
                                fileNode.title = dr["NAME"].ToString();
                                //fileNode.pId = string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) ? ("folder_" + folder.Id) : ("file_" + dr["PARENT_ID"].ToString().Trim());
                                fileNode.isFolder = dr["IS_FOLDER"].ToString() == "1" ? true : false;
                                fileNode.isParent = fileNode.isFolder.Value;//文件夹才有子项
                                fileNode.iconSkin = fileNode.isParent ? "pIcon010" : "";
                                nodeList.Add(fileNode);
                            }
                        }
                        break;
                }
            }
            return JSON.DecodeToStr(nodeList);
        }

        public ActionResult GetComputerListByCurUser()
        {
            #region 根据当前用户获取所辖终端列表
            string sqlComputer = string.Format(@"
                     SELECT A.*
                      FROM FM_COMPUTER A,
                           (    SELECT ID
                                  FROM EM_DISTRICT
                            CONNECT BY PRIOR ID = PARENT_ID
                            START WITH ID = (SELECT DISTRICT_ID
                                               FROM ABP_USERS
                                              WHERE ID = {0})) B
                     WHERE A.DISTRICT_ID = B.ID", CurrUserId());
            #endregion
            List<TreeNode> nodeList = new List<TreeNode>();//初始化node

            DataTable comDt = DbHelper.ExecuteGetTable(sqlComputer);
            if (comDt != null && comDt.Rows.Count > 0)
            {
                //拼装终端的nodes
                foreach (DataRow row in comDt.Rows)
                {
                    TreeNode node = new TreeNode();
                    node.id = "computer_" + row["ID"].ToString().Trim();//computer_ + 编号
                    node.name = row["NAME"].ToString();//节点名称
                    node.title = "(" + row["CODE"].ToString() + ")" + row["NAME"].ToString() + "[" + row["IP"].ToString() + "]";//节点说明
                    node.pId = "0";//父节点
                    node.nodeType = "computer";

                    nodeList.Add(node);//添加节点
                }
            }
            return Json(nodeList);//返回节点列表json
        }
        /// <summary>
        /// 根据共享文件夹获取下级文件
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public ActionResult GetFileListByFolder(long folderId)
        {
            List<TreeNode> nodeList = new List<TreeNode>();//初始化node

            #region 获取文件夹及文件 fileSql
            string fileSql = string.Format(@"SELECT A.ID,
                               A.CLIENT_PATH,
                               A.COMPUTER_ID,
                               A.FILE_FORMAT_ID,
                               A.FILE_LIBRARY_ID,
                               A.FOLDER_ID,
                               A.FOLDER_VERSION_ID,
                               A.MD5,
                               A.NAME,
                               A.PARENT_ID,
                               A.RELY_MONIT_FILE_ID,
                               A.REMARK,
                               A.SERVER_PATH,
                               A.STATUS,
                               B.IS_FOLDER,
                               B.NAME FORMAT_NAME,
                               B.ICON FILE_ICON,
                               C.NAME MD5_NAME,
                               C.""SIZE"" FILE_SIZE,
                               A.IS_HIDE
                          FROM FM_MONIT_FILE A
                               LEFT JOIN FM_FILE_FORMAT B ON(A.FILE_FORMAT_ID = B.ID)
                               LEFT JOIN FM_FILE_LIBRARY C ON(A.FILE_LIBRARY_ID = C.ID)
                         WHERE     A.FOLDER_VERSION_ID = (SELECT MAX(K.ID)
                                                            FROM FM_FOLDER_VERSION K
                                                           WHERE K.FOLDER_ID = {0} AND K.END_TIME IS NOT NULL)
                               AND A.FOLDER_ID={0}
                               AND A.PARENT_ID IS NULL", folderId);
            #endregion

            DataTable fileDt = DbHelper.ExecuteGetTable(fileSql);
            if (fileDt != null && fileDt.Rows.Count > 0)
            {
                foreach (DataRow dr in fileDt.Rows)
                {
                    TreeNode fileNode = new TreeNode();
                    fileNode.id = "file_" + dr["ID"].ToString().Trim();
                    fileNode.name = dr["NAME"].ToString();
                    fileNode.title = dr["NAME"].ToString();
                    fileNode.pId = string.IsNullOrEmpty(dr["PARENT_ID"].ToString()) ? ("folder_" + folderId) : ("file_" + dr["PARENT_ID"].ToString().Trim());
                    fileNode.isFolder = dr["IS_FOLDER"].ToString() == "1" ? true : false;
                    fileNode.isParent = fileNode.isFolder.Value;//文件夹才有子项
                    fileNode.iconSkin = fileNode.isParent ? "pIcon010" : "";
                    fileNode.fileFormatIcon = dr["FILE_ICON"].ToString();
                    fileNode.isHide = dr["IS_HIDE"].ToString() == "1" ? true : false;
                    nodeList.Add(fileNode);
                }
            }

            return Json(nodeList);//返回节点列表json
        }
        /// <summary>
        /// 根据文件获取下级文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public ActionResult GetFileListByFile(long fileId)
        {
            List<TreeNode> nodeList = new List<TreeNode>();//初始化node

            #region 获取文件夹及文件 fileSql
            string fileSql = string.Format(@"SELECT A.ID,
                           A.CLIENT_PATH,
                           A.COMPUTER_ID,
                           A.FILE_FORMAT_ID,
                           A.FILE_LIBRARY_ID,
                           A.FOLDER_ID,
                           A.FOLDER_VERSION_ID,
                           A.MD5,
                           A.NAME,
                           A.PARENT_ID,
                           A.RELY_MONIT_FILE_ID,
                           A.REMARK,
                           A.SERVER_PATH,
                           A.STATUS,
                           B.IS_FOLDER,
                           B.NAME FORMAT_NAME,
                           B.ICON FILE_ICON,
                           C.NAME MD5_NAME,
                           C.""SIZE"" FILE_SIZE
                      FROM FM_MONIT_FILE A
                           LEFT JOIN FM_FILE_FORMAT B ON(A.FILE_FORMAT_ID = B.ID)
                           LEFT JOIN FM_FILE_LIBRARY C ON(A.FILE_LIBRARY_ID = C.ID)
                     WHERE a.PARENT_ID = {0}", fileId);
            #endregion

            DataTable fileDt = DbHelper.ExecuteGetTable(fileSql);
            if (fileDt != null && fileDt.Rows.Count > 0)
            {
                foreach (DataRow dr in fileDt.Rows)
                {
                    TreeNode fileNode = new TreeNode();
                    fileNode.id = "file_" + dr["ID"].ToString().Trim();
                    fileNode.name = dr["NAME"].ToString();
                    fileNode.title = dr["NAME"].ToString();
                    fileNode.pId = "file_" + fileId;
                    fileNode.isFolder = dr["IS_FOLDER"].ToString() == "1" ? true : false;
                    fileNode.isParent = fileNode.isFolder.Value;//文件夹才有子项
                    fileNode.iconSkin = fileNode.isParent ? "pIcon010" : "";
                    fileNode.fileFormatIcon = dr["FILE_ICON"].ToString();
                    nodeList.Add(fileNode);
                }
            }

            return Json(nodeList);//返回节点列表json
        }


        //根据传入的类别和编号，查找对应的内容
        public ActionResult GetAttrListByMonitFile(long monitFileId)
        {
            string sql = string.Format(@"SELECT E.ID ATTR_TYPE_ID,
                           NVL (E.NAME, 'Other') ATTR_TYPE_NAME,
                           D.ID ATTR_ID,
                           D.NAME ATTR_NAME,
                            CASE WHEN D.NAME ='Length' THEN  A.ATTR_VAL||'B'
                              ELSE   A.ATTR_VAL END  ATTR_VAL
                      FROM FM_FILE_ATTR A,
                           FM_FILE_LIBRARY B,
                           FM_MONIT_FILE C,
                           FM_ATTR D LEFT JOIN FM_ATTR_TYPE E ON (D.ATTR_TYPE_ID = E.ID)
                     WHERE     A.FILE_LIBRARY_ID = B.ID
                           AND C.FILE_LIBRARY_ID = B.ID
                           AND A.ATTR_ID = D.ID
                           AND C.ID = {0}
                     ORDER BY E.ID ASC NULLS LAST", monitFileId);
            DataTable attrdt = DbHelper.ExecuteGetTable(sql);
            if (attrdt != null && attrdt.Rows.Count > 0)
            {
                return Content(JSON.DecodeToStr(attrdt));
                //return Json(attrdt);
            }
            else
                return null;
        }


        #endregion

        #region 文件监控或还原
        /// <summary>
        /// 监控并上传文件
        /// </summary>
        /// <param name="monitFileId"></param>
        /// <returns></returns>
        public string UpFileByMonitFile(long? monitFileId)
        {
            var errMsg = _MonitFileAppService.UpFileByMonitFile(monitFileId);
            return errMsg;
        }
        /// <summary>
        /// 还原文件
        /// </summary>
        /// <param name="monitFileId"></param>
        /// <returns></returns>
        public string RestoreFileByMonitFile(long? monitFileId)
        {
            var errMsg = _MonitFileAppService.RestoreFileByMonitFile(monitFileId);
            return errMsg;
        }
        #endregion




        #region 文件上传管理       
        public ActionResult UploadFile()
        {
            return View();
        }
        public ActionResult Upload()
        {
            try
            {
                string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
                var user = _UserAppService.GetUser(CurrUserId());
                string basePath = UploadPath + user.Name + "/";
                string name = string.Empty;
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                //如果目录不存在，则创建目录
                if (files != null)
                {
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    //string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string nowTime = System.Web.HttpContext.Current.Request["nowtime"];
                 
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = files[i].FileName;
                        string[] names = fileName.Split('.');
                        string trueName = names[0].ToString() +"_"+ Convert.ToDateTime(nowTime).ToString("yyyyMMddHHmmss");
                        if (fileName.Contains("."))
                        {
                            trueName += "." + names[1].ToString();
                        }

                        files[i].SaveAs(basePath + trueName);
                        FileUploadModel fileUploadModel = new FileUploadModel();
                        fileUploadModel.FileName = fileName;
                        fileUploadModel.FilePath = basePath + trueName;
                        fileUploadModel.UploadTime = Convert.ToDateTime(nowTime);
                        fileUploadModel.UserId = Convert.ToInt32(user.Id);
                        fileUploadModel.UserName = user.Name;
                        _FileUploadAppService.InsertOrUpdateFileUpload(fileUploadModel);
                    }
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("fasle;" + ex.Message);
            }

        }

        #endregion


        #region 文件预览
        //根据传入的类别和编号，查找对应的内容
        public ActionResult GetFilePathByMonitFile(long monitFileId)
        {
            string sql = string.Format(@"SELECT  MF.SERVER_PATH ,FA.NAME 
                                                 FROM  
                                                     FM_MONIT_FILE MF ,
                                                     FM_FILE_FORMAT FA  
                                                 WHERE MF.ID={0} AND
                                                     MF.FILE_FORMAT_ID=FA.ID", monitFileId);
            DataTable attrdt = DbHelper.ExecuteGetTable(sql);
            if (attrdt != null && attrdt.Rows.Count > 0)
            {
                return Content(JSON.DecodeToStr(attrdt));

            }
            else
                return null;
        }

        #region 预览Excel


        /// <summary>
        /// Index页面
        /// </summary>
        /// <param name="url">例：/uploads/......XXX.xls</param>
        public ActionResult GetHtmlUrl(string url, long monitFileId)
        {
            string physicalPath = url;
            string scode = "file" + monitFileId.ToString();
            string extension = Path.GetExtension(physicalPath);
            string htmlUrl = "";

            switch (extension.ToLower())
            {

                case ".xls":
                case ".xlsx":
                    //htmlUrl = PreviewExcel(physicalPath, url, scode);
                    htmlUrl = AsPoseHelper.GetPdfFromExcel(physicalPath, scode);
                    break;
                case ".doc":
                case ".docx":
                    // htmlUrl = PreviewWord(physicalPath, url, scode);
                    htmlUrl = AsPoseHelper.GetPdfFromWord(physicalPath, scode);
                    break;
                case ".txt":
                    //htmlUrl = PreviewTxt(physicalPath, url, scode);
                    htmlUrl = AsPoseHelper.GetPdfFromTxt(physicalPath, scode);
                    break;
                case ".pdf":
                    htmlUrl = PreviewPdf(physicalPath, url, scode);
                    break;
            }
            var surl = htmlUrl;
            return Content(surl);
        }

        /// <summary>
        /// 预览Excel
        /// </summary>
        public string PreviewExcel(string physicalPath, string url, string scode)
        {
            Microsoft.Office.Interop.Excel.Application application = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            application = new Microsoft.Office.Interop.Excel.Application();
            object missing = Type.Missing;
            object trueObject = true;
            application.Visible = false;
            application.DisplayAlerts = false;
            workbook = application.Workbooks.Open(physicalPath, missing, trueObject, missing, missing, missing,
              missing, missing, missing, missing, missing, missing, missing, missing, missing);
            //Save Excel to Html
            object format = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            string htmlName = scode + ".html";//Path.GetFileNameWithoutExtension(physicalPath)
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            workbook.SaveAs(outputFile, format, missing, missing, missing,
                     missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing,
                     missing, missing, missing, missing);
            workbook.Close();
            application.Quit();
            return Path.GetDirectoryName(Server.UrlDecode(url)) + "\\" + htmlName;
        }
        #endregion
        #region 预览Word
        /// <summary>
        /// 预览Word
        /// </summary>
        public string PreviewWord(string physicalPath, string url, string scode)
        {
            Microsoft.Office.Interop.Word._Application application = null;
            Microsoft.Office.Interop.Word._Document doc = null;
            application = new Microsoft.Office.Interop.Word.Application();
            object missing = Type.Missing;
            object trueObject = true;
            application.Visible = false;
            application.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
            doc = application.Documents.Open(physicalPath, missing, trueObject, missing, missing, missing,
              missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            //Save Excel to Html
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            string htmlName = scode + ".html";//Path.GetFileNameWithoutExtension(physicalPath)
            String outputFile = Path.GetDirectoryName(physicalPath) + "\\" + htmlName;
            doc.SaveAs(outputFile, format, missing, missing, missing,
                     missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing,
                     missing, missing, missing, missing);
            doc.Close();
            application.Quit();
            return Path.GetDirectoryName(Server.UrlDecode(url)) + "\\" + htmlName;
        }
        #endregion
        #region 预览Txt
        /// <summary>
        /// 预览Txt
        /// </summary>
        public string PreviewTxt(string physicalPath, string url, string scode)
        {
            return Server.UrlDecode(url);
        }
        #endregion
        #region 预览Pdf
        /// <summary>
        /// 预览Pdf
        /// </summary>
        public string PreviewPdf(string physicalPath, string url, string scode)
        {
            return Server.UrlDecode(url);
        }
        #endregion
        #endregion

        #region 数据库文件备份和还原SqlServer
        public string masterPath = ConfigurationManager.AppSettings["MasterPath"];

        public string UpFileByDataBase(string ip, string folderName,string fileName)
        {
            ComputerModel computer = _ComputerAppService.GetComputerByIp(ip);
            string mess = "";
            if (computer != null)
            {
                FolderModel folder = _FolderAppService.GetFolderByComputerAndName(computer.Id, folderName);
                if (folder != null)
                {
                    string fromPath = string.Format("\\\\{0}\\{1}\\{2}.bak",ip,folderName,fileName);
                    string toPath =string.Format("{0}\\DataBase", masterPath);
                    this.CheckDir(toPath);
                    mess = TransFile(computer.UserName, computer.Pwd, ip, fromPath, toPath+"\\"+fileName+".bak");
                }
                else
                {
                    mess = string.Format("结果:false;此IP({0})的终端下此共享目录不存在", ip);
                }              
            }
            else
            {
                mess= string.Format("结果:false;此IP({0})的终端在数据库中不存在", ip);
            }
            return mess;
        }

        private string TransFile(string userName, string pwd,string ip,string fromPath,string toPath)
        {
            string mess = "";
            using (SharedTool tool = new SharedTool(userName, pwd, ip))
            {
                if (System.IO.File.Exists(fromPath))
                {
                    try
                    {                     
                        System.IO.File.Copy(fromPath, toPath, true);//从客户端拷贝文件到服务端(覆盖式拷贝)
                        mess = string.Format("结果:true;此IP({0})的终端下({1})数据库处理成功", ip, fromPath);
                    }
                    catch (Exception ex)
                    {
                        mess = string.Format("结果:false;此IP({0})的终端下({1})备份文件传输失败", ip, fromPath);
                    }
                }
                else
                {
                    mess = string.Format("结果:false;此IP({0})的终端下此文件路径不存在({1})", ip, fromPath);
                }
                return mess;
            }
        }

        public void CheckDir(string directory)
        {
            //如果不存在就创建file文件夹
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

        }

        public string DownFileByDataBase(string ip, string folderName, string fileName)
        {
            ComputerModel computer = _ComputerAppService.GetComputerByIp(ip);
            string mess = "";
            if (computer != null)
            {
                FolderModel folder = _FolderAppService.GetFolderByComputerAndName(computer.Id, folderName);
                if (folder != null)
                {
                    string toPath = string.Format("\\\\{0}\\{1}\\{2}.bak", ip, folderName, fileName);
                    string fromPath = string.Format("{0}\\DataBase\\{1}.bak", masterPath,fileName);
                    if (System.IO.File.Exists(fromPath) == false)
                    {
                        mess = string.Format("结果:false;还未有此路径文件的备份({0})", fileName);
                    }
                    else
                        mess = TransFile(computer.UserName, computer.Pwd, ip, fromPath, toPath);
                }
                else
                {
                    mess = string.Format("结果:false;此IP({0})的终端下此共享目录不存在", ip);
                }
            }
            else
            {
                mess = string.Format("结果:false;此IP({0})的终端在数据库中不存在", ip);
            }
            return mess;
        }
        #endregion

    }
}
