using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Checksums;
using System.Collections;

namespace EasyMan.Extensions
{
    /// <summary>
    /// 压缩解压
    /// </summary>
    public static class ZipExtensions
    {
        /// <summary>
        /// 解压zip文件到指定路径
        /// s</summary>
        /// <param name="zipFileName">zip文件</param>
        /// <param name="dstFileName">解压路径</param>
        /// <returns></returns>
        public static bool UnZip(string zipFileName, string dstFileName)
        {
            if (!File.Exists(zipFileName))
                return false;

            ZipEntry theEntry;
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFileName));
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = dstFileName;//Path.GetDirectoryName(dstFileName);
                directoryName += Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
                //生成解压目录
                Directory.CreateDirectory(directoryName);
                if (fileName != String.Empty)
                {
                    //解压文件到指定的目录
                    FileStream streamWriter = File.Create(dstFileName + theEntry.Name);
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();
            return true;
        }
        /// <summary>
        /// 取zip文件中的指定文件
        /// Android *.apk AndroidManifest.xml
        /// iOS *.ipa iTunesMetadata.plist
        /// WP *.* AppManifest.xaml
        /// </summary>
        /// <param name="zipFileName">zip文件</param>
        /// <param name="innerFileName">需要取的文件名</param>
        /// <param name="fuzzySame">模糊比较文件名</param>
        /// <returns></returns>
        public static byte[] ReadInnerFileBytes(string zipFileName, string innerFileName, bool fuzzySame)
        {
            if (!File.Exists(zipFileName))
                return null;

            innerFileName = innerFileName.ToLower();
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFileName)))
            {
                ZipEntry entry;//AndroidManifest.xml
                while ((entry = s.GetNextEntry()) != null)
                {
                    string srcName = entry.Name.ToLower();

                    if (entry.Name == innerFileName || fuzzySame && srcName.IndexOf(innerFileName) >= 0)
                    {
                        List<byte> dyns = null;
                        byte[] buff = new byte[10240];
                        bool isFirst = true;

                        while (true)
                        {
                            int size = s.Read(buff, 0, 10240);
                            if (size > 0)
                            {
                                if (isFirst && size < 10240)
                                {
                                    byte[] rr = new byte[size];
                                    Array.Copy(buff, rr, size);
                                    return rr;
                                }
                                isFirst = false;
                                if (dyns == null)
                                    dyns = new List<byte>(10240 * 2);
                                if (size == 10240)
                                    dyns.AddRange(buff);
                                else
                                {
                                    for (int i = 0; i < size; i++)
                                        dyns.Add(buff[i]);
                                }
                            }
                            else
                                break;
                        }

                        return dyns != null ? dyns.ToArray() : null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFile">待压缩文件</param>
        /// <param name="destinationFile">目标文件</param>
        public static void CompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException();

            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream compressedStream = null;
            try
            {
                // Read the bytes from the source file into a byte array
                sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Read the source stream values into the buffer
                byte[] buffer = new byte[sourceStream.Length];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);

                if (checkCounter != buffer.Length)
                    throw new ApplicationException();

                // Open the FileStream to write to
                destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);

                // Create a compression stream pointing to the destiantion stream
                compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);

                // Now write the compressed data to the destination file
                compressedStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();

                if (compressedStream != null)
                    compressedStream.Close();

                if (destinationStream != null)
                    destinationStream.Close();
            }
        }
        /// <summary>
        /// 压缩文件夹的方法
        /// </summary>
        /// <param name="DirToZip">待压缩的文件夹</param>
        /// <param name="ZipedFile">压缩后的文件名称</param>
        public static bool ZipDir(string DirToZip, string ZipedFile, int CompressionLevel)
        {
            bool is_bool = true;
            //如果文件没有找到，则提示 
            if (!Directory.Exists(DirToZip))
            {
                //待压缩文件不存在
                is_bool = false;
            }
            else
            {
                if (DirToZip.LastIndexOf("//") == DirToZip.Length - 1)
                {
                    DirToZip = DirToZip.Substring(0, DirToZip.Length - 1);
                }
                //压缩文件为空时默认与压缩文件夹同一级目录
                if (ZipedFile == string.Empty)
                {
                    /**获得目录的名称**/
                    ZipedFile = DirToZip.Substring(DirToZip.LastIndexOf("//") + 1);
                    ZipedFile = DirToZip.Substring(0, DirToZip.LastIndexOf("//")) + Path.DirectorySeparatorChar + ZipedFile + ".zip";
                }
                if (Path.GetExtension(ZipedFile) != ".zip")
                {
                    ZipedFile = ZipedFile + ".zip";
                }
                using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(ZipedFile)))
                {
                    try
                    {
                        /**设置文件压缩级别**/
                        zipoutputstream.SetLevel(CompressionLevel);
                        Crc32 crc = new Crc32();
                        Hashtable fileList = getAllFies(DirToZip);
                        foreach (DictionaryEntry item in fileList)
                        {
                            FileStream fs = File.OpenRead(item.Key.ToString());
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length));
                            entry.DateTime = (DateTime)item.Value;
                            entry.Size = fs.Length;
                            fs.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            entry.Crc = crc.Value;
                            zipoutputstream.PutNextEntry(entry);
                            zipoutputstream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        is_bool = false;
                    }
                }
            }
            return is_bool;
        }


        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="sourceFile">已压缩文件</param>
        /// <param name="destinationFile">目标文件</param>
        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException();

            // Create the streams and byte arrays needed
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream decompressedStream = null;

            try
            {
                // Read in the compressed source stream
                sourceStream = new FileStream(sourceFile, FileMode.Open);

                // Create a compression stream pointing to the destiantion stream
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);

                // Read the footer to determine the length of the destiantion file
                byte[] quartetBuffer = new byte[4];
                int position = (int)sourceStream.Length - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);

                byte[] buffer = new byte[checkLength + 100];

                int offset = 0;
                int total = 0;

                // Read the compressed data into the buffer
                while (true)
                {
                    int bytesRead = decompressedStream.Read(buffer, offset, 100);

                    if (bytesRead == 0)
                        break;

                    offset += bytesRead;
                    total += bytesRead;
                }

                // Now write everything to the destination file
                destinationStream = new FileStream(destinationFile, FileMode.Create);
                destinationStream.Write(buffer, 0, total);

                // and flush everyhting to clean out the buffer
                destinationStream.Flush();
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();

                if (decompressedStream != null)
                    decompressedStream.Close();

                if (destinationStream != null)
                    destinationStream.Close();
            }

        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        public static byte[] CompressString(string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            if (source.Length > 128)
            {
                MemoryStream destinationStream = new MemoryStream();
                using (GZipStream gzip = new GZipStream(destinationStream, CompressionMode.Compress))
                {
                    byte[] buf = Encoding.UTF8.GetBytes(source);
                    gzip.Write(buf, 0, buf.Length);
                    gzip.Flush();
                }
                return destinationStream.ToArray();
            }
            else
            {
                byte[] temp = Encoding.UTF8.GetBytes(source);
                byte[] result = new byte[temp.Length + 4];
                result[0] = 0x0;
                result[1] = 0x0;
                result[2] = 0x0;
                result[3] = 0x0;
                for (int i = 0; i < temp.Length; i++)
                    result[i + 4] = temp[i];
                return result;
            }
        }
        /// <summary>
        /// 解压字符串
        /// </summary>
        public static string DecompressString(byte[] source)
        {
            if (source == null || source.Length == 0)
                return string.Empty;
            if (source.Length < 4)
                throw new NotImplementedException();
            if (source[0] == 0 && source[1] == 0 && source[2] == 0 && source[3] == 0)
                return Encoding.UTF8.GetString(source, 4, source.Length - 4);
            GZipStream gzip = new GZipStream(new MemoryStream(source), CompressionMode.Decompress);
            using (StreamReader reader = new StreamReader(gzip))
            {
                return reader.ReadToEnd();
            }
        }

        public static bool ZipFileMain(string zipDir, string zipFile)
        {
            string[] filenames = Directory.GetFiles(zipDir, "*.*", SearchOption.AllDirectories);

            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(zipFile));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            foreach (string file in filenames)
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(file.Replace(zipDir, ""));
                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();

                crc.Reset();
                crc.Update(buffer);

                entry.Crc = crc.Value;

                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);
            }
            s.Finish();
            s.Close();
            return true;
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        private static Hashtable getAllFies(string dir)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            getAllDirFiles(fileDire, FilesList);
            getAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        /// <summary>
        /// 获取一个文件夹下的所有文件夹里的文件
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="filesList"></param>
        private static void getAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    if (file.FullName.EndsWith("zip"))
                    {
                        continue;
                    }
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                getAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>
        /// 获取一个文件夹下的文件
        /// </summary>
        /// <param name="strDirName">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>
        private static void getAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                if (file.FullName.EndsWith("zip"))
                {
                    continue;
                }
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }


        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceDirName">源目录</param>
        /// <param name="destDirName">新目录</param>
        /// <param name="copySubDirs">是否复制子目录</param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, true);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    //do not copy svn files. update by huzhenwu 2011-11-22
                    if (subdir.Name.Contains("svn"))
                    {
                        continue;
                    }
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}