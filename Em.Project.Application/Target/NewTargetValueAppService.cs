using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Service
{
    /// <summary>
    /// 指标目标
    /// </summary>
    public class NewTargetValueAppService : EasymanAppServiceBase, INewTargetValueAppService
    {
        #region 初始化

        private readonly IRepository<TargetValue,long> _TargetValueCase;
        private readonly IRepository<Target,long> _TargetCase;
        private readonly IRepository<Manager, long> _ManagerCase;
        private readonly IRepository<District, long> _DistrictCase;


        /// <summary>
        /// 构造函数注入仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public NewTargetValueAppService(IRepository<Target, long> TargetCase,
            IRepository<TargetValue, long> TargetValueCase,
            IRepository<Manager, long> ManagerCase,
            IRepository<District, long> DistrictCase)
        {
            _TargetCase = TargetCase;
            _TargetValueCase = TargetValueCase;
            _ManagerCase = ManagerCase;
            _DistrictCase = DistrictCase;
        }
        #endregion

        #region 获得当前指标目标值列表
        /// <summary>
        /// 获得区县指标的json串
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <param name="districtId"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public string GetQxTargetValueJson(QxTargetVal tv)
        {
            return JSON.DecodeToStr(GetTargetValueTable(tv.TargetTagId.Value,tv.DistrictId.Value,tv.Year));
        }
        /// <summary>
        /// 获得指标的table
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <param name="districtId"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public DataTable GetTargetValueTable(long targetTagId,long districtId,string yearMonth)
        {
            DataTable table = new DataTable();
            string sql = "";//初始化sql语句

            if (targetTagId == 1)//区县的指标
            {
                sql = GetQxTargetSql(targetTagId, districtId, yearMonth);
                if (sql != "")
                    table = DbHelper.ExecuteGetTable(sql);
            }
            else if (targetTagId == 2)//客户经理的指标
            {
                sql = GetManagerSql(targetTagId, districtId, yearMonth);
                if (sql != "")
                    table = DbHelper.ExecuteGetTable(sql);
            }
            return table;
        }
        #endregion

        #region 行转列-基准列
        /// <summary>
        /// 获取指定区县的客户经理列表
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public string  GetManagerJson(long? districtId)
        {
            string result = "";
            if(districtId!=null)
            {
                //获取当前区县的客户经理（后期需要过滤是否打标）
                var managerList = _ManagerCase.GetAllList(p => p.DistrictId == districtId);
                if (managerList != null && managerList.Count > 0)
                {
                    result= JSON.DecodeToStr(managerList.Select(p => new { p.ManagerNo, p.ManagerName,p.DistrictId,p.Id }).OrderBy(p=>p.Id));
                }
            }
            else//获取全部客户经理
            {
                var managerList = _ManagerCase.GetAllList();
                if (managerList != null && managerList.Count > 0)
                {
                    result = JSON.DecodeToStr(managerList.Select(p => new { p.ManagerNo, p.ManagerName,p.DistrictId,p.Id }).OrderBy(p=>p.Id));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取区县指标列表json串
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <returns></returns>
        public string GetQxTargetJson(long? targetTagId)
        {
            string result = "";
            if (targetTagId != null)
            {
                var targetList = _TargetCase.GetAllList(p => p.TargetTagId == targetTagId.Value && p.IsUse);
                if (targetList != null && targetList.Count > 0)
                {
                    result = JSON.DecodeToStr(targetList.Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Weight,
                        p.TargetTypeId,
                        TargetTypeName = p.TargetType.Name//指标类型名
                    }).OrderBy(p => p.TargetTypeId).ThenBy(p => p.Id).ToList());//根据指标类型、指标排序
                }
            }
            return result;
        }
        #endregion

        #region 目标值保存
        /// <summary>
        /// 保存客户经理的指标目标值
        /// </summary>
        /// <param name="vals"></param>
        public void SaveManagerTargetValue(TargetVals vals)
        {
            if(vals!=null&&vals.TargetValues.Length>0)
            {
                var valList = JSON.EncodeToEntity<List<TargetValueModel>>(vals.TargetValues);
                if(valList!=null&&valList.Count>0)
                {
                    foreach (var val in valList)
                    {
                        var tv= _TargetValueCase.FirstOrDefault(p => p.YearMonth == vals.Month &&
                        p.TargetId == val.TargetId &&
                        p.DistrictId == vals.DistrictId &&
                        p.TargetTagId == vals.TargetTagId &&
                        p.ManagerNo == val.ManagerNo);
                        if (tv != null)
                        {
                            tv.TValue = val.TValue;
                        }
                        else
                            {
                            tv = new TargetValue {YearMonth=vals.Month,
                                TargetTagId =vals.TargetTagId,
                                DistrictId =vals.DistrictId ,
                              ManagerNo=val.ManagerNo,
                            ManagerName=val.ManagerName,
                             TargetId=val.TargetId,
                             TValue=val.TValue,
                             };
                        }
                        _TargetValueCase.InsertOrUpdateAsync(tv);
                    }
                }
            }
        }

        /// <summary>
        /// 保存区县指标的目标值
        /// </summary>
        /// <param name="vals"></param>
        public void SaveQxTargetValue(string vals)
        {
            if(!string.IsNullOrEmpty(vals)&&vals.Length>0)
            {
                try
                {
                    var targetValueList = JSON.EncodeToEntity<List<TargetValueModel>>(vals);
                    if (targetValueList != null && targetValueList.Count > 0)
                    {
                        foreach (var tgval in targetValueList)
                        {
                            var val = tgval.MapTo<TargetValue>();
                            var hasVal = _TargetValueCase.FirstOrDefault(p => p.TargetId == tgval.TargetId &&
                             p.YearMonth == tgval.YearMonth &&
                             p.TargetTagId == tgval.TargetTagId &&
                             p.DistrictId == tgval.DistrictId);
                            if (hasVal != null)
                            {
                                val = hasVal;
                                val.TValue = tgval.TValue;
                                _TargetValueCase.UpdateAsync(val);
                            }
                            else
                            {
                                _TargetValueCase.InsertAsync(val);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception("传入的指标信息不能为空！");
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取客户经理目标值sql
        /// </summary>
        /// <param name="targetTagId"></param>
        /// <param name="districtId"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        private string GetManagerSql(long targetTagId, long districtId, string yearMonth)
        {
            string sql = "";

            #region baseSql
            string baseSql = string.Format(@"SELECT A.ID,
                    A.TARGET_TYPE_ID,
                    D.NAME TARGET_TYPE_NAME,
                    A.TARGET_TAG_ID,
                    A.NAME,
                    A.WEIGHT,
                    A.MANAGER_NO,
                    A.MANAGER_NAME,
                    NVL (B.TVALUE, 0) TVALUE,
                    NVL (C.RESULT_VALUE, 0) RESULT_VALUE
                FROM (SELECT A.ID,
                            A.TARGET_TYPE_ID,
                            A.TARGET_TAG_ID,
                            A.NAME,
                            A.WEIGHT,
                            A.IS_USE,
                            B.MANAGER_NO,
                            B.MANAGER_NAME
                        FROM GP_TARGET A, GP_MANAGER B
                        WHERE A.TARGET_TAG_ID = {1} AND B.DISTRICT_ID = {2} AND A.IS_USE = 1) A
                    LEFT JOIN
                    GP_TARGET_VALUE B
                        ON (    A.ID = B.TARGET_ID
                            AND B.YEAR_MONTH = '{0}'
                            AND A.MANAGER_NO = B.MANAGER_NO)
                    LEFT JOIN
                    GP_MONTH_TARGET_DETAIL C
                        ON (    A.ID = C.TARGET_ID
                            AND C.MONTH =
                                    TO_CHAR (
                                    (TO_DATE ('{0}', 'yyyyMM') - INTERVAL '1' MONTH),
                                    'YYYYMM')
                            AND A.MANAGER_NO = C.MANAGER_NO)
                    LEFT JOIN GP_TARGET_TYPE D ON (A.TARGET_TYPE_ID = D.ID)", yearMonth, targetTagId, districtId);
            #endregion

            //获得当前有效的客户经理并动态拼凑行转列的sql
            var managerList = _ManagerCase.GetAllList(p => p.DistrictId == districtId).OrderBy(p => p.Id).ToList();//获取当前区县的客户经理（后期需要过滤是否打标）
            if (managerList != null && managerList.Count > 0)
            {
                string sqlHead = "SELECT ID,TARGET_TYPE_ID,TARGET_TYPE_NAME,TARGET_TAG_ID, NAME, WEIGHT";
                foreach (var ma in managerList)
                {
                    //遍历拼凑头部
                    sqlHead += ",SUM(DECODE(MANAGER_NO,'" + ma.ManagerNo + "',TVALUE)) TARGET_V_" + ma.ManagerNo;//目标值
                    sqlHead += ",SUM(DECODE(MANAGER_NO,'" + ma.ManagerNo + "',RESULT_VALUE)) LAST_V_" + ma.ManagerNo;//上月完成值
                }
                sql = sqlHead + " FROM (" + baseSql + ") GROUP BY ID,TARGET_TYPE_ID,TARGET_TYPE_NAME,TARGET_TAG_ID, NAME, WEIGHT";
            }
            return sql;
        }
       /// <summary>
       /// 获得区县指标目标值sql
       /// </summary>
       /// <param name="targetTagId"></param>
       /// <param name="districtId"></param>
       /// <param name="year"></param>
       /// <returns></returns>
        private string GetQxTargetSql(long targetTagId, long districtId, string year)
        {
            string sql = "";
            string baseSql = string.Format(@"SELECT A.ID,
                   A.WEIGHT,
                   A.DISTRICT_ID,
                   A.DISTRICT_NAME,
                   NVL (B.TVALUE, 0) TVALUE
              FROM (SELECT A.ID,
                           A.WEIGHT,
                           A.IS_USE,
                           B.ID DISTRICT_ID,
                           B.NAME DISTRICT_NAME
                      FROM GP_TARGET A, EM_DISTRICT B
                     WHERE A.TARGET_TAG_ID = {1} AND B.PARENT_ID = {2} AND A.IS_USE = 1) A
                   LEFT JOIN
                   GP_TARGET_VALUE B
                      ON (    A.ID = B.TARGET_ID
                          AND B.YEAR_MONTH = '{0}'
                          AND A.DISTRICT_ID = B.DISTRICT_ID)", year, targetTagId, districtId);

            var qxTargetList = _TargetCase.GetAllList(p => p.TargetTagId == 1).OrderBy(p => p.TargetTypeId).ThenBy(p => p.Id).ToList();
            if(qxTargetList!=null&&qxTargetList.Count>0)
            {
                string sqlHead = "SELECT DISTRICT_ID,DISTRICT_NAME";
                foreach (var tg in qxTargetList)
                {

                    sqlHead += ",SUM(DECODE(ID, " + tg.Id + ", TVALUE)) TARGET_V_" + tg.Id;//目标值
                }
                sql = sqlHead + " FROM (" + baseSql + ") GROUP BY DISTRICT_ID,DISTRICT_NAME";
            }
            return sql;
        }
        #endregion
    }
}
