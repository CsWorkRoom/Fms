using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    /// <summary>
    /// 查询参数类
    /// </summary>
    public class QueryParam
    {
        /// <summary>
        /// 字段编码
        /// </summary>
        public string FieldCode { get; set; }
        /// <summary>
        /// 参数代码
        /// </summary>
        public string FieldParam { get; set; }
        /// <summary>
        /// 操作类型-条件
        /// </summary>
        public string OpType { get; set; }
        /// <summary>
        /// 控件值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 筛选类型
        /// </summary>
        public string FilterType { get; set; }
    }
}
