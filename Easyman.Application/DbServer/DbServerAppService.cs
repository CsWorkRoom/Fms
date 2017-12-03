using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Web.Mvc;
using System.Data;
using EasyMan.Common.Data;
using Abp.Application.Services.Dto;
using System.Data.Common;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Easyman.Common.Helper;

namespace Easyman.Service
{
    /// <summary>
    /// 数据库管理 + 基于数据库的sqlhelper
    /// </summary>
    public class DbServerAppService : EasymanAppServiceBase, IDbServerAppService
    {

        #region 初始化

        private readonly IRepository<DbServer, long> _dbServerRepository;
        /// <summary>
        /// 构造函数注入DbServer的仓储
        /// </summary>
        /// <param name="dbServerRepository"></param>
        public DbServerAppService(IRepository<DbServer, long> dbServerRepository)
        {
            _dbServerRepository = dbServerRepository;
        }
        #endregion


        #region 服务接口

        #region 对dbserver的基础操作
        /// <summary>
        /// 获取数据库集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DbServerSearchOutput GetDbServerSearch(DbServerSearchInput input)
        {
            var rowCount = 0;
            var servers = _dbServerRepository.GetAll().SearchByInputDto(input, out rowCount);

            #region 写法一： AutoMapper自动为DTO的DbTagName字段赋值（原理：获取DbTag属性实例，并将其Name获取赋值给DbTagName）

            var outPut = new DbServerSearchOutput()
            {
                Datas = servers.ToList().Select(s => s.MapTo<DbServerOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            #endregion

            #region 写法一（补充）：循环为DbTagName赋值（由于AutoMapper已自动赋值，以下语句在满足规律情况下可以不写） 

            //outPut.Datas.ToList().ForEach(p => p.DbTagName = p.DbTag.Name);

            #endregion

            #region 写法二：在LINQ的Select中写代码段赋值

            //var outPut = new DbServerSearchOutput()
            //{
            //    Datas = servers.ToList().Select(s =>
            //    {
            //        var temp = s.MapTo<DbServerOutput>();

            //        if (s.DbTag != null)
            //            temp.DbTagName = s.DbTag.Name;
            //        else
            //            temp.DbTagName = "";
            //        return temp;
            //    }
            //    ).ToList(),
            //    Page = new Pager(input.Page) { TotalCount = rowCount }
            //};

            #endregion



            return outPut;
        }

        /// <summary>
        /// 根据ID获取某个数据库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DbServerOutput GetDbServer(long id)
        {
            try
            {
                var resDbServer = new DbServerOutput();
                var db = _dbServerRepository.Get(id);
                if (db != null)
                {
                    resDbServer = db.MapTo<DbServerOutput>();
                    resDbServer.DbTagName = db.DbTag == null ? "" : db.DbTag.Name;
                    resDbServer.DbTypeName = db.DbType == null ? "" : db.DbType.Name;
                }
                return resDbServer;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }
         
        /// <summary>
        /// 更新和新增数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DbServerOutput InsertOrUpdateDbServer(DbServerInput input)
        {

            if (_dbServerRepository.GetAll().Any(p => p.Id != input.Id && p.ByName == input.ByName))
            {
                throw new System.Exception("数据库别名重复");
            }
            //var dbServer = _dbServerRepository.Get(input.Id) ?? new DbServer();

            //var dbServer = AutoMapper.Mapper.Map<DbServer>(input);

            var dbServer = AutoMapper.Mapper.Map<DbServerInput, DbServer>(input);

            var server = _dbServerRepository.InsertOrUpdate(dbServer);

            if (server != null)
            {
                return AutoMapper.Mapper.Map<DbServerOutput>(server);
            }
            else
            {
                throw new UserFriendlyException("更新失败！");
            }
        }

        /// <summary>
        /// 删除一条数据库
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDbServer(EntityDto<long> input)
        {
            try
            {
                _dbServerRepository.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作错误，对象或已被删除！");
            }
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetDbServerTreeJson()
        {
            var builder = new StringBuilder();

            var servers = _dbServerRepository.GetAllList();

            var dbServers = servers.Select(s => new
            {
                id = s.Id,
                name = s.ByName,
                open = false,
                iconSkin = "menu"
            }).ToList();

            #region 暂时不启用默认中心库
            //var sysdb = new {
            //    id = Convert.ToInt64(0),
            //    name = "默认中心库",
            //    open = false,
            //    iconSkin = "menu"
            //};
            //dbServers.Add(sysdb);
            #endregion

            return dbServers;
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _dbServerRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.ByName, Value = x.Id.ToString() })
                .ToList();
            return dataList;
        }
        #endregion

        #region 基于dbserver的数据库操作helper

        #region 创建数据库链接
        /// <summary>
        /// 根据数据库ID获取链接
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public IDbSession GetSession(long dbId)
        {
            IDbSession _session = null;
            try
            {
                var dbserver = GetDbServer(dbId);
                if (dbserver != null)
                {
                    var conStr = dbserver == null ? null : GetConnectStr(dbserver);
                    DatabaseType dbType;

                    switch (dbserver.DbTypeName.ToUpper())
                    {
                        case "DB2":
                            dbType = DatabaseType.Db2;
                            break;
                        case "ORACLE":
                            dbType = DatabaseType.Oracle;
                            break;
                        case "MYSQL":
                            dbType = DatabaseType.MySql;
                            break;
                        case "SQLSERVER":
                            dbType = DatabaseType.SqlServer;
                            break;
                        default:
                            dbType = DatabaseType.Oracle;
                            break;
                    }
                    _session = new DefaultSession(conStr, dbType);
                }
                return _session;
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据库连接失败：" + ex.Message);
            }
        }
        /// <summary>
        ///  根据数据库信息获取链接
        /// </summary>
        /// <param name="dbserver"></param>
        /// <returns></returns>
        public IDbSession GetSession(DbServerOutput dbserver)
        {
            IDbSession _session = null;
            try
            {
                if (dbserver != null)
                {
                    var conStr = dbserver == null ? null : GetConnectStr(dbserver);
                    DatabaseType dbType;

                    switch (dbserver.DbTypeName.ToUpper())
                    {
                        case "DB2":
                            dbType = DatabaseType.Db2;
                            break;
                        case "ORACLE":
                            dbType = DatabaseType.Oracle;
                            break;
                        case "MYSQL":
                            dbType = DatabaseType.MySql;
                            break;
                        case "SQLSERVER":
                            dbType = DatabaseType.SqlServer;
                            break;
                        default:
                            dbType = DatabaseType.Oracle;
                            break;
                    }
                    _session = new DefaultSession(conStr, dbType);
                }
                return _session;
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据库连接失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbserver"></param>
        /// <returns></returns>
        public string GetConnectStr(DbServerOutput dbserver)
        {
            string aesPwd = dbserver.Password;
            try
            {
                var p = EncryptHelper.AesDecrpt(dbserver.Password);
                aesPwd = p;
            }
            catch
            {
            }
            
            switch (dbserver.DbTypeName.ToUpper())
            {
                case "DB2":
                    return string.Format("Server={0}:{1};Database={2};UID={3};PWD={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
                case "ORACLE":
                    return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
                case "MYSQL":
                    //return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, dbserver.Password);
                    break;
                case "SQLSERVER":
                    //return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, dbserver.Password);
                    break;
                default://默认oracle
                    return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
            }
            return "";
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public string GetConnectStr(long dbId)
        {
            var dbserver = GetDbServer(dbId);
            if (dbserver != null)
            {
                string aesPwd = dbserver.Password;
                try
                {
                    var p = EncryptHelper.AesDecrpt(dbserver.Password);
                    aesPwd = p;
                }
                catch
                {
                }
                switch (dbserver.DbTypeName.ToUpper())
                {
                    case "DB2":
                        return string.Format("Server={0}:{1};Database={2};UID={3};PWD={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
                    case "ORACLE":
                        return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
                    case "MYSQL":
                        //return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, dbserver.Password);
                        break;
                    case "SQLSERVER":
                        //return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, dbserver.Password);
                        break;
                    default://默认oracle
                        return string.Format("Data Source={0}:{1}/{2};User Id={3};Password={4};Connection Timeout =3600", dbserver.Ip, dbserver.Port, dbserver.DataCase, dbserver.User, aesPwd);
                }
            }
            return "";
        }

        #endregion

        #region 一些快捷的sql执行（无事务性的sql）
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public DataTable ExecuteGetTable(long dbId, string sql, ref ErrorInfo err)
        {
            DataTable newTable = new DataTable();//初始化查询结果表
            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    var dr = _session.ExecuteReader(sql);
                    newTable.Load(dr);//加载数据到datatable
                    _session.Closed();
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    err.IsError = true;
                    err.Message = ex.Message;
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return newTable;
        }
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteGetTable(long dbId, string sql)
        {
            DataTable newTable = new DataTable();//初始化查询结果表
            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    var dr = _session.ExecuteReader(sql);
                    newTable.Load(dr);//加载数据到datatable
                    _session.Closed();
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return newTable;
        }
        /// <summary>
        /// 执行sql得到一个DataTable
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteGetTable(DbServerOutput dbserver, string sql)
        {
            DataTable newTable = new DataTable();//初始化查询结果表
            var _session = GetSession(dbserver);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    var dr = _session.ExecuteReader(sql);
                    newTable.Load(dr);//加载数据到datatable
                    _session.Closed();
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return newTable;
        }
        /// <summary>
        /// 传入sql及分页信息得到一个DataTable
        /// </summary>
        /// <param name="dbServer"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable ExecuteGetTable(DbServerOutput dbServer, string sql, int pageIndex, int pageSize)
        {
            #region 设置分页
            string rownNum = "ROWNUM ";//初始化编号

            switch (dbServer.DbTypeName.ToUpper())
            {
                case "DB2":
                    rownNum = string.Format(" ROW_NUMBER() OVER( PARTITION BY 1 ) ");
                    break;
                case "ORACLE":
                    rownNum = "ROWNUM ";
                    break;
                case "SQLSERVER":
                    string nullSql = "SELECT * FROM ( " + sql + " ) T WHERE 1<>1";
                    DataTable dt = ExecuteGetTable(dbServer, nullSql);
                    rownNum = dt.Columns[0].Caption;
                    rownNum = string.Format("ROW_NUMBER() OVER (ORDER BY {0} DESC)", rownNum);
                    break;
            }
            int startNum = (pageIndex - 1) * pageSize;

            string nowsql = string.Format(@"        
                SELECT *
                FROM (SELECT {1} N, T.*
                        FROM ({0}) T)
                WHERE N > {2} AND N <= {3}", sql, rownNum, startNum, startNum + pageSize);
            #endregion

            #region 执行sql
            DataTable newTable = new DataTable();//初始化查询结果表
            var _session = GetSession(dbServer);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    var dr = _session.ExecuteReader(nowsql);
                    newTable.Load(dr);//加载数据到datatable
                    _session.Closed();
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            #endregion
            return newTable;
        }
        /// <summary>
        /// 传入sql及分页信息得到一个DataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable ExecuteGetTable(long dbId, string sql, int pageIndex, int pageSize)
        {
            DataTable newTable = new DataTable();//初始化查询结果表
            var dbServer = GetDbServer(dbId);
            if (dbServer != null)
            {
                #region 设置分页
                string rownNum = "ROWNUM ";//初始化编号

                switch (dbServer.DbTypeName.ToUpper())
                {
                    case "DB2":
                        rownNum = string.Format(" ROW_NUMBER() OVER( PARTITION BY 1 ) ");
                        break;
                    case "ORACLE":
                        rownNum = "ROWNUM ";
                        break;
                    case "SQLSERVER":
                        string nullSql = "SELECT * FROM ( " + sql + " ) T WHERE 1<>1";
                        DataTable dt = ExecuteGetTable(dbServer, nullSql);
                        rownNum = dt.Columns[0].Caption;
                        rownNum = string.Format("ROW_NUMBER() OVER (ORDER BY {0} DESC)", rownNum);
                        break;
                }
                int startNum = (pageIndex - 1) * pageSize;

                string nowsql = string.Format(@"        
                SELECT *
                FROM (SELECT {1} N, T.*
                        FROM ({0}) T)
                WHERE N > {2} AND N <= {3}", sql, rownNum, startNum, startNum + pageSize);
                #endregion

                #region 执行sql
                var _session = GetSession(dbServer);
                if (_session != null)
                {
                    try
                    {
                        _session.Open();
                        var dr = _session.ExecuteReader(nowsql);
                        newTable.Load(dr);//加载数据到datatable
                        _session.Closed();
                    }
                    catch (Exception ex)
                    {
                        _session.Closed();
                        throw new Exception("sql执行失败：" + ex.Message);
                    }
                    finally
                    {
                        _session.Closed();
                        _session.Dispose();
                    }
                }
                #endregion
            }
            return newTable;
        }

        /// <summary>
        /// 获得DataReader
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(long dbId, string sql)
        {
            IDataReader dr = null;
            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
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
        /// <summary>
        /// 获得DataReader
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(DbServerOutput dbserver, string sql)
        {
            IDataReader dr = null;
            var _session = GetSession(dbserver);
            if (_session != null)
            {
                try
                {
                    _session.Open();
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

        /// <summary>
        /// 执行sql返回一个值
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public object ExecuteScalar(long dbId, string sql, ref ErrorInfo err)
        {
            object obj = null;//初始化

            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    obj = _session.ExecuteScalar(sql);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    err.IsError = true;
                    err.Message = ex.Message;
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 执行sql返回一个值
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbServerOutput dbserver, string sql)
        {
            object obj = null;//初始化
            var _session = GetSession(dbserver);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    obj = _session.ExecuteScalar(sql);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 执行sql，根据dbserver
        /// </summary>
        /// <param name="dbserver"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(DbServerOutput dbserver, string sql)
        {
            int obj = 0;//初始化
            var _session = GetSession(dbserver);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    obj = _session.Execute(sql);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 根据数据库dbId执行sql
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(long dbId, string sql)
        {
            int obj = 0;//初始化

            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    obj = _session.Execute(sql);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj;
        }
        /// <summary>
        /// 根据数据库dbID执行sql插入dataTable
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="strDataTable"></param>
        /// <returns></returns>
        public bool Execute(long dbId, string sql, string strDataTable)//注意在这里，不通直接转DATATABLE类型的数据
        {
            DataTable dt = Easyman.Common.JSON.ToDataTable(strDataTable);//数据类型转换回datatable
            var result = false;
            var connectStr = GetConnectStr(dbId);
            var dbServer = GetDbServer(dbId);
            if (dbServer != null)
            {
                var dbType = dbServer.DbTypeName.ToLower();
                result = Common.DbHelper.ExecuteScalar(dbType, connectStr, sql, dt);
            }
            return result;
        }

        #region DataTable转成List

        #endregion

        #region 批量插入数据
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dbId">数据库ID</param>
        /// <param name="sql">Insert插入语句：Insert into Users(user_name,email,address) values (@UserName, @Email, @Address)</param>
        /// <param name="entList">要插入的数据集合.注意：类型的属性名与变量名UserName, Email, Address能够匹配</param>
        /// <returns></returns>
        public bool BatchImport<T>(long dbId, string sql, List<T> entList)
        {
            int obj = 0;//初始化

            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    obj = _session.Execute(sql, entList);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj > 0;
        }
        #endregion

        #region 批量插入数据
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="sql"></param>
        /// <param name="dt">DataTable类型，需要含有列名，且列名和传入sql中的变量名一样</param>
        /// <returns></returns>
        public bool BatchImport(long dbId, string sql, DataTable dt)
        {
            int obj = 0;//初始化

            var _session = GetSession(dbId);
            if (_session != null)
            {
                try
                {
                    _session.Open();
                    DbParameter[] paraArr = new DbParameter[dt.Columns.Count];//声明变量数
                    if (dt != null && dt.Columns.Count > 0)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            List<object> arr = new List<object>();
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    arr.Add(row[i]);
                                }
                            }
                            //DbParameter dbp = null;
                            //// IDbDataParameter dbp = new DataParameter();
                            //dbp.ParameterName = dt.Columns[i].ColumnName;
                            //dbp.DbType = GetDbType(dt.Columns[i].DataType.ToString());
                            //dbp.Direction = ParameterDirection.Input;
                            //dbp.Value = arr.ToArray();
                            //paraArr[i] = dbp;
                            // IDbDataParameter dbp = new DataParameter();
                            paraArr[i].ParameterName = dt.Columns[i].ColumnName;
                            paraArr[i].DbType = GetDbType(dt.Columns[i].DataType.ToString());
                            paraArr[i].Direction = ParameterDirection.Input;
                            paraArr[i].Value = arr.ToArray();
                        }
                    }
                    obj = _session.Execute(sql, paraArr);
                }
                catch (Exception ex)
                {
                    _session.Closed();
                    throw new Exception("sql执行失败：" + ex.Message);
                }
                finally
                {
                    _session.Closed();
                    _session.Dispose();
                }
            }
            return obj > 0;
        }
        #endregion

        #region 得到数据类型
        /// <summary>
        /// 得到数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public System.Data.DbType GetDbType(string type)
        {
            switch (type)
            {
                case "Int32":
                    return System.Data.DbType.Int32;
                case "Int16":
                    return System.Data.DbType.Int16;
                case "Int64":
                    return System.Data.DbType.Int64;
                case "DateTime":
                    return System.Data.DbType.DateTime;
                case "Decimal":
                    return System.Data.DbType.Decimal;
                case "Double":
                    return System.Data.DbType.Double;
                default:
                    return System.Data.DbType.String;

            }
        }
        #endregion


        #endregion

        #endregion

        #endregion
    }
}
