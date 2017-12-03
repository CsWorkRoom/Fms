using Easyman.Common.Reconsitution;
using EasyMan.Common.Data;
using System.Configuration;

namespace Easyman.Common.Data
{
    public static class DatabaseRegister
    {
        private const string DefaultKey = "Default";
        public static void InitDataBase()
        {
            var connStringList = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings connStr in connStringList)
            {
                if (string.IsNullOrWhiteSpace(connStr.ConnectionString))
                    continue;
                var type = connStr.ProviderName;
                var name = connStr.Name;
                var conn = Utility.Decode(connStr.ConnectionString) ?? connStr.ConnectionString;

                ISessionFactory factory =null;
                //1、可以只对默认库(DefaultKey)做连接字符串的注册。
                //2、EF的连接字符串的写法同样适用于ado的连接串
                //3、当前为了支持注册多个库的连接字符串，故取消了只对默认库DefaultKey的注册。看以下代码
                //if (name == DefaultKey)//只对默认库做注册(取消)
                //{
                switch (type)
                    {
                        case "Oracle.ManagedDataAccess.Client"://Oracle
                            factory = new SessionFactory(conn, DatabaseType.Oracle);
                            break;
                        case "db2"://待补充，需确认驱动格式
                            factory = new SessionFactory(conn, DatabaseType.Db2);
                            break;
                        case "mysql"://待补充，需确认格式
                            factory = new SessionFactory(conn, DatabaseType.MySql);
                            break;
                        case "System.Data.SqlClient"://sqlserver
                            factory = new SessionFactory(conn, DatabaseType.SqlServer);
                            break;
                        default:
                            factory = new SessionFactory(conn, DatabaseType.Oracle);
                            break;
                    };
                //}
                DatabaseSession.Register(name, factory);

            }
        }
    }
}
