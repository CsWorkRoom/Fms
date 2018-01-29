using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
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

        /// <summary>
        /// 压缩文件目录
        /// </summary>
        /// <param name="strFile">D:\\Debug\\</param>
        /// <param name="strZip">D:\\Debug2\\a.zip</param>
        public void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            zip(strFile, s, strFile);
            s.Finish();
            s.Close();
        }

        /// <summary>
        /// 压缩文件目录
        /// </summary>
        /// <param name="strFile">D:\\Debug\\</param>
        /// <param name="strZip">D:\\Debug2\\a.zip</param>
        public void ZipFileOne(string srcFileName, string zipFileName)
        {
            FileStream srcFile = File.OpenRead(srcFileName);

            GZipOutputStream zipFile = new GZipOutputStream(File.Open(zipFileName, FileMode.Create));

            byte[] fileData = new byte[srcFile.Length];
            srcFile.Read(fileData, 0, (int)srcFile.Length);
            zipFile.Write(fileData, 0, fileData.Length);

            srcFile.Close();
            zipFile.Close();
        }


        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        #endregion

        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {

                if (Directory.Exists(file))
                {
                    zip(file, s, staticFile);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }



     


    }
}
