#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：EnumExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 15:29:47
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

#region 主体

namespace EasyMan
{
    using System;
    using System.Collections.Generic;

    public static class EnumExtensions
    {
        public static Dictionary<object, object> GetEmunDictionary(this Type enumType)
        {
            var result = new Dictionary<object, object>();
            var enumValues = Enum.GetValues(enumType);
            object keyName = null;

            foreach (var enumValue in enumValues)
            {
                var enumObject = Enum.Parse(enumType, enumValue.ToString());

                var menberInfo = enumObject.GetType().GetMember(enumObject.ToString());
                if (menberInfo.Length > 0)
                {
                    var attr = Attribute.GetCustomAttribute(menberInfo[0], typeof(EnumAttribute)) as EnumAttribute;
                    if (attr != null)
                    {
                        keyName = attr.DisPlayName;
                    }
                }

                if (keyName == null)
                {
                    keyName = Enum.GetName(enumType, enumValue);
                }
                try
                {
                    if (keyName != null) result.Add(keyName, (int)enumObject);
                }
                catch
                {
                    if (keyName != null) result.Add(keyName, enumObject);
                }
            }

            return result;
        }

        public static string GetDisplayName(this Enum e)
        {
            var keyName = "";

            var menberInfo = e.GetType().GetMember(e.ToString());
            if (menberInfo.Length <= 0) return keyName;
            var attr = Attribute.GetCustomAttribute(menberInfo[0], typeof(EnumAttribute)) as EnumAttribute;
            if (attr != null)
            {
                keyName = attr.DisPlayName;
            }

            return keyName;
        }

        public static string GetString(this bool value)
        {
            return value ? "是" : "否";
        }
    }


    public class EnumAttribute : Attribute
    {
        public EnumAttribute()
        { }

        public EnumAttribute(string displayName)
        {
            DisPlayName = displayName;
        }

        public string DisPlayName { get; set; }
    }
}
#endregion
