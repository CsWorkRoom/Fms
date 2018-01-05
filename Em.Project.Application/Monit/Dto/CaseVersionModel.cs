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
    [AutoMap(typeof(CaseVersion))]
    public class CaseVersionModel : EntityDto<long>
    {
        public new long Id { get; set; }
        /// <summary>
        /// 所属实例ID
        /// </summary>
        public virtual long? ScriptNodeCaseId { get; set; }

        /// <summary>
        /// 文件版本批次ID
        /// </summary>
        public virtual long? FolderVersionId { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

    }
}
