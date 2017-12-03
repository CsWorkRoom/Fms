using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    public static class MonthBillConst
    {
        /// <summary>
        /// 生成中(固化中)
        /// </summary>
        [Description("生成中(固化中)")]
        public const string generate = "固化中";
        /// <summary>
        /// 固化失败
        /// </summary>
        [Description("固化失败")]
        public const string error = "固化失败";
        /// <summary>
        /// 固化成功
        /// </summary>
        [Description("固化成功")]
        public const string success = "固化成功";
    }
}
