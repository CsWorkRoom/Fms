using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
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
    /// 打分项管理
    /// </summary>
    public class SubitemAppService : EasymanAppServiceBase, ISubitemAppService
    {
        #region 初始化

        private readonly IRepository<Subitem,long> _SubitemCase;
        private readonly IRepository<MonthSubitemScore, long> _SubitemScoreCase;


        /// <summary>
        /// 构造函数注入Subitem仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public SubitemAppService(IRepository<Subitem, long> SubitemCase,
            IRepository<MonthSubitemScore, long> SubitemScoreCase)
        {
            _SubitemCase = SubitemCase;
            _SubitemScoreCase = SubitemScoreCase;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据ID获取某个打分项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubitemModel GetSubitem(long id)
        {
            var entObj= _SubitemCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<SubitemModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 更新和新增打分项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SubitemModel InsertOrUpdateSubitem(SubitemModel input)
        {
            if(_SubitemCase.GetAll().Any(p=>p.Id!=input.Id&&p.Name==input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            //var entObj =input.MapTo<Subitem>();
            var entObj = _SubitemCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new Subitem();
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            //var entObj= AutoMapper.Mapper.Map<Subitem>(input);
            var resObj= _SubitemCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<SubitemModel>();
            }
        }

        /// <summary>
        /// 删除一条打分项
        /// </summary>
        /// <param name="input"></param>
        public void DeleteSubitem(EntityDto<long> input)
        {
            try
            {
                _SubitemCase.Delete(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("删除失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取打分项json
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetSubitemTreeJson()
        {
            var objList= _SubitemCase.GetAllList();
            if(objList!=null&& objList.Count>0)
            {
                return objList.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    open = false,
                    iconSkin = "menu"
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有类型 List<SelectListItem>
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> SubitemList()
        {
            var objList = _SubitemCase.GetAllList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
            }
            return null;
        }
        /// <summary>
        /// 获取所有类型 List<SubitemModel>
        /// </summary>
        /// <returns></returns>
        public List<SubitemModel> AllSubitemList()
        {
            var objList = _SubitemCase.GetAllList().OrderBy(p=>p.Id).ToList();
            if (objList != null && objList.Count > 0)
            {
                return objList.Select(p => {
                    var obj = p.MapTo<SubitemModel>();
                    obj.SubitemTypeName = p.SubitemType == null ? "" : p.SubitemType.Name;
                    return obj;
                }).ToList();
            }
            return null;
        }
        #endregion

        #region 领导打分
        /// <summary>
        /// 获取指定区县的打分项json串 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public string GetSubitemJson(SubitemScore ss)
        {
            return JSON.DecodeToStr( GetSubitemScoreTable(ss.DistrictId.Value, ss.Month));
        }
        /// <summary>
        /// 获取指定区县/月份的打分值table
        /// </summary>
        /// <param name="districtId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable GetSubitemScoreTable(long districtId, string month)
        {
            DataTable table = new DataTable();
            string sql = "";//初始化sql语句

            sql = GetSubitemSql(districtId, month);
            if (sql != "")
                table = DbHelper.ExecuteGetTable(sql);
            return table;
        }

        /// <summary>
        /// 循环保存打分值
        /// </summary>
        /// <param name="sbScores"></param>
        public void SaveSubitemScore(string sbScores)
        {
            if (!string.IsNullOrEmpty(sbScores) && sbScores.Length > 0)
            {
                try
                {
                    var scoreList = JSON.EncodeToEntity<List<MonthSubitemScoreModel>>(sbScores);
                    if (scoreList != null && scoreList.Count > 0)
                    {
                        foreach (var score in scoreList)
                        {
                            var val = score.MapTo<MonthSubitemScore>();
                            var hasVal = _SubitemScoreCase.FirstOrDefault(p => p.Month == score.Month &&
                             p.SubitemId == score.SubitemId &&
                             p.ManagerNo == score.ManagerNo);
                            if (hasVal != null)
                            {
                                val = hasVal;
                                val.MarkScore = score.MarkScore;
                                _SubitemScoreCase.UpdateAsync(val);
                            }
                            else
                            {
                                _SubitemScoreCase.InsertAsync(val);
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



        /// <summary>
        /// 私有方法：获取指定区县/月份打分项sql
        /// </summary>
        /// <param name="districtId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string GetSubitemSql(long districtId, string month)
        {
            string sql = "";

            string baseSql = string.Format(@"SELECT A.ID,
                   A.NAME,
                   A.MANAGER_NO,
                   A.MANAGER_NAME,
                   NVL (B.MARK_SCORE, 0) MARK_SCORE
              FROM (SELECT A.ID,
                           A.NAME,
                           A.WEIGHT,
                           B.MANAGER_NO,
                           B.MANAGER_NAME
                      FROM GP_SUBITEM A, GP_MANAGER B
                     WHERE B.DISTRICT_ID = {0}) A
                   LEFT JOIN
                   GP_MONTH_SUBITEM_SCORE B
                      ON (    A.ID = B.SUBITEM_ID
                          AND A.MANAGER_NO = B.MANAGER_NO
                          AND B.MONTH = '{1}')", districtId, month);

            var subitemList = AllSubitemList();
            if (subitemList != null && subitemList.Count > 0)
            {
                string sqlHead = "SELECT MANAGER_NO,MANAGER_NAME";
                foreach (var sub in subitemList)
                {

                    sqlHead += ",SUM(DECODE(ID," + sub.Id + " , MARK_SCORE)) SUBITEM_SCORE_" + sub.Id;//目标值
                }
                sql = sqlHead + " FROM (" + baseSql + ") GROUP BY MANAGER_NO,MANAGER_NAME";
            }
            return sql;
        }
        #endregion
    }
}
