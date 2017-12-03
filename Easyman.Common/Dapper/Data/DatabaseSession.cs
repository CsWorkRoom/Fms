#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：DatabaseSession
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 10:56:01
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Data.SqlClient;

#region 主体


namespace EasyMan.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class DatabaseSession
    {
        private const string DefaultKey = "Default";
        private static readonly IDictionary<string, ISessionFactory> SessionFactorys = new Dictionary<string, ISessionFactory>();

        [ThreadStatic]
        // ReSharper disable once ThreadStaticFieldHasInitializer
        private static readonly IDictionary<string, IDbConnection> Sessions = new Dictionary<string, IDbConnection>();

        public static void Register(string key, ISessionFactory sessionFactory)
        {
            SessionFactorys[key] = sessionFactory;
        }

        public static void RegisterDefault(ISessionFactory sessionFactory)
        {
            SessionFactorys[DefaultKey] = sessionFactory;
        }

        public static IDbSession OpenSession(string key = null)
        {
            var session = GetSessionFactory(key).Create();
            session.Open();
            return session;
        }

        private static ISessionFactory GetSessionFactory(string key = null)
        {
            key = key ?? DefaultKey;

            return SessionFactorys[key];
        }
    }
}
#endregion
