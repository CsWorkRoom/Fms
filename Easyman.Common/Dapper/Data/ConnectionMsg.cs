using Easyman.Common.Reconsitution;
using EasyMan.Common.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common.Data
{
    public class ConnectionMsg
    {
        public static ConnectionMsg connection;
        private const string DefaultKey = "Default";//默认连接串名
        private static object _lock = new object();

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; }
        /// <summary>
        /// 数据库种类
        /// </summary>
        public DatabaseType DbType { get; set; }


        #region 自定义私有构造函数
        private ConnectionMsg()
        {
            var connStringList = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings connStr in connStringList)
            {
                if (!string.IsNullOrWhiteSpace(connStr.ConnectionString) && connStr.Name == DefaultKey)
                {
                    var type = connStr.ProviderName;
                    var name = connStr.Name;
                    ConnectionStr = Utility.Decode(connStr.ConnectionString) ?? connStr.ConnectionString;//连接串

                    switch (type)
                    {
                        case "Oracle.ManagedDataAccess.Client"://Oracle
                            DbType = DatabaseType.Oracle;
                            break;
                        case "db2"://待补充，需确认驱动格式
                            DbType = DatabaseType.Db2;
                            break;
                        case "mysql"://待补充，需确认格式
                            DbType = DatabaseType.MySql;
                            break;
                        case "System.Data.SqlClient"://sqlserver
                            DbType = DatabaseType.SqlServer;
                            break;
                        default:
                            DbType = DatabaseType.Oracle;
                            break;
                    };
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ConnectionMsg GetCurConnection()
        {
            if (connection == null)
            {
                lock (_lock)
                {
                    if (connection == null)
                    {
                        connection = new ConnectionMsg();
                    }
                }
            }
            return connection;
        }
    }
}
