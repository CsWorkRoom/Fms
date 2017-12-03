using Abp.Application.Services.Dto;
using Easyman.Domain;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using EasyMan.Dtos;
using System;

namespace Easyman.Dto
{

    [AutoMap(typeof(Param))]
    public class ParamModel 
    {
        public virtual long Id { get; set; }

        public virtual long? TbReportOutEventId { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsField { get; set; }

        public virtual string FieldCode { get; set; }

        public virtual string PValue { get; set; }

        public virtual string Remark { get; set; }

        /// <summary>
        /// 参数顺序号
        /// </summary>
        public virtual int? OrderNum { get; set; }
    }
}