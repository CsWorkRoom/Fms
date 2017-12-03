using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common
{
    public static class OperateSection
    {
        #region 从配置文件中获取节点信息
        /// <summary>
        /// 获得PwdRule节点信息
        /// </summary>
        /// <returns></returns>
        public static PwdRuleSection GetPwdRuleSection()
        {
            return (PwdRuleSection)ConfigurationManager.GetSection("PwdRule");
        }
        /// <summary>
        /// 获得PwdComplex节点信息
        /// </summary>
        /// <returns></returns>
        public static PwdComplexSection GetPwdComplexSection()
        {
            return (PwdComplexSection)ConfigurationManager.GetSection("PwdComplex");
        }
        /// <summary>
        /// 获得PwdComplex节点的子节点add项的List集合:List<MyKeyValueSetting>
        /// </summary>
        /// <returns></returns>
        public static List<MyKeyValueSetting> GetPwdComplexSetList()
        {
            PwdComplexSection pwdComplex = (PwdComplexSection)ConfigurationManager.GetSection("PwdComplex");
            if (pwdComplex != null && pwdComplex.KeyValues != null && pwdComplex.KeyValues.Count > 0)
            {
                return pwdComplex.KeyValues.Cast<MyKeyValueSetting>().ToList();
            }
            else
                return null;
        }
        #endregion

        #region 修改配置文件节点信息(暂未实现)
        //可参考地址：http://www.cnblogs.com/fish-li/archive/2011/12/18/2292037.html#_labelStart
        #endregion
    }
}
