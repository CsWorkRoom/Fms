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
    /// 
    /// </summary>
    [AutoMap(typeof(ScriptNodeCaseLog))]
    public class ScriptNodeCaseLogModel
    {
        public  long Id { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
       
        public  long? ScriptNodeCaseId { get; set; }
        /// <summary>
        /// 日志时间
        /// </summary>
        
        public  DateTime? LogTime { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
       
        public  short? LogLevel { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
        
        public  string LogMsg { get; set; }
        /// <summary>
        /// 脚本类型名
        /// </summary>
       
        public  string SqlMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
