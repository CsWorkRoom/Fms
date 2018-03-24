using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Em.Project.Common.Helper
{
   public static class OracleHelper
    {


        #region 数据批量插入
        /**  
        * 批量插入数据  
        * @tableName 表名称  
        * @columnRowData 键-值存储的批量数据：键是列名称，值是对应的数据集合  
        * @conStr 连接字符串  
        * @len 每次批处理数据的大小  
        */
        public static int BatchInsert(string tableName, Dictionary<string, object> columnRowData, string conStr, int len)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("必须指定批量插入的表名称", "tableName");
            }

            if (columnRowData == null || columnRowData.Count < 1)
            {
                throw new ArgumentException("必须指定批量插入的字段名称", "columnRowData");
            }

            int iResult = 0;
            string[] dbColumns = columnRowData.Keys.ToArray();
            StringBuilder sbCmdText = new StringBuilder();
            if (columnRowData.Count > 0)
            {
                //准备插入的SQL  
                sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
                sbCmdText.Append(string.Join(",", dbColumns));
                sbCmdText.Append(") VALUES (");
                sbCmdText.Append(":" + string.Join(",:", dbColumns));
                sbCmdText.Append(")");

                using (OracleConnection conn = new OracleConnection(conStr))
                {
                    
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        //绑定批处理的行数  
                        cmd.ArrayBindCount = len;
                        cmd.BindByName = true;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sbCmdText.ToString();
                        cmd.CommandTimeout = 1800;//100分钟  

                        //创建参数  
                        OracleParameter oraParam;
                        List<IDbDataParameter> cacher = new List<IDbDataParameter>();
                        OracleDbType dbType = OracleDbType.Varchar2;
                        foreach (string colName in dbColumns)
                        {
                            dbType = GetOracleDbType(columnRowData[colName]);
                            oraParam = new OracleParameter(colName, dbType);
                            oraParam.Direction = ParameterDirection.Input;
                            oraParam.OracleDbTypeEx = dbType;

                            oraParam.Value = columnRowData[colName];
                            cmd.Parameters.Add(oraParam);
                        }
                        //打开连接  
                        conn.Open();

                        /*执行批处理*/
                        var trans = conn.BeginTransaction();
                        try
                        {
                            cmd.Transaction = trans;
                            iResult = cmd.ExecuteNonQuery();
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            if (conn != null) conn.Close();
                        }

                    }
                }
            }
            return iResult;
        }

        /**  
         * 根据数据类型获取OracleDbType  
         */
        private static OracleDbType GetOracleDbType(object value)
        {
            OracleDbType dataType = OracleDbType.Varchar2;
            if (value is string[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is DateTime[])
            {
                dataType = OracleDbType.TimeStamp;
            }
            else if (value is int[] || value is short[])
            {
                dataType = OracleDbType.Int32;
            }
            else if (value is long[])
            {
                dataType = OracleDbType.Int64;
            }
            else if (value is decimal[] || value is double[] || value is float[])
            {
                dataType = OracleDbType.Decimal;
            }
            else if (value is Guid[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is bool[] || value is Boolean[])
            {
                dataType = OracleDbType.Byte;
            }
            else if (value is byte[])
            {
                dataType = OracleDbType.Blob;
            }
            else if (value is char[])
            {
                dataType = OracleDbType.Char;
            }
            return dataType;
        }
        #endregion


        #region
        /// <summary>
        /// 
        /// </summary>
        public static string ExeProduce(string conStr,string ticks, long folderId, long computerId, long scriptNodeCaseId)
        {
            string outMsg = "";
            using (OracleConnection oc = new OracleConnection(conStr))
            {
                try
                {

                    oc.Open();
                    OracleCommand om = oc.CreateCommand();
                    om.CommandType = CommandType.StoredProcedure;
                    om.CommandText = "FMS_CL.PKG_MONIT_FILE.SAVE_DATA";
                    om.Parameters.Add("CUR_TICKS", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                    om.Parameters["CUR_TICKS"].Value = ticks;
                    om.Parameters.Add("FOLDER_ID", OracleDbType.Decimal).Direction = ParameterDirection.Input;
                    om.Parameters["FOLDER_ID"].Value = folderId;
                    om.Parameters.Add("COMPUTER_ID", OracleDbType.Decimal).Direction = ParameterDirection.Input;
                    om.Parameters["COMPUTER_ID"].Value = computerId;
                    om.Parameters.Add("SCRIPT_NODE_CASE_ID", OracleDbType.Decimal).Direction = ParameterDirection.Input;
                    om.Parameters["SCRIPT_NODE_CASE_ID"].Value = scriptNodeCaseId;
                    om.Parameters.Add("OUT_MSG", OracleDbType.Varchar2).Direction = ParameterDirection.Output;

                    om.ExecuteNonQuery();
                    outMsg=om.Parameters[4].Value.ToString();
                    oc.Close();                   
                }
                catch (Exception ex)
                {                 
                    throw ex;
                }
                finally
                {
                    if (oc != null) oc.Close();
                }
              
            }
            return outMsg;

        }
        #endregion
    }
}
