using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common
{
    /// <summary>
    /// 用户级密码基础配置类
    /// </summary>
    public class PwdRuleSection: ConfigurationSection
    {
        /// <summary>
        /// 是否启用试错
        /// </summary>
        [ConfigurationProperty("isTrialError", IsRequired = true)]
        public bool IsTrialError
        {
            get { return Convert.ToBoolean(this["isTrialError"].ToString().Trim()); }
            set { this["isTrialError"] = value; }
        }
        /// <summary>
        /// 可试错次数
        /// </summary>
        [ConfigurationProperty("trialErrorCount", IsRequired = true)]
        public string TrialErrorCount
        {
            get { return this["trialErrorCount"].ToString(); }
            set { this["trialErrorCount"] = value; }
        }

        /// <summary>
        /// 是否启用复杂类型
        /// </summary>
        [ConfigurationProperty("isValidatecComplex", IsRequired = true)]
        public bool IsValidatecComplex
        {
            get { return Convert.ToBoolean(this["isValidatecComplex"].ToString().Trim()); }
            set { this["isValidatecComplex"] = value; }
        }

        /// <summary>
        /// 是否启用周期性修改密码
        /// </summary>
        [ConfigurationProperty("isCycle", IsRequired = true)]
        public bool IsCycle
        {
            get { return Convert.ToBoolean(this["isCycle"].ToString().Trim()); }
            set { this["isCycle"] = value; }
        }
        /// <summary>
        /// 周期时间（单位：天）
        /// </summary>
        [ConfigurationProperty("cycleTime", IsRequired = true)]
        public string CycleTime
        {
            get { return this["cycleTime"].ToString(); }
            set { this["cycleTime"] = value; }
        }

        /// <summary>
        /// 是否启用随机密码
        /// </summary>
        [ConfigurationProperty("isRandomPwd", IsRequired = true)]
        public bool IsRandomPwd
        {
            get { return Convert.ToBoolean(this["isRandomPwd"].ToString().Trim()); }
            set { this["isRandomPwd"] = value; }
        }
        /// <summary>
        /// 默认密码
        /// </summary>
        [ConfigurationProperty("defualtPwd", IsRequired = true)]
        public string DefualtPwd
        {
            get { return this["defualtPwd"].ToString(); }
            set { this["defualtPwd"] = value; }
        }
    }
}
