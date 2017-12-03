#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：EnumerableExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 15:27:50
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Linq;

#region 主体



namespace EasyMan
{
    using System;
    using System.Collections.Generic;


    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
                action(t);
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in source)
                action(item, index++);
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}
#endregion
