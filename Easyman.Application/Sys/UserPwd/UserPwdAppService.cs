using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using Easyman.Users;
using EasyMan;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easyman.Sys
{
    public class UserPwdAppService : IUserPwdAppService
    {
        #region 初始化

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserPwdLog, long> _userPwdLogRepository;

        public UserPwdAppService(IRepository<User, long> userRepository, IRepository<UserPwdLog, long> userPwdLogRepository)
        {
            _userRepository = userRepository;
            _userPwdLogRepository = userPwdLogRepository;
        }

        #endregion

        public User GetUser(long id)
        {
            return _userRepository.Get(id);
        }

        public User GetUser(string name)
        {
            return _userRepository.FirstOrDefault(p => p.UserName == name);
        }

        public void InsertOrUpdate(User user)
        {
            _userRepository.InsertOrUpdateAsync(user);
        }

        /// <summary>
        /// 插入一条密码修改记录
        /// </summary>
        /// <param name="pwdLog"></param>
        public void InsertUserPwdLog(UserPwdLogDto pwdLog) {
            var log = pwdLog.MapTo<UserPwdLog>();
            log.CreationTime = DateTime.Now;
            _userPwdLogRepository.Insert(log);
        }


        /// <summary>
        /// 获取用户的所有密码修改记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserPwdLogDto> GetAllPwdLog(long userId)
        {
            return _userPwdLogRepository.GetAllList(p => p.UserId == userId).OrderByDescending(o => o.Id).MapTo<List<UserPwdLogDto>>();
            //return null;
        }
        /// <summary>
        /// 获取用户的最后一条密码修改记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserPwdLogDto GetLastPwdLog(long userId)
        {
            var logList = _userPwdLogRepository.GetAllList(p => p.UserId == userId);
            if (logList != null && logList.Count > 0)
            {
                return logList.Max(p => p.Id).MapTo<UserPwdLogDto>();
            }
            else
                return null;
            //return _userPwdLogRepository.GetAllList(p => p.UserId == userId).Max(p => p.Id).MapTo<UserPwdLogDto>();
            //return new UserPwdLogDto { CreationTime = DateTime.Now.AddDays(-2) };
        }
    }
}
