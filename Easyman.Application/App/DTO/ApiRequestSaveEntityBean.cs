using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：客户端请求，保存Entity
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ApiRequestSaveEntityBean<T>
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
        /// 参数
        /// </summary>
        public IList<ApiKeyValueBean> para { get; set; }

        /// <summary>
        /// 泛型对象
        /// </summary>
        public T entity { get; set; }
    }
}
