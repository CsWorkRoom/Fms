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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Service
{
    /// <summary>
    /// 月度总奖金管理
    /// </summary>
    public class MonthBonusAppService : EasymanAppServiceBase, IMonthBonusAppService
    {
        #region 初始化

        private readonly IRepository<MonthBonus,long> _MonthBonusCase;
        private readonly IRepository<MonthTargetDetail,long> _MonthTargetDetailCase;
        /// <summary>
        /// 构造函数注入MonthBonus仓储
        /// </summary>
        /// <param name="dbTagManager"></param>
        public MonthBonusAppService(IRepository<MonthBonus, long> MonthBonusCase,
            IRepository<MonthTargetDetail, long> MonthTargetDetailCase)
        {
            _MonthBonusCase = MonthBonusCase;
            _MonthTargetDetailCase = MonthTargetDetailCase;
        }
        #endregion

        #region 月度奖金公共方法
        /// <summary>
        /// 根据ID获取某个月度总奖金
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonthBonusModel GetMonthBonus(long id)
        {
            var entObj= _MonthBonusCase.FirstOrDefault(id);
            if (entObj != null)
            {
               return AutoMapper.Mapper.Map<MonthBonusModel>(entObj);
            }
            throw new UserFriendlyException("未找到编号为【"+id.ToString()+"】的对象！");
        }
        /// <summary>
        /// 根据月份month获取某个月度总奖金
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public MonthBonusModel GetMonthBonus(string month)
        {
            if (!string.IsNullOrEmpty(month))
            {
                var bonus = _MonthBonusCase.FirstOrDefault(p => p.Month == month);
                if (bonus != null)
                    return bonus.MapTo<MonthBonusModel>();
                else
                    return null;
            }
            else
                return null;
        }
        /// <summary>
        /// 更新和新增月度总奖金
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MonthBonusModel InsertOrUpdateMonthBonus(MonthBonusModel input)
        {
            if(_MonthBonusCase.GetAll().Any(p=>p.Id!=input.Id&&p.Month==input.Month))
            {
                throw new UserFriendlyException("月份【" + input.Month + "】的对象已存在！");
            }
            // var entObj =input.MapTo<MonthBonus>();
            var entObj = _MonthBonusCase.GetAll().FirstOrDefault(x => x.Id == input.Id) ?? new MonthBonus();
            //var entObj= AutoMapper.Mapper.Map<MonthBonus>(input);
            entObj = Fun.ClassToCopy(input, entObj, (new string[] { "Id" }).ToList());
            var resObj= _MonthBonusCase.InsertOrUpdate(entObj);
            if (resObj == null)
            {
                throw new UserFriendlyException("新增或更新失败！");
            }
            else
            {
                return resObj.MapTo<MonthBonusModel>();
            }
        }

        /// <summary>
        /// 删除一条月度总奖金
        /// </summary>
        /// <param name="input"></param>
        public void DeleteMonthBonus(EntityDto<long> input)
        {         
                _MonthBonusCase.Delete(input.Id);
        }

        #endregion
    }
}
