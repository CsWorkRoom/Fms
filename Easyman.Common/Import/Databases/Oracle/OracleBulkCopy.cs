using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNet.SignalR;
using EasyMan.Dtos;
using EasyMan.Import;

namespace EasyMan.Databases.Oracle
{
    public class OracleBulkCopy : BulkCopyBase
    {
        private readonly Dictionary<string, Func<object, object>> _convertor = new Dictionary<string, Func<object, object>>();
        //public event RowsCopiedEventHandler RowsCopied;

        public OracleBulkCopy(IDbConnection connection)
            : base(connection)
        {

        }

        public OracleBulkCopy(IDbConnection connection, string table)
            : this(connection)
        {
            DestinationTableName = table;
        }

        public override int WriteToServer(DataRow[] dataRow)
        {
            var table = new DataTable();
            dataRow.Each(table.ImportRow);
            return WriteToServer(table);
        }

        public override int WriteToServer(DataTable dataTable)
        {
            return WriteToServer(dataTable.CreateDataReader(), dataTable.Rows.Count);
        }

        public override int WriteToServer(IDataReader dataReader,int tolCount)
        {
            if (string.IsNullOrWhiteSpace(DestinationTableName))
                throw new InvalidOperationException("目标表名称不能为空");

            var schema = SchemaTable ?? GetSchemaTable();
            var buffer = CreateBuffer(schema.Rows.Count);

            var totalRows = 0;
            using (var command = GetInsertCommand(schema))
            {
                var count = 0;

                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    command.Parameters[i].Value = buffer[i];
                }
                while (dataReader.Read())
                {
                    if (count == BatchSize)
                    {
                        command.ArrayBindCount = count;
                        command.ExecuteNonQuery();
                        count = 0;
                        var cashDatas = new CashHubDto();
                        cashDatas.Goal = tolCount;
                        cashDatas.Raised = totalRows;
                        var context = GlobalHost.ConnectionManager.GetHubContext<CashHub>();
                        context.Clients.All.getMessage(cashDatas);
                    }

                    for (int i = 0; i < schema.Rows.Count; i++)
                    {
                        if (dataReader.IsDBNull(i))
                        {
                            buffer[i][count] = DBNull.Value;
                        }
                        else
                        {
                            try
                            {
                                buffer[i][count] = _convertor[command.Parameters[i].ParameterName](dataReader[i]);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidDataException("数据读取错误,行：{0}，列{1}".FormatWith(dataReader.RecordsAffected, i), ex.GetBaseException());
                            }
                        }
                    }
                    ++count;
                    ++totalRows;
                }

                if (count > 0)
                {
                    command.ArrayBindCount = count;
                    command.ExecuteNonQuery();
                }
            }
            return totalRows;
        }

        private object[][] CreateBuffer(int fieldCount)
        {
            var result = new object[fieldCount][];

            for (int i = 0; i < fieldCount; i++)
            {
                result[i] = new object[BatchSize];
            }
            return result;
        }

        private OracleCommand GetInsertCommand(DataTable schema)
        {
            var sql = new StringBuilder();
            var result = (OracleCommand)Connection.CreateCommand();

            foreach (DataRow row in schema.Rows)
            {
                if (sql.Length > 0)
                    sql.Append(",");

                var parameter = new OracleParameter();

                result.Parameters.Add(parameter);
                parameter.ParameterName = row["ColumnName"].ToString();
                sql.AppendFormat(":{0}", parameter.ParameterName);

                var type = row["DataType"] as Type;

                if (type == typeof(string))
                {
                    _convertor.Add(parameter.ParameterName, str => str);
                    parameter.OracleDbType = OracleDbType.Varchar2;
                }
                else if (type == typeof(Int16))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToInt16(str));
                    parameter.OracleDbType = OracleDbType.Int16;
                }
                else if (type == typeof(int))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToInt32(str));
                    parameter.OracleDbType = OracleDbType.Int32;
                }
                else if (type == typeof(long))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToInt64(str));
                    parameter.OracleDbType = OracleDbType.Int64;
                }
                else if (type == typeof(decimal))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToDecimal(str));
                    parameter.OracleDbType = OracleDbType.Decimal;
                }
                else if (type == typeof(double))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToDouble(str));
                    parameter.OracleDbType = OracleDbType.Double;
                }
                else if (type == typeof(DateTime))
                {
                    _convertor.Add(parameter.ParameterName, str => Convert.ToDateTime(str));
                    parameter.OracleDbType = OracleDbType.Date;
                }
            }

            sql.Append(")");
            sql.Insert(0, "INSERT INTO " + DestinationTableName + " VALUES (");

            result.CommandText = sql.ToString();
            result.BindByName = true;

            return result;
        }
    }
}
