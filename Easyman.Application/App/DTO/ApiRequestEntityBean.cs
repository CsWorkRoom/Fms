using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：客户端请求，获取单个Entity
    /// </summary>
    public class ApiRequestEntityBean
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
        /// 参数
        /// </summary>
        public IList<ApiKeyValueBean> para { get; set; }
    }
}
