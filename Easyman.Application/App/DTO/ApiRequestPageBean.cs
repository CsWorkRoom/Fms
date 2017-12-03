using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：客户端请求，获取分页数据
    /// </summary>
    public class ApiRequestPageBean
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string authToken { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public List<ApiKeyValueBean> attachParams { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 查询关键字（非查询时为空）
        /// </summary>
        public List<ApiKeyValueBean> searchKey { get; set; }

        /// <summary>
        /// 排序（desc/asc）
        /// </summary>
        public List<ApiKeyValueBean> orderBy { get; set; }

    }
}
