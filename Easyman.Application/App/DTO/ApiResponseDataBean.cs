using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：服务端响应数据
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ApiResponseDataBean<T>
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 是否出错
        /// </summary>
        public bool isError { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 实际返回的数据
        /// </summary>
        public T data { get; set; }

    }
}
