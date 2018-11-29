using Easyman.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Easyman.Web.Controllers
{
    /// <summary>
    /// 本地计算机文件夹及文件管理器
    /// </summary>
    public class PcFileController : Controller
    {
        // GET: PcFile
        public ActionResult Index()
        {
            ViewBag.DbPath = MvcApplication.DbPath;
            return View();
        }

        /// <summary>
        /// 获取子目录及文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSubs(string path)
        {
            List<SubInfo> list = new List<SubInfo>();
            if (string.IsNullOrWhiteSpace(path))
            {
                foreach (System.IO.DriveInfo di in DriveInfo.GetDrives())
                {
                    list.Add(new SubInfo { Type = 1, Name = di.Name, FullName = di.Name });
                }
            }
            else
            {
                if (Directory.Exists(path) == true)
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (var d in di.GetDirectories())
                    {
                        list.Add(new SubInfo { Type = 1, Name = d.Name, FullName = d.FullName, CreateTime = d.CreationTime, UpdateTime = d.LastWriteTime });
                    }

                    foreach (var f in di.GetFiles())
                    {
                        list.Add(new SubInfo { Type = 2, Name = f.Name, FullName = f.FullName, Size = f.Length, CreateTime = f.CreationTime, UpdateTime = f.LastWriteTime });
                    }
                }
            }

            string result = "{\"records\":" + list.Count + ",\"rows\":" + Common.JSON.DecodeToStr(list) + "}";
            return result;
        }
    }
}