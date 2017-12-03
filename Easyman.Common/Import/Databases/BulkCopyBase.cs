using System;
using System.Data;

namespace EasyMan.Databases
{
    public class RowsCopiedEventArgs : EventArgs
    {
        public bool Abort { get; set; }
        public int RowsCopied { get; private set; }

        public RowsCopiedEventArgs(int rowsCopied)
        {
            RowsCopied = rowsCopied;
        }
    }

    /// <summary>
    /// 批量插入
    /// </summary>
    public abstract class BulkCopyBase : MarshalByRefObject, IDisposable
    {
        public delegate void RowsCopiedEventHandler(object sender, RowsCopiedEventArgs e);

        protected BulkCopyBase(IDbConnection connection)
        {
            Connection = connection;
            BatchSize = 1000;
            Timeout = 60 * 2;
        }

        /// <summary>
        /// 超时时间，单位：秒
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 目标表名
        /// </summary>
        public string DestinationTableName { get; set; }

        public int NotifyAfter { get; set; }

        /// <summary>
        /// 批量数量
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        public DataTable SchemaTable { get; set; }

        protected IDbConnection Connection { get; set; }

        protected DataTable GetSchemaTable()
        {
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + DestinationTableName + " WHERE 1 = 2";
                return command.ExecuteReader().GetSchemaTable();
            }
        }

        public abstract int WriteToServer(DataRow[] dataRow);

        public abstract int WriteToServer(DataTable dataTable);

        public abstract int WriteToServer(IDataReader dataReader,int count);

        public void Dispose()
        {

        }
    }
}
