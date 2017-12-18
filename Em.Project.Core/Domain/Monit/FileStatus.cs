using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Domain
{
    public enum FileStatus
    {
        /// <summary>
        /// 文件删除
        /// </summary>
        Delete,
        /// <summary>
        /// 新增文件
        /// </summary>
        Add,
        /// <summary>
        /// 修改文件
        /// </summary>
        Modify
    }
}
