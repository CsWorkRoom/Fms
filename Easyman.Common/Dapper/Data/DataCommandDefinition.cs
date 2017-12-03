#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：DataCommandDefinition
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/2/17 16:34:15
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System;
using System.Data;
using System.Threading;

#region 主体



namespace EasyMan.Common.Data
{
    public class DataCommandDefinition
    {
        /// <summary>
        /// Initialize the command definition
        /// 
        /// </summary>
        public DataCommandDefinition(string commandText, object parameters = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null, DataCommandFlags flags = DataCommandFlags.Buffered,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            CommandText = commandText;
            Parameters = parameters;
            Transaction = transaction;
            CommandTimeout = commandTimeout;
            CommandType = commandType;
            Flags = flags;
            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// The command (sql or a stored-procedure name) to execute
        /// 
        /// </summary>
        public string CommandText { get; private set; }
        /// <summary>
        /// The parameters associated with the command
        /// 
        /// </summary>
        public object Parameters { get; private set; }
        /// <summary>
        /// The active transaction for the command
        /// 
        /// </summary>
        public IDbTransaction Transaction { get; private set; }
        /// <summary>
        /// The effective timeout for the command
        /// 
        /// </summary>
        public int? CommandTimeout { get; private set; }
        /// <summary>
        /// The type of command that the command-text represents
        /// 
        /// </summary>
        public CommandType? CommandType { get; private set; }
        /// <summary>
        /// Should data be buffered before returning?
        /// 
        /// </summary>
        public bool Buffered
        {
            get { return Flags == DataCommandFlags.Buffered; }
        }
        /// <summary>
        /// Additional state flags against this command
        /// 
        /// </summary>
        public DataCommandFlags Flags { get; private set; }
        /// <summary>
        /// Can async queries be pipelined?
        /// 
        /// </summary>
        public bool Pipelined { get; private set; }
        /// <summary>
        /// For asynchronous operations, the cancellation-token
        /// 
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }
    }

    [Flags]
    public enum DataCommandFlags
    {
        None = 0,
        Buffered = 1,
        Pipelined = 2,
        NoCache = 4,
    }
}


#endregion
