using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.Base.Content.Dto;
using Easyman.Content.Dto;
using Easyman.Domain;
using Easyman.Users;
using EasyMan;
using EasyMan.Dtos;
using Newtonsoft.Json;
using Abp.Application.Services.Dto;

namespace Easyman.Content
{
    /// <summary>
    /// 推送管理
    /// </summary>
    public class PushwayAppService : EasymanAppServiceBase, IPushwayAppService
    {
        #region 初始化
      
        private readonly IRepository<PushWay, long> _pushWayRepository;
        private readonly IRepository<ContentPushWay, long> _contentPushWayRepository;
       /// <summary>
       /// 
       /// </summary>
       /// <param name="pushWayRepository"></param>
       /// <param name="contentPushWayRepository"></param>
        public PushwayAppService( IRepository<PushWay, long> pushWayRepository, IRepository<ContentPushWay, long> contentPushWayRepository)
        {           
            _pushWayRepository = pushWayRepository;      
            _contentPushWayRepository = contentPushWayRepository;
        }
        #endregion
        
        #region 推送模式管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PushWaySearchOutput SearchPushWay(PushWaySearchInput input)
        {
            int rowCount;
            var data = _pushWayRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new PushWaySearchOutput
            {
                Datas = data.ToList().Select(s =>
                {
                    var temp = s.MapTo<PushWayOutput>();

                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="input"></param>
        public void UpdateOrInserPushway(PushWayInput input)
        {
            var type = _pushWayRepository.GetAll().FirstOrDefault(a => a.Id == input.Id) ?? new PushWay();
            if (_pushWayRepository.GetAll().Any(p => p.Id != input.Id && p.Name == input.Name))
            {
                throw new UserFriendlyException("名为【" + input.Name + "】的对象已存在！");
            }
            type.Name = input.Name;
            type.Remark = input.Remark;
            _pushWayRepository.InsertOrUpdate(type);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PushWayInput GetPushway(long id)
        {
            return _pushWayRepository.Get(id).MapTo<PushWayInput>();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        public void DelPushway(EntityDto<long> input)
        {
            var type = _pushWayRepository.Get(input.Id);
            var content = _contentPushWayRepository.GetAll().Count(a => a.PushWayId == input.Id);
            if (content > 0)
            {
                throw new UserFriendlyException("删除出错，推送模式正在被使用，请先删除在使用的推送模式，在执行此删除操作！");

            }
            else
            {
                _pushWayRepository.Delete(type);

            }
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetPushways()
        {
            return _pushWayRepository.GetAll().Select(a => new SelectListItem() { Text = a.Name, Value = a.Id.ToString() }).ToList();

        }
        public List<PushWay> GetPushWay()
        {
            return _pushWayRepository.GetAll().ToList();
        }

        public bool IsPushWay(int id, int contentId)
        {
            var push = _contentPushWayRepository.GetAll().Where(a => a.PushWayId == id && a.ContentId == contentId);
            if (push.Any())
                return true;
            else
                return false;
        }
        #endregion

    }
}
