#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：Pager
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 16:22:17
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System;

#region 主体



namespace EasyMan.Dtos
{
    public class Pager
    {
        public Pager()
        {
        }
        public Pager(Pager pager)
        {
            PageSize = pager.PageSize;
            PageIndex = pager.PageIndex;
            TotalCount = pager.TotalCount;
        }

        public Pager(int pageSie, int pageIndex)
        {
            PageSize = pageSie;
            PageIndex = pageIndex;
        }

        public Pager(int pageSie, int pageIndex, int totalCount)
        {
            PageSize = pageSie;
            PageIndex = pageIndex;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
            }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPage; }
        }

        /// <summary>
        /// 取得记录开始行索引，开始=（当前页-1）*页面大小
        /// </summary>
        /// <returns></returns>
        public int GetRecordStartIndex()
        {
            return (PageIndex - 1) * PageSize;
        }

        /// <summary>
        /// 取得记录结束行索引，开始=当前页*页面大小
        /// </summary>
        /// <returns></returns>
        public int GetRecordEndIndex()
        {
            return PageIndex * PageSize;
        }

    }
}
#endregion
