
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
    public class CurTarget 
    {
        /// <summary>
        /// 指标权重
        /// </summary>
        public virtual string Month { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        public virtual string CurWay { get; set; }
     

    }
}
