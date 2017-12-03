using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common.Helper
{
   public class ZipHelper
    {
        #region 压缩文件包
        /// <summary>
        /// 压缩文件包
        /// </summary>
        /// <param name="strFiles">压缩文件集（格式例:E:\Word\6\201706291055551704.csv|E:\Word\6\201706291055587790.csv|E:\Word\6\201706291056024561.csv）</param>
        /// <param name="strMapPath">返回文件保存的物理路径值</param>
        public void ZipFiles(string strFiles, string strMapPath)
        {
            if (string.IsNullOrEmpty(strFiles))
            {
                // Logger.Debug("目标压缩文件集为空，压缩失败。");//日志文件
                return;
            }
            try
            {
                string[] filenames = strFiles.Split('|');
                using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(strMapPath)))
                {
                    s.SetLevel(9); // 压缩级别 0-9
                    //s.Password = "123"; //Zip压缩文件密码
                    byte[] buffer = new byte[4096]; //缓冲区大小
                    foreach (string file in filenames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = (System.IO.File.OpenRead(file)))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                        System.IO.File.Delete(file);//删除已被压缩的散文件
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                // Logger.Debug("文件集压缩出现异常，压缩失败。错误信息：" + ex);//日志文件
            }
        }
        #endregion

       


    }
}
