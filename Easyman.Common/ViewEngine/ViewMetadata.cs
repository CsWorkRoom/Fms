#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ViewMetadata
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/16 16:38:29
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

#region 主体

namespace Easyman.Common.ViewEngine
{
    using System;

    [Serializable]
    public class ViewMetaData
    {
        public string Name { get; set; }
        public string AssemblyFullName { get; set; }
    }
}
#endregion
