using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shell32;

namespace Easyman.Service
{
   public class FileTool
    {


        /// <summary>

        /// 获取文件属性字典

        /// </summary>

        /// <param name="filePath">文件路径</param>

        /// <returns>属性字典</returns>

        public static Dictionary<string, string> GetProperties(string filePath)
        {         
                if (!File.Exists(filePath))                {
                    throw new FileNotFoundException("指定的文件不存在。", filePath);
                }
            FileInfo info = new FileInfo(filePath);
           // System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
            Dictionary<string, string> Properties = new Dictionary<string, string>();
            //Properties.Add("Comments", info.Comments);
            //Properties.Add("CompanyName", info.CompanyName);
            //Properties.Add("FileBuildPart", info.FileBuildPart.ToString());
            //Properties.Add("FileDescription", info.FileDescription);
            //Properties.Add("FileMajorPart", info.FileMajorPart.ToString());
            PropertyInfo[] propertys = info.GetType().GetProperties();
            foreach (PropertyInfo item  in propertys)
            {
                object svalue = item.GetValue(info, null);//用pi.GetValue获得值
                string tvalue = svalue != null? svalue.ToString(): "" ;  
                Properties.Add(item.Name, tvalue);
             
               
            }

            //s += "f1";
            //      //初始化Shell接口 
            //Shell32.Shell shell = new Shell32.ShellClass();
            ////获取文件所在父目录对象 
            //Folder folder = shell.NameSpace(Path.GetDirectoryName(filePath));
            //s += "f2"+folder==null?"NULL":"ok";
            //string surl = Path.GetFileName(filePath);
            //s += surl;
            ////获取文件对应的FolderItem对象 
            //FolderItem item = folder.ParseName(Path.GetFileName(filePath));
            //s += "f3";
            ////字典存放属性名和属性值的键值关系对 
            //Dictionary<string, string> Properties = new Dictionary<string, string>();
            //int i = 0;
            //while (true)
            //{
            //    //获取属性名称 
            //    string key = folder.GetDetailsOf(null, i);
            //    if (string.IsNullOrEmpty(key))
            //    {
            //        //当无属性可取时，退出循环 
            //        break;
            //    }                               
            //    //获取属性值 
            //    string value = folder.GetDetailsOf(item, i);
            //    //保存属性 
            //    if (Properties.Where(e => e.Key == key).Count() == 0 && !string.IsNullOrEmpty(value) && value != "")
            //        Properties.Add(key, value);
            //    i++;
            //}
            return Properties;
        }
        ///// <summary>
        ///// 存储属性名与其下标（key值均为小写）
        ///// </summary>
        //private static Dictionary<string, int> _propertyIndex = null;
        ///// <summary>
        ///// /// 获取指定文件指定下标的属性值
        ///// </summary>
        ///// <param name="filePath">文件路径</param>
        ///// <param name="index">属性下标</param>
        ///// <returns>属性值</returns>
        //public static string GetPropertyByIndex(string filePath, int index)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        throw new FileNotFoundException("指定的文件不存在。", filePath);
        //    }
        //    //初始化Shell接口 
        //    Shell32.Shell shell = new Shell32.ShellClass();
        //    //获取文件所在父目录对象 
        //    Folder folder = shell.NameSpace(Path.GetDirectoryName(filePath));
        //    //获取文件对应的FolderItem对象 
        //    FolderItem item = folder.ParseName(Path.GetFileName(filePath));
        //    string value = null;
        //    //获取属性名称 
        //    string key = folder.GetDetailsOf(null, index);
        //    if (false == string.IsNullOrEmpty(key))
        //    {
        //        //获取属性值 
        //        value = folder.GetDetailsOf(item, index);
        //    }
        //    return value;
        //}

        ///// <summary>
        ///// 获取指定文件指定属性名的值
        ///// </summary>
        ///// <param name="filePath">文件路径</param>
        //        /// <param name="propertyName">属性名</param>
        ///// <returns>属性值</returns>
        //public static string GetPropertyEx(string filePath, string propertyName)
        //{
        //    if (_propertyIndex == null)
        //    {
        //        InitPropertyIndex();
        //    }
        //    //转换为小写
        //    string propertyNameLow = propertyName.ToLower();
        //    if (_propertyIndex.ContainsKey(propertyNameLow))
        //    {
        //        int index = _propertyIndex[propertyNameLow];
        //                        return GetPropertyByIndex(filePath, index);
        //    }
        //    return null;
        //}       

        ///// <summary>
        ///// 初始化属性名的下标
        ///// </summary>
        //private static void InitPropertyIndex()
        //{
        //    Dictionary<string, int> propertyIndex = new Dictionary<string, int>();
        //    //获取本代码所在的文件作为临时文件，用于获取属性列表
        //    string tempFile = System.Reflection.Assembly.GetExecutingAssembly().FullName;

        //    Dictionary<string, string> allProperty = GetProperties(tempFile);
        //    if (allProperty != null)
        //    {
        //        int index = 0;
        //        foreach (var item in allProperty.Keys)
        //        {
        //            //属性名统一转换为小写，用于忽略大小写
        //            _propertyIndex.Add(item.ToLower(), index);
        //            index++;
        //        }
        //    }
        //    _propertyIndex = propertyIndex;
        //}


        /// <summary>
        /// 获取文件夹属性
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryByDir(string dirPath)
        {
            Dictionary<string, string> Properties = new Dictionary<string, string>();
            var dicInfo = new DirectoryInfo(dirPath);//获取的目录信息 
            Properties.Add("CreationTime", dicInfo.CreationTime.ToString());
            Properties.Add("Extension", dicInfo.Extension.ToString());
            Properties.Add("FullName", dicInfo.FullName.ToString());
            Properties.Add("LastAccessTime", dicInfo.LastAccessTime.ToString());
            Properties.Add("LastWriteTime", dicInfo.LastWriteTime.ToString());
            Properties.Add("Name", dicInfo.Name.ToString());
            Properties.Add("Root", dicInfo.Root.ToString());
            return Properties;
        }

        /// <summary>
        /// 计算文件的hash值 用于比较两个文件是否相同
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件hash值</returns>
        public static string GetFileHash(string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var hash = System.Security.Cryptography.HashAlgorithm.Create();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
