using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：键值对
    /// </summary>
    public class ApiKeyValueBean
    {
        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public IList<ApiKeyValueBean> child { get; set; }
    }
}
