using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：文件信息
    /// </summary>
    public class ApiFileBean
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string PATH { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long USER_ID { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long LENGTH { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime UPLOAD_TIME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FILE_TYPE { get; set; }

    }
}
