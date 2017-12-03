using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using Easyman.Users;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMap(typeof(UserPwdLog))]
    public class UserPwdLogDto : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 用户编号：USER_ID
        /// </summary>
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 原密码
        /// </summary>
        public virtual string OldPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public virtual string NewPwd { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }

    
}
