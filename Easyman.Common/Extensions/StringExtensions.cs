#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：StringExtensions
* 版本：4.0.30319.42000
* 作者：zhl 时间：2016/12/5 15:18:15
* ========================================================================
*/
#endregion

using System;
using System.Security.Cryptography;
using System.Text;
using System.Data;

#region 主体

using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using EasyMan.Extensions;

namespace EasyMan
{

    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string FormatWith(this string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
                return string.Empty;

            if (args == null)
                return format;

            if (!args.Any())
                return format;

            return string.Format(format, args);
        }

        public static List<T> JsonToList<T>(this string json)
        {
            return JsonToList<T>(json, true);
        }

        public static List<T> JsonToList<T>(this string json, bool isReplace)
        {
            if (isReplace)
                json = json.Replace('\'', '\"');

            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static T JsonToEntity<T>(this string json)
        {
            return JsonToEntity<T>(json, true);
        }
        public static T JsonToEntity<T>(this string json, bool isReplace)
        {
            if (isReplace)
                json = json.Replace('\'', '\"');

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Md5Encrypt(this string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(strText));
            md5.Dispose();
            var sBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 不允许空字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> 为空。</exception>
        public static string NotEmpty(this string str, string name)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(name.NotEmpty("name"));
            return str;
        }

        /// <summary>
        /// 不允许空和只包含空格的字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> 为空或者全为空格。</exception>
        public static string NotEmptyOrWhiteSpace(this string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(name.NotEmpty("name"));
            return str;
        }

        /// <summary>
        /// 手机号加密（中间4位*号）
        /// </summary>
        /// <param name="mobileNo">要加密的手机号</param>
        public static string PhoneEncrypt(this string mobileNo)
        {
            if (!string.IsNullOrWhiteSpace(mobileNo) && mobileNo.Length > 7)
                return mobileNo.Substring(0, 3) + "****" + mobileNo.Substring(7, mobileNo.Length - 7);
            return mobileNo;
        }

        /// <summary>
        /// 是否空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 是否非空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

       /// <summary>
       /// 空字符串时返回null
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
        public static string NullIfEmpty(this string value)
        {
            if (value == string.Empty)
                return null;

            return value;
        }
        
        /// <summary>
        /// 字符串是否数字与字母组成
        /// </summary>
        /// <param name="value">源字符串</param>
        /// <returns></returns>
        public static bool IsSlug(this string value)
        {
            if (RegexUtils.SlugRegex.IsMatch(value))
                return true;
            return false;
        }

        /// <summary>
        /// 是否Ip地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIPAddress(this string value)
        {
            if (RegexUtils.IPAddressRegex.IsMatch(value))
                return true;
            return false;
        }

        /// <summary>
        /// 是否Email地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            if (RegexUtils.EmailRegex.IsMatch(value))
                return true;
            return false;
        }

        /// <summary>
        /// 是否Url地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(this string value)
        {
            if (RegexUtils.UrlRegex.IsMatch(value))
                return true;
            return false;
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this string value)
        {
            if (RegexUtils.PositiveNumberRegex.IsMatch(value))
                return true;
            return false;
        }


        /// <summary>
        /// 分隔字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separators">分隔符</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndTrim(this string value, params char[] separators)
        {
            return value.Trim().Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        /// <summary>
        /// 返回非number类型的sql in字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separators">分隔符</param>
        /// <returns></returns>
        public static string StringToSqlIn(this string value, params char[] separators)
        {
            var list = value.SplitAndTrim(separators).ToList();
            if (list.Count() == 0)
                return value;
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = "'{0}'".FormatWith(list[i]);
            }
            return string.Join(",", list);
        }


        public static string IntToSqlIn(this string value, params char[] separators)
        {
            var list = value.SplitAndTrim(separators).ToList();
            if (list.Count() == 0)
                return value;
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = "{0}".FormatWith(list[i]);
            }
            return string.Join(",", list);
        }
    }
}
#endregion
