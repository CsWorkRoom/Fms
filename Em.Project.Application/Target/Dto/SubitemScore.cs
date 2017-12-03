using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    public class SubitemScore
    {
        public virtual string Month { get; set; }

        public virtual long? TargetTagId { get; set; }

        public virtual long? DistrictId { get; set; }

    }
}
