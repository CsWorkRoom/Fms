using Abp.AutoMapper;
using Easyman.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    /// <summary>
    /// 脚本流实例日志
    /// </summary>
    [AutoMap(typeof(ScriptCaseLog))]
    public class ScriptCaseLogModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        public long? ScriptCaseId { get; set; }
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime? LogTime { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        public short? LogLevel { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>

        public string LogMsg { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>

        public string SqlMsg { get; set; }
        /// <summary>
        /// 脚本流实例名
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 脚本实例日志，查询所用
    /// </summary>
    [AutoMap(typeof(Easyman.Domain.ScriptNodeCase))]
    public class ScriptNodeCaseModel{

        public long Id { get; set; }
        /// <summary>
        /// 编译后的脚本内容
        /// </summary>
        public  string CompileContent { get; set; }
    }


}
