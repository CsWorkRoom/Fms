﻿#region 版本信息
/************************************************
*CLR Version:   4.0.30319.18449
*NameSpace:   FileHelper
*FileName:       FileEncoding.cs
*Author:           danlincao
*Email:             danlincao@easyman.com.cn
*Company:	    成都联宇创新科技有限公司
*CompanySite:	www.easyman.com.cn
*CreateTime:	    2015/5/12 22:30:47
*Description:		
*
*Modify UserName:
*
*Modify History:
*
***********************************************/
#endregion

#region 引用
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace EasyMan
{
    /// <summary>
    /// 获取文件的编码格式
    /// </summary>
    public class EncodingType
    {
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(FileStream fs)
        {
            var unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            var unicodeBig = new byte[] { 0xFE, 0xFF, 0x00 };
            var utf8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            Encoding reVal = Encoding.Default;

            var r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUtf8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUtf8Bytes(IEnumerable<byte> data)
        {
            var charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            foreach (byte t in data)
            {
                var curByte = t; //当前分析的字节.
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}
