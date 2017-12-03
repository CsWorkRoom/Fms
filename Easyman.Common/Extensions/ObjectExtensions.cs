#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ObjectExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/1/28 15:06:45
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

#region 主体

namespace EasyMan
{
    using System;

    public static class ObjectExtensions
    {
        public static T ToConvertOrDefault<T>(this object obj, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this object obj, DateTime defaultValue)
        {
            if (obj == null)
                return defaultValue;
            DateTime result;

            if (DateTime.TryParse(obj.ToString(), out result))
                return result;

            return defaultValue;
        }

        public static double ToDouble(this object obj, double defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;
            double result;

            if (Double.TryParse(obj.ToString(), out result))
                return result;

            return defaultValue;
        }
        public static int ToInt32(this object obj, int defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;
            int result;

            if (int.TryParse(obj.ToString(), out result))
                return result;

            return defaultValue;
        }

        public static double ToTenThousand(this object obj, int defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;
            double result;
            if (double.TryParse(obj.ToString(), out result))
            {
                return result / 10000;
            }
            return defaultValue;
        }

        public static T ToEnum<T>(this object value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString());
        }
        
        /// <summary>
        /// 不允许为Null。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="instance">对象实例。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>对象实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> 为null。</exception>
        public static T NotNull<T>(this T instance, string name) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(name.NotEmpty("name"));
            return instance;
        }
    }
}
#endregion
