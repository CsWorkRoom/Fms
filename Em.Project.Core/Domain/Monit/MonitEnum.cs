using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Domain
{
    /// <summary>
    /// 文件监控状态（FM_MONIT_FILE的STATUS字段）
    /// </summary>
    public enum MonitStatus
    {
        /// <summary>
        /// 无变化
        /// </summary>
        [Description("无变化")]
        UnChanged = 0,
        /// <summary>
        /// 文件删除
        /// </summary>
        [Description("文件被删除")]
        Delete = 1,
        /// <summary>
        /// 新增文件
        /// </summary>
        [Description("新增文件")]
        Add = 2,
        /// <summary>
        /// 修改文件
        /// </summary>
        [Description("文件被修改")]
        Modify = 3
    }
    /// <summary>
    /// 拷贝状态
    /// </summary>
    public enum CopyStatus
    {
        /// <summary>
        /// 等待被拷贝
        /// </summary>
        [Description("等待被拷贝")]
        Wait = 0,
        /// <summary>
        /// 拷贝成功
        /// </summary>
        [Description("拷贝成功")]
        Success = 1,
        /// <summary>
        /// 拷贝中
        /// </summary>
        [Description("拷贝中")]
        Excuting = 2,
        /// <summary>
        /// 拷贝失败
        /// </summary>
        [Description("拷贝失败")]
        Fail = 3,
        /// <summary>
        /// 源文件不存在
        /// </summary>
        [Description("源文件不存在")]
        NotExist = 4
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 监控日志类型
        /// </summary>
        [Description("监控日志类型")]
        MonitLog = 0,
        /// <summary>
        /// 上传（客户端到服务端拷贝）
        /// </summary>
        [Description("上传（客户端到服务端拷贝）")]
        UpLog = 1,
        /// <summary>
        /// 还原（服务端到客户端还原）
        /// </summary>
        [Description("还原（服务端到客户端还原）")]
        RestoreLog = 2,
        /// <summary>
        /// 下载（客户web上下载）
        /// </summary>
        [Description("下载（客户web上下载）")]
        DownLog = 3
    }

    public enum LogStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,
        /// <summary>
        /// 执行中
        /// </summary>
        [Description("执行中")]
        Executing = 2
    }

}
