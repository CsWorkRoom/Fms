#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ISession
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 16:15:41
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Data;

#region 主体



namespace EasyMan.Common.Data
{
    using System;
    using System.Collections.Generic;


    public interface IDbSession : IDisposable
    {
        void Open();

        void Closed();

        Guid SessionId { get; }
        IDbConnection Connection { get; }

        void BeginTranscation();

        int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute parameterized SQL
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// Number of rows affected
        /// </returns>
        int Execute(DataCommandDefinition command);
        /// <summary>
        /// Execute parameterized SQL that selects a single value
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The first cell selected
        /// </returns>
        object ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute parameterized SQL that selects a single value
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The first cell selected
        /// </returns>
        T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute parameterized SQL that selects a single value
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The first cell selected
        /// </returns>
        object ExecuteScalar(DataCommandDefinition command);
        /// <summary>
        /// Execute parameterized SQL that selects a single value
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The first cell selected
        /// </returns>
        T ExecuteScalar<T>(DataCommandDefinition command);
        /// <summary>
        /// Execute parameterized SQL and return an <see cref="T:System.Data.IDataReader"/>
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Data.IDataReader"/> that can be used to iterate over the results of the SQL query.
        /// </returns>
        /// 
        /// <remarks>
        /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="T:System.Data.DataTable"/>
        ///             or <see cref="T:System.Data.DataSet"/>.
        /// 
        /// </remarks>
        /// 
        /// <example>
        /// 
        /// <code>
        /// <![CDATA[
        ///             DataTable table = new DataTable("MyTable");
        ///             using (var reader = ExecuteReader(cnn, sql, param))
        ///             {
        ///                 table.Load(reader);
        ///             }
        ///             ]]>
        /// </code>
        /// 
        /// </example>
        IDataReader ExecuteReader(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Execute parameterized SQL and return an <see cref="T:System.Data.IDataReader"/>
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Data.IDataReader"/> that can be used to iterate over the results of the SQL query.
        /// </returns>
        /// 
        /// <remarks>
        /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="T:System.Data.DataTable"/>
        ///             or <see cref="T:System.Data.DataSet"/>.
        /// 
        /// </remarks>
        IDataReader ExecuteReader(DataCommandDefinition command);
        /// <summary>
        /// Execute parameterized SQL and return an <see cref="T:System.Data.IDataReader"/>
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Data.IDataReader"/> that can be used to iterate over the results of the SQL query.
        /// </returns>
        /// 
        /// <remarks>
        /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="T:System.Data.DataTable"/>
        ///             or <see cref="T:System.Data.DataSet"/>.
        /// 
        /// </remarks>
        IDataReader ExecuteReader(DataCommandDefinition command, CommandBehavior commandBehavior);
        /// <summary>
        /// Return a list of dynamic objects, reader is closed after the call
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;
        /// </remarks>
        IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new [space] get new object
        /// </remarks>
        /// 
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
        ///             created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// 
        /// </returns>
        IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query, returning the data typed as per the Type suggested
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
        ///             created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// 
        /// </returns>
        IEnumerable<object> Query(Type type, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Executes a query, returning the data typed as per T
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs completion gets annoying. Eg type new [space] get new object
        /// </remarks>
        /// 
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
        ///             created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// 
        /// </returns>
        IEnumerable<T> Query<T>(DataCommandDefinition command);
    }
}
#endregion
