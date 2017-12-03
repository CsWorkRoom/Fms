using System;
using System.Data;
using System.IO;
using Abp.UI;
using EasyMan.Databases.Oracle;
using EasyMan.Common.Data;

namespace EasyMan.Import
{
    public class ImportHelper
    {
        /// <summary>
        /// 导入数据到目标表
        /// </summary>
        /// <param name="tableName">目标表名</param>
        /// <param name="filePath">数据文件流路径，支持xls、xlsx、csv文件</param>
        /// <param name="columnCont">字段数量</param>
        /// <param name="databaseSessionKey">数据库连接Key</param>
        /// <returns>成功行数</returns>
        public static int Import(string tableName, string filePath, int columnCont = 0, string databaseSessionKey = null)
        {
            using (var session = DatabaseSession.OpenSession(databaseSessionKey))
            {
                return Import(session, tableName, filePath, columnCont);
            }
        }

        /// <summary>
        ///  导入数据到目标表
        /// </summary>
        /// <param name="session">数据库连接Session</param>
        /// <param name="tableName">表名</param>
        /// <param name="filePath">数据文件流路径，支持xls、xlsx、csv文件</param>
        /// <param name="columnCont">字段数量</param>
        /// <returns>成功行数</returns>
        public static int Import(IDbSession session, string tableName, string filePath, int columnCont = 0)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("tableName");
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath");

            int rows;
            using (var tran = session.Connection.BeginTransaction())
            {
                var oracleBulkCopy = new OracleBulkCopy(session.Connection) { Timeout = 30 * 60, DestinationTableName = tableName };
                try
                {
                    var extension = Path.GetExtension(filePath).ToLower();
                    DataTable dt;
                    if (extension.StartsWith(".xls") || extension.StartsWith(".xlsx"))
                    {
                        dt = ExcelHelper.ExportExcelDataTable(filePath);
                    }
                    else if (extension == ".csv" || extension == ".txt")
                    {
                        dt = ExcelHelper.ExportCsvDataReader(filePath);
                    }
                    else
                        throw new Exception("暂不支持：{0} 文件".FormatWith(extension));

                    if (columnCont > 0 && dt.Columns.Count != columnCont)
                        throw new Exception("字段数量不一致（{0} / {1}）".FormatWith(columnCont, dt.Columns.Count));
                    
                    rows = oracleBulkCopy.WriteToServer(dt);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new UserFriendlyException("导入失败: " + ex.GetTrueMessage(), ex.GetBaseException());
                }
                finally { oracleBulkCopy.Dispose(); }
            }
            return rows;
        }
    }
}
