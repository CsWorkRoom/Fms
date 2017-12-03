using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：服务端响应，分页数据
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ApiPagingDataBean<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int totalCount { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> data { get; set; }

    }
}
