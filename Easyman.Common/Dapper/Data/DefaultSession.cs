#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：DefaultSession
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 16:22:23
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using IBM.Data.DB2;

#region 主体



namespace EasyMan.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class DefaultSession : IDbSession
    {
        private IDbTransaction _transaction;
        private bool _wasDisposed;
        public DefaultSession(string connString, DatabaseType dbType)
        {
            SessionId = Guid.NewGuid();
            Connection = CreateConnection(connString, dbType);
        }
        public Guid SessionId { get; private set; }

        public IDbConnection Connection { get; private set; }

        public void Open()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public void Closed()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
            _wasDisposed = true;
        }

        private IDbConnection CreateConnection(string connString, DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.Oracle:
                    return new OracleConnection(connString);
                    break;
                case DatabaseType.SqlServer:
                    return new SqlConnection(connString);
                    break;
                case DatabaseType.MySql:
                    throw new Exception("未实现该数据库");
                    // return new MySqlConnection(_connString);
                    break;
                case DatabaseType.Db2:
                    //return new OleDbConnection(connString);
                    return new DB2Connection(connString);
                    break;
                default:
                    return new OracleConnection(connString);
                    break;
            }
        }

        private CommandFlags ToCommandFlags(DataCommandFlags inputFlags)
        {
            return (CommandFlags)Enum.Parse(typeof(CommandFlags), ((int)inputFlags).ToString(CultureInfo.InvariantCulture));
        }

        #region opation

        public void BeginTranscation()
        {
            _transaction = Connection.BeginTransaction();
        }

        public int Execute(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public int Execute(DataCommandDefinition command)
        {
            return Connection.Execute(
                new CommandDefinition(command.CommandText,
                    command.Parameters,
                    command.Transaction,
                    command.CommandTimeout,
                    command.CommandType,
                    ToCommandFlags(command.Flags)));
        }

        public object ExecuteScalar(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public object ExecuteScalar(DataCommandDefinition command)
        {
            return Connection.ExecuteScalar(
               new CommandDefinition(command.CommandText,
                   command.Parameters,
                   command.Transaction,
                   command.CommandTimeout,
                   command.CommandType,
                   ToCommandFlags(command.Flags)));
        }

        public T ExecuteScalar<T>(DataCommandDefinition command)
        {
            return Connection.ExecuteScalar<T>(
                new CommandDefinition(command.CommandText,
                    command.Parameters,
                    command.Transaction,
                    command.CommandTimeout,
                    command.CommandType,
                    ToCommandFlags(command.Flags)));
        }

        public System.Data.IDataReader ExecuteReader(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        }

        public System.Data.IDataReader ExecuteReader(DataCommandDefinition command)
        {
            return Connection.ExecuteReader(
               new CommandDefinition(command.CommandText,
                   command.Parameters,
                   command.Transaction,
                   command.CommandTimeout,
                   command.CommandType,
                   ToCommandFlags(command.Flags)));
        }

        public System.Data.IDataReader ExecuteReader(DataCommandDefinition command, System.Data.CommandBehavior commandBehavior)
        {
            return Connection.ExecuteReader(
                new CommandDefinition(command.CommandText,
                    command.Parameters,
                    command.Transaction,
                    command.CommandTimeout,
                    command.CommandType,
                    ToCommandFlags(command.Flags)), commandBehavior);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, System.Data.IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, System.Data.IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<object> Query(Type type, string sql, object param = null, System.Data.IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            return Connection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(DataCommandDefinition command)
        {
            return Connection.Query<T>(
                new CommandDefinition(command.CommandText,
                    command.Parameters,
                    command.Transaction,
                    command.CommandTimeout,
                    command.CommandType,
                    ToCommandFlags(command.Flags)));
        }
        #endregion
    }

}
#endregion
