using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：错误信息/提示信息
    /// </summary>
    public class ApiErrorBean
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool isError { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }
    }
}
