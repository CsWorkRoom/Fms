using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Easyman.Dto
{
    [AutoMap(typeof(MonitLog))]
    public class MonitLogModel : EntityDto<long>
    {
        public new long Id { get; set; }

        /// <summary>
        /// 监控版本ID
        /// </summary>
        public virtual long? CaseVersionId { get; set; }

        /// <summary>
        /// 文件及文件夹ID
        /// </summary>
        public virtual long? MonitFileId { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public virtual short? LogType { get; set; }


        /// <summary>
        /// 日志信息
        /// </summary>
        public virtual string LogMsg { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public virtual DateTime? LogTime { get; set; }
    }
}
