using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：登录信息
    /// </summary>
    public class ApiLoginBean
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        public string tenancyname { get; set; }

        /// <summary>
        /// 手机IMEI
        /// </summary>
        public string imei { get; set; }

        /// <summary>
        /// 当前手机版本
        /// </summary>
        public string version { get; set; }

    }
}
