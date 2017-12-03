using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Service
{
    /// <summary>
    /// 月度总奖金管理
    /// </summary>
    public class MonthTargetAppService : EasymanAppServiceBase, IMonthTargetAppService
    {
        #region 初始化
        private readonly IMonthBonusAppService _MonthBonusService;
        private readonly IRepository<MonthBill, long> _MonthBillCase;
        private readonly IRepository<MonthBillLog, long> _MonthBillLogCase;

        private readonly IRepository<Target, long> _TargetCase;
        private readonly IRepository<MonthTarget, long> _MonthTargetCase; 
        private readonly IRepository<MonthTargetDetail, long> _MonthTargetDetailCase;
        private readonly IRepository<Manager, long> _ManagerCase;
        private readonly IRepository<District, long> _DistrictCase;
        /// <summary>
        /// 构造函数注入仓储
        /// </summary>
        /// <param name="MonthBonusService"></param>
        public MonthTargetAppService(IMonthBonusAppService MonthBonusService,
            IRepository<MonthBill, long> MonthBillCase,
            IRepository<MonthBillLog, long> MonthBillLogCase,
            IRepository<Target, long> TargetCase,
            IRepository<MonthTarget, long> MonthTargetCase,
            IRepository<MonthTargetDetail, long> MonthTargetDetailCase,
            IRepository<Manager, long> ManagerCase,
            IRepository<District, long> DistrictCase)
        {
            _MonthBonusService = MonthBonusService;
            _MonthBillCase = MonthBillCase;
            _MonthBillLogCase = MonthBillLogCase;
            _TargetCase = TargetCase;
            _MonthTargetDetailCase = MonthTargetDetailCase;
            _MonthTargetCase = MonthTargetCase;
            _ManagerCase = ManagerCase;
            _DistrictCase = DistrictCase;
        }
        #endregion


        #region 公共方法
        [UnitOfWork]
        public void CuringMonthTarget(CurTarget obj)
        {
            //string month = GetPropertyValue(obj,"month").ToString();
            //string curWay = GetPropertyValue(obj, "curWay").ToString();
            string month = obj.Month;
            string curWay = obj.CurWay;
            MonthBonusModel bonus = new MonthBonusModel();
            var result = ValidateMonth(month, ref bonus);
            if (result == "")
            {
                var user = GetCurrentUserAsync().Result;
                //1.生成固化单
                #region bill
                MonthBill bill = new MonthBill
                {
                    Id = 0,
                    BonusValue = bonus.BonusValue,
                    CurTime = DateTime.Now,
                    CurUserId = user.Id,//外部调用可能会报错
                    IsUse = true,
                    Month = month,
                    Status = MonthBillConst.generate,
                    CurWay = curWay
                };
                #endregion
                long billId = _MonthBillCase.InsertAndGetIdAsync(bill).Result;//获得固化单ID

                //写日志
                MonthLog(billId, "开始启动生成月份【" + month + "】固化单【" + billId.ToString() + "】：启动人【" + user.Name + "】");
                //2.生成月度指标内容
                var targetList = _TargetCase.GetAllList();
                if (targetList != null && targetList.Count > 0)
                {
                    try
                    {
                        var managerList = _ManagerCase.GetAllList();//获取客户经理信息装入内存中
                        var districtList = _DistrictCase.GetAllList(p=>p.CurLevel==2);//获取所有二级组织（区县）

                        for (int i = 0; i < targetList.Count; i++)
                        {
                            var target = targetList[i];
                            MonthLog(billId, "===Start======固化指标【" + target.Name + "】编号【" + target.Id.ToString() + "】======");
                            MonthLog(billId, "->本次固化共有【" + targetList.Count.ToString() + "】个指标");

                            MonthLog(billId, "->第【" + i.ToString() + "】个指标：建立MonthTarget实例单开始");
                            #region mTarg
                            MonthTarget mTarg = new MonthTarget
                            {
                                Id = 0,
                                ChooseType = target.ChooseType,
                                EndTable = target.EndTable,
                                MainField = target.MainField,
                                Month = month,
                                MonthBillId = billId,
                                Weight = target.Weight,
                                Remark = target.Remark,
                                TargetId = target.Id,
                                TargetName = target.Name,
                                TargetTagId = target.TargetTagId,
                                TargetTypeId = target.TargetTypeId,
                                CrisisValue = target.CrisisValue//计分门槛值
                            };
                            #endregion
                            long monthTargetId = _MonthTargetCase.InsertAndGetIdAsync(mTarg).Result;//得到当前固化指标ID
                            MonthLog(billId, "->第【" + i.ToString() + "】个指标：建立MonthTarget实例单【" + monthTargetId.ToString() + "】完成");
                            //3.生成月度指标明细（当未获取到目标值时，默认为0）
                            InitMonthTargetDetail( month,  monthTargetId, billId,  target,managerList,districtList);

                            MonthLog(billId, "===End======固化指标【" + target.Name + "】编号【" + target.Id.ToString() + "】======");
                        }
                    }
                    catch (Exception ex)
                    {
                        bill.Status = MonthBillConst.error;
                        bill.IsUse = false;
                        _MonthBillCase.UpdateAsync(bill);
                        string errStr = "月份【" + month + "】固化单生成失败.错误：" + ex.InnerException.Message;
                        MonthLog(billId, "失败：" + errStr);
                        bill.Status = MonthBillConst.error;
                        bill.IsUse = false;
                        bill.Remark = errStr;
                        _MonthBillCase.UpdateAsync(bill);
                        throw new Exception(errStr);
                    }
                }
                bill.Status = MonthBillConst.success;
                _MonthBillCase.UpdateAsync(bill);

                //_MonthBillCase.InsertAsync(bill);
                MonthLog(billId, "结束：月份【" + month + "】固化单生成全部完成。成功");
            }
            else
                throw new Exception(result);
        }


        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="field">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }
        #endregion

        /// <summary>
        /// 验证月固化信息
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string ValidateMonth(string month, ref MonthBonusModel bonus)
        {
            string result = "";
            if (string.IsNullOrEmpty(month))
            {
                result = "待固化月份值不能为空!";
            }
            else
            {
                bonus = _MonthBonusService.GetMonthBonus(month);
                if (bonus != null)
                {
                    var bill = _MonthBillCase.FirstOrDefault(p => p.Month == month && p.IsUse);
                    if (bill != null)
                    {
                        result = "月度【" + month + "】已产生固化单,编号为【" + bill.Id.ToString() + "】。";
                    }
                }
                else
                    result = "请先设定月度【" + month + "】总奖金!";
            }
            return result;
        }

        /// <summary>
        /// 写入固化日志
        /// </summary>
        /// <param name="monthBillId"></param>
        /// <param name="log"></param>
        private void MonthLog(long monthBillId, string log, string logResult = "", bool commit = false)
        {
            try
            {
                _MonthBillLogCase.InsertAsync(new MonthBillLog { Id = 0, MonthBillId = monthBillId, Log = log, LogTime = DateTime.Now, LogResult = logResult });
                if (commit)
                    CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 初始化月度指标明细
        /// 目前直接把权重当作指标分数来看待
        /// </summary>
        /// <param name="month"></param>
        /// <param name="monthTargetId"></param>
        /// <param name="monthBillId"></param>
        /// <param name="target"></param>
        /// <param name="managerList"></param>
        private void InitMonthTargetDetail(string month, long monthTargetId, long monthBillId, Target target, List<Manager> managerList,List<District> districtList)
        {
            if (target.TargetTagId == 1)//区县指标
            {
                if (districtList != null && districtList.Count > 0)
                {
                    MonthLog(monthBillId, "  →批量给指标生成区县指标明细开始");
                    foreach (var dis in districtList)
                    {
                        double? tval = 0;
                        var tvs = target.TargetValue.Where(p => p.DistrictId == dis.Id && p.YearMonth == month.Substring(0,4)).ToList();
                        if(tvs!=null&&tvs.Count>0)
                        {
                            tval = tvs[0].TValue;
                        }
                        MonthTargetDetail detail = NewTargetDetail(month, monthBillId, target, monthTargetId);
                        detail.DistrictId = dis.Id;//区县id
                        detail.TValue = tval * Convert.ToInt16(month.Substring(4)) / 12;//区县的目标值根据年度比例得出
                        _MonthTargetDetailCase.InsertAndGetIdAsync(detail);
                    }
                    MonthLog(monthBillId, "  ←批量给指标生成区县指标明细完成");
                }
            }
            else if (target.TargetTagId == 2)//客户经理
            {
                if (managerList != null && managerList.Count > 0)
                {
                    MonthLog(monthBillId, "  →批量给指标生成客户经理指标明细开始");
                    foreach (var man in managerList)
                    {
                        double? tval = 0;
                        
                        if(man.DistrictId!=null)
                        {
                            //var tvs = target.TargetValue.Where(p => p.ManagerNo==man.ManagerNo && p.YearMonth == month).ToList();
                            //if (tvs != null && tvs.Count > 0)
                            //{
                            //    tval = tvs[0].TValue;
                            //}
                            MonthTargetDetail detail = NewTargetDetail(month, monthBillId, target, monthTargetId);
                            detail.ManagerNo = man.ManagerNo;//客户经理
                            detail.ManagerName = man.ManagerName;//客户经理名
                            detail.DistrictId = man.DistrictId;//客户经理归属区县
                            detail.TValue = tval;
                            _MonthTargetDetailCase.InsertAndGetIdAsync(detail);
                        }
                    }
                    MonthLog(monthBillId, "  ←批量给指标生成客户经理指标明细完成");
                }
            }
        }

        private MonthTargetDetail NewTargetDetail(string month,long monthBillId, Target target, long monthTargetId)
        {
            MonthTargetDetail detail = new MonthTargetDetail
            {
                Id = 0,
                Month = month,
                MonthBillId= monthBillId,//固化单
                MonthTargetId = monthTargetId,
                TargetTagId = target.TargetTagId,
                TargetTypeId = target.TargetTypeId,
                TargetId = target.Id,
                EndTable = target.EndTable,
                MainField = target.MainField,
                Weight = target.Weight,
                CrisisValue = target.CrisisValue//计分门槛值
            };
            return detail;
        }

    }
}
