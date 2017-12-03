#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ISessionFactory
* 版本：4.0.30319.42000
* 作者：zhl 时间：2016/2/17 10:57:10
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

#region 主体
namespace EasyMan.Common.Data
{
    using System.Data;
    public interface ISessionFactory
    {
        IDbSession Create();
    }
}
#endregion
