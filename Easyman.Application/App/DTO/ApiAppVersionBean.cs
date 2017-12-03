using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：APP版本信息
    /// </summary>
    public class ApiAppVersionBean
    {
        /// <summary>
        /// 是否可更新：有更高版本可用时为true
        /// </summary>
        public bool Is_Upgrade { get; set; }

        /// <summary>
        /// 是否要强制更新：需停止使用老板本时为true
        /// </summary>
        public bool IS_MUST { get; set; }

        /// <summary>
        /// 最新版本名或版本号 eg:1.0.0
        /// </summary>
        public string VERSION_NAME { get; set; }

        /// <summary>
        /// 最新版本编码（版本唯一标识整数数字）
        /// </summary>
        public string VERSION_CODE { get; set; }

        /// <summary>
        /// 更新地址（绝对路径）
        /// </summary>
        public string UPDATE_URL { get; set; }

        /// <summary>
        /// 更新日志
        /// </summary>
        public string UPGRADE_LOG { get; set; }

    }
}
