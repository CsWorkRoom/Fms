#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ISearchExtension
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 17:19:55
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体



namespace EasyMan
{
    using Dtos;
    using EasyQuery;
    using System.Collections.Generic;
    using System.Linq;
    using Linq;


    public static class SearchExtension
    {
        public static IQueryable<T> SearchByInputDtoNoPager<T>(this IQueryable<T> source, SearchInputDto input)
        {
            return source.Search(input.SearchList).Order(input.Order);
        }

        public static IQueryable<T> SearchByInputDto<T>(this IQueryable<T> source, SearchInputDto input, out int reordCount)
        {
            reordCount = source.Search(input.SearchList).Order(input.Order).Count();
            return source.Search(input.SearchList).Order(input.Order).Page(input.Page);
        }

        public static IQueryable<T> SearchByInputDtoNoPager<T>(this IQueryable<T> source, SearchInputDto<long> input)
        {
            return source.Search(input.SearchList).Order(input.Order);
        }

        public static IQueryable<T> SearchByInputDto<T>(this IQueryable<T> source, SearchInputDto<long> input, out int reordCount)
        {
            reordCount = source.Search(input.SearchList).Order(input.Order).Count();
            return source.Search(input.SearchList).Order(input.Order).Page(input.Page);
        }

        private static IQueryable<T> Search<T>(this IQueryable<T> source, IList<SearchFilter> searchColumns)
        {
            if (!searchColumns.Any())
                return source;

            var filterExpressions = searchColumns.Select((a, i) => a.GetExpression().FormatWith(a.Name, "@" + i));
            var values = searchColumns.Select(s => s.TValue).ToArray();
            var whereQuery = string.Join(" and ", filterExpressions);

            return source.Where(whereQuery, values);
        }

        private static IQueryable<T> Order<T>(this IQueryable<T> source, Order order)
        {
            return order != null ? source.OrderBy("{0} {1}".FormatWith(order.Name, order.Type)) : source;
        }

        private static IQueryable<T> Page<T>(this IQueryable<T> source, Pager pager)
        {
            return source.Skip(pager.GetRecordStartIndex()).Take(pager.PageSize);
        }
    }
}
#endregion
