//using IBM.Data.DB2;
//using MySql.Data.MySqlClient;
using Abp.Domain.Uow;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Easyman.Common
{
    public class DatabaseHelper
    {
        /// <summary>
        /// oracle数据库插入
        /// </summary>
        /// <param name="connectionString">执行库地址</param>
        /// <param name="dataTable">数据</param>
        /// <param name="sqlScript">与数据对应的表的查询语句</param>
        /// <returns>执行情况</returns>
        [UnitOfWork]
        public static bool DataTableToOracleDatabase(string connectionString,
            DataTable dataTable, string sqlScript)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    var cmd = new OracleCommand(sqlScript, conn);
                    var adapter = new OracleDataAdapter(cmd);
                    var cb = new OracleCommandBuilder(adapter);
                    var dsNew = new DataTable();
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
                    return true;
                }
                catch (Exception e)
                {
                    e.Message.ErrorMsg();
                    return false;
                }
            }
        }
        /// <summary>
        /// sqlserver数据库插入
        /// </summary>
        /// <param name="connectionString">执行库地址</param>
        /// <param name="dataTable">数据</param>
        /// <param name="sqlScript">与数据对应的表的查询语句</param>
        /// <returns>执行情况</returns>
        [UnitOfWork]
        public static bool DataTableToSqlServerDatabase(string connectionString,
         DataTable dataTable, string sqlScript)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    var cmd = new SqlCommand(sqlScript, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var cb = new SqlCommandBuilder(adapter);
                    var dsNew = new DataTable();
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
                    return true;
                }
                catch (Exception e)
                {
                    e.Message.ErrorMsg();
                    return false;
                }
            }
        }
        /// <summary>
        /// DB2数据库插入
        /// </summary>
        /// <param name="connectionString">执行库地址</param>
        /// <param name="dataTable">数据</param>
        /// <param name="sqlScript">与数据对应的表的查询语句</param>
        /// <returns>执行情况</returns>
        [UnitOfWork]
        public static bool DataTableToDB2Database(string connectionString,
         DataTable dataTable, string sqlScript)
        {
            //using (var conn = new DB2Connection(connectionString))
            //{
            //    try
            //    {
            //        var cmd = new DB2Command(sqlScript, conn);
            //        var adapter = new DB2DataAdapter(cmd);
            //        var cb = new DB2CommandBuilder(adapter);
            //        var dsNew = new DataTable();
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
            //        return true;
            //    }
            //    catch (Exception e)
            //    {
            //        e.Message.ErrorMsg();
            //        return false;
            //    }
            //}
            return false;
        }
        /// <summary>
        /// MySql数据库插入
        /// </summary>
        /// <param name="connectionString">执行库地址</param>
        /// <param name="dataTable">数据</param>
        /// <param name="sqlScript">与数据对应的表的查询语句</param>
        /// <returns>执行情况</returns>
        [UnitOfWork]
        public static bool DataTableToMySqlDatabase(string connectionString,
         DataTable dataTable, string sqlScript)
        {
            //using (var conn = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        var cmd = new MySqlCommand(sqlScript, conn);
            //        var adapter = new MySqlDataAdapter(cmd);
            //        var cb = new MySqlCommandBuilder(adapter);
            //        var dsNew = new DataTable();
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
            //        return true;
            //    }
            //    catch (Exception e)
            //    {
            //        e.Message.ErrorMsg();
            //        return false;
            //    }
            //}
            return false;
        }

        #region 数据库表是否存在
        public static string GetIsDataBaseTableSql(string strDataBaseTyle,string strTableName,string strUser=null)
        {
            string strSql = "";
            strTableName = strTableName.ToUpper();
            switch (strDataBaseTyle.ToLower())
            {
                case "oracle":
                    strSql= "SELECT COUNT(*) FROM ALL_TABLES WHERE UPPER(TABLE_NAME) = '" + strTableName + "'";
                    if (strUser != null)
                        strSql += " AND OWNER = '" + strUser + "'";
                    break;
                case "db2":
                    strSql= "SELECT COUNT(*) FROM SYSCAT.TABLES WHERE UPPER(TABNAME) = '" + strTableName + "'";
                    if (strUser != null)
                        strSql += " AND TABSCHEMA = '" + strUser + "' WITH UR";
                    break;
                case "sqlserver":
                    strSql = "select COUNT(*) from sysobjects where UPPER(name)='" + strTableName + "' and xtype='U'";
                    break;
                case "mysql":
                    strSql = "SELECT table_name FROM information_schema.TABLES WHERE UPPER(table_name) ='" + strTableName + "'";
                    break;
                default:
                    strSql = "SELECT COUNT(*) FROM ALL_TABLES WHERE UPPER(TABLE_NAME) = '" + strTableName + "'";
                    if (strUser != null)
                        strSql += " AND OWNER = '" + strUser + "'";
                    break;
            }
            return strSql;
        }
        #endregion
    }
}
