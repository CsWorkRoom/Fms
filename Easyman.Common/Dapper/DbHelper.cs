#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 实现数据库的操作
* 文件名：DbHelper.cs
* 作者：cs 时间：2017/7/6 23:40:01
* 邮箱：419746405@qq.com
* ========================================================================
*/
#endregion

using EasyMan.Common.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common
{
    public class DbHelper
    {
        /// <summary>
        /// 执行一个sql语句得到一个table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataTable ExecuteGetTable(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var _dbSession = DatabaseSession.OpenSession();
            DataTable table = new DataTable();
            try
            {
                var dr = _dbSession.ExecuteReader(sql,param,transaction,commandTimeout, commandType);
                table.Load(dr);
                _dbSession.Closed();
            }
            catch (Exception ex)
            {
                _dbSession.Closed();
                throw new Exception("sql执行失败：" + ex.Message);
            }
            finally
            {
                _dbSession.Closed();
            }
            return table;
        }

        /// <summary>
        /// 执行sql语句，返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int Execute(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            int result = 0;//默认为0
            var _dbSession = DatabaseSession.OpenSession();
            DataTable table = new DataTable();
            try
            {
                result= _dbSession.Execute(sql, param, transaction, commandTimeout, commandType);
                _dbSession.Closed();
            }
            catch (Exception ex)
            {
                _dbSession.Closed();
                throw new Exception("sql执行失败：" + ex.Message);
            }
            finally
            {
                _dbSession.Closed();
            }
            return result;
        }

        /// <summary>
        /// 执行sql，返回obj
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, object param = null, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, System.Data.CommandType? commandType = null)
        {
            object obj = 0;//默认为0
            var _dbSession = DatabaseSession.OpenSession();
            DataTable table = new DataTable();
            try
            {
                obj = _dbSession.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
                _dbSession.Closed();
            }
            catch (Exception ex)
            {
                _dbSession.Closed();
                throw new Exception("sql执行失败：" + ex.Message);
            }
            finally
            {
                _dbSession.Closed();
            }
            return obj;
        }
        /// <summary>
        /// 获得DataReader
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IDataReader ExecuteDataReader(string sql)
        {
            IDataReader dr = null;
            var _session = DatabaseSession.OpenSession();

            if (_session != null)
            {
                try
                {
                    dr = _session.ExecuteReader(sql);
                    return dr;
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    _session.Dispose();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
            }
            return dr;
        }


        #region wf的数据导入功能
        /// <summary>
        ///执行sql,插入数据dataTable,返回bool
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectStr"></param>
        /// <param name="sql"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static bool ExecuteScalar(string dbType, string connectStr, string sql, DataTable dataTable)
        {
            var result = false;
            var dsNew = new DataTable();
            switch (dbType)
            {
                default://默认oracle数据库
                    using (var conn = new OracleConnection(connectStr))
                    {
                        try
                        {
                            var cmd = new OracleCommand(sql, conn);
                            var adapter = new OracleDataAdapter(cmd);
                            var cb = new OracleCommandBuilder(adapter);
                            var count = adapter.Fill(dsNew);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                DataRow dr = dsNew.NewRow();
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName];
                                }
                                dsNew.Rows.Add(dr);
                            }
                            count = adapter.Update(dsNew);
                            adapter.UpdateBatchSize = 200;
                            result = true;
                        }
                        catch (Exception e)
                        {
                            e.Message.ErrorMsg();
                        }
                    }
                    break;
                case "db2":
                    //using (var conn = new DB2Connection(connectionString))
                    //{
                    //    try
                    //    {
                    //        var cmd = new DB2Command(sqlScript, conn);
                    //        var adapter = new DB2DataAdapter(cmd);
                    //        var cb = new DB2CommandBuilder(adapter);
                    //        var count = adapter.Fill(dsNew);
                    //        for (int i = 0; i < dataTable.Rows.Count; i++)
                    //        {
                    //            DataRow dr = dsNew.NewRow();
                    //            foreach (DataColumn col in dataTable.Columns)
                    //            {
                    //                dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName];
                    //            }
                    //            dsNew.Rows.Add(dr);
                    //        }
                    //        count = adapter.Update(dsNew);
                    //        adapter.UpdateBatchSize = 200;
                    //        result = true;
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        e.Message.ErrorMsg();
                    //    }
                    //}
                    break;
                case "mysql":
                    //using (var conn = new MySqlConnection(connectionString))
                    //{
                    //    try
                    //    {
                    //        var cmd = new MySqlCommand(sqlScript, conn);
                    //        var adapter = new MySqlDataAdapter(cmd);
                    //        var cb = new MySqlCommandBuilder(adapter);
                    //        var count = adapter.Fill(dsNew);
                    //        for (int i = 0; i < dataTable.Rows.Count; i++)
                    //        {
                    //            DataRow dr = dsNew.NewRow();
                    //            foreach (DataColumn col in dataTable.Columns)
                    //            {
                    //                dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName];
                    //            }
                    //            dsNew.Rows.Add(dr);
                    //        }
                    //        count = adapter.Update(dsNew);
                    //        adapter.UpdateBatchSize = 200;
                    //        result = true;
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        e.Message.ErrorMsg();
                    //    }
                    //}
                    break;
                case "sqlserver":
                    using (var conn = new SqlConnection(connectStr))
                    {
                        try
                        {
                            var cmd = new SqlCommand(sql, conn);
                            var adapter = new SqlDataAdapter(cmd);
                            var cb = new SqlCommandBuilder(adapter);
                            var count = adapter.Fill(dsNew);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                DataRow dr = dsNew.NewRow();
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName];
                                }
                                dsNew.Rows.Add(dr);
                            }
                            count = adapter.Update(dsNew);
                            adapter.UpdateBatchSize = 200;
                            result = true;
                        }
                        catch (Exception e)
                        {
                            e.Message.ErrorMsg();
                        }
                    }
                    break;
            }
            return result;
        }
        #endregion

        //扩展其他数据库操作方法

    }
}
