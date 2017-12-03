#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：SessionFactory
* 版本：4.0.30319.42000
* 作者：zhl 时间：2016/2/17 11:34:38
* 邮箱：
* ========================================================================
*/
#endregion

#region 主体



namespace EasyMan.Common.Data
{
    using System;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class SessionFactory : ISessionFactory
    {
        private readonly string _connString;
        private readonly DatabaseType _dbType;

        public SessionFactory(string connString, DatabaseType dbType)
        {
            _connString = connString;
            _dbType = dbType;
        }

        public IDbSession Create()
        {
            return new DefaultSession(_connString, _dbType);
        }
    }
}
#endregion
