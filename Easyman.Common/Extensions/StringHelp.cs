using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace EasyMan.Extensions
{
    public class StringHelp
    {
        /// <summary>
        /// 将一个字符串转换成64位编码的字符串
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>返回64位编码一个字符串</returns>
        public static string Base64StringEncode(string input)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// 将一个64位编码的字符串解码
        /// </summary>
        /// <param name="input">64位编码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string Base64StringDecode(string input)
        {
            byte[] decbuff = Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        /// <summary>
        /// 替换功能
        /// </summary>
        /// <param name="input">检查字符串</param>
        /// <param name="newValue">替换的值</param>
        /// <param name="oldValue">将要插入的新值</param>
        /// <returns>A string</returns>
        public static string CaseInsenstiveReplace(string input,
           string newValue, string oldValue)
        {
            Regex regEx = new Regex(oldValue,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, newValue);
        }

        /// <summary>
        /// 通过正则表达式分割字符串
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="regularExpression">正则表达式</param>
        /// <returns>返回一个分割后的字符串</returns>
        public static string GetRegularExpressionByBrokenString(string str,
           string regularExpression)
        {
            string[] temp = System.Text.RegularExpressions.Regex.Split(str, regularExpression);
            return temp.Aggregate("", (current, item) => current + item.ToString());
        }

        /// <summary>
        /// 通过正则表达式分割字符串并返回一个数组
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="regularExpression">正则表达式</param>
        /// <returns>返回一个分割后的字符串</returns>
        public static string[] GetRegularExpressionByBrokenStringArray(string str,
           string regularExpression)
        {
            return System.Text.RegularExpressions.Regex.Split(str, regularExpression);
        }

        /// <summary>
        /// 根据条件分割字符串
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="conditionValue">分割条件值</param>
        /// <returns>返回一个分割后的字符串</returns>
        public static string GetValueByBrokenString(string str,
           string conditionValue)
        {
            string[] sArray = str.Split(conditionValue);
            return sArray.Aggregate("", (current, i) => current + i);
        }

        /// <summary>
        /// 根据条件分割字符串并返回一个数组
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="conditionValue">分割条件值</param>
        /// <returns>返回一个分割后的字符串</returns>
        public static string[] GetValueByBrokenStringArray(string str,
           string conditionValue)
        {
            return str.Split(conditionValue);
        }

        /// <summary>
        /// 获取指定字节长度的中英文混合字符串
        /// </summary>
        private string GetString(string str, int len)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }
                if (pos >= 0)
                    result = str.Substring(0, pos);
            }
            else
                result = str;
            return result;
        }

        ///   <summary>   
        ///   将指定字符串按指定长度进行截取并加上指定的后缀  
        ///   </summary>   
        /// <param name="oldStr">需要截断的字符串</param>
        /// <param name="maxLength">字符串的最大长度</param>
        /// <param name="endWith">超过长度的后缀</param>
        /// <returns> 如果超过长度，返回截断后的新字符串加上后缀，否则，返回原字符串 </returns>   
        public static string StringTruncat(string oldStr, int maxLength, string endWith)
        {
            //判断原字符串是否为空  
            if (string.IsNullOrEmpty(oldStr))
                return oldStr + endWith;


            //返回字符串的长度必须大于1  
            if (maxLength < 1)
                throw new Exception("返回的字符串长度必须大于[0] ");


            //判断原字符串是否大于最大长度  
            if (oldStr.Length > maxLength)
            {
                //截取原字符串  
                string strTmp = oldStr.Substring(0, maxLength);


                //判断后缀是否为空  
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                return strTmp + endWith;
            }
            return oldStr;
        }

        /// <summary>
        /// 将一个字符集转换成UTF-8编码的字符集
        /// </summary>
        /// <param name="unicodeString">需要转换的字符集</param>
        /// <returns>返回一个UTF-8的字符集</returns>
        public static string Get_Utf8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }

        /// <summary>
        /// 字符串转为二进制
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>转换后的二进制</returns>
        public static string StringToBinary(string str)
        {
            byte[] data = Encoding.Unicode.GetBytes("对与错！");
            StringBuilder strResult = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                strResult.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            string binary = strResult.ToString();
            return binary;
        }

        /// <summary>
        /// 二进制转为字符串
        /// </summary>
        /// <param name="str">二进制</param>
        /// <returns>转换后的字符串</returns>
        public static string BinaryToString(string str)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
            System.Text.RegularExpressions.Regex.Match(str, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            string result = Encoding.Unicode.GetString(data, 0, data.Length);
            return result;
        }

        /// <summary>
        /// 把byte转化成十六进制string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string EncodingSms(string s)
        {
            string result = string.Empty;

            var arrByte = System.Text.Encoding.GetEncoding("GB2312").GetBytes(s);

            return arrByte.Aggregate(result, (current, t) => current + System.Convert.ToString(t, 16));
        }

        /// <summary>
        /// 把十六进制string转化成byte 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string DecodingSms(string s)
        {
            var arrByte = new byte[s.Length / 2];
            int index = 0;
            for (int i = 0; i < s.Length; i += 2)
            {
                arrByte[index++] = Convert.ToByte(s.Substring(i, 2), 16);        //Convert.ToByte(string,16)把十六进制string转化成byte 
            }
            var result = System.Text.Encoding.Default.GetString(arrByte);

            return result;

        }

        /// <summary>
        /// 将字符串转成二进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Bianma(string s)
        {
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }

        /// <summary>
        /// 将二进制转成字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string jiema(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }


    }
}
