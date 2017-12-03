using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(DbTag))]
    public class DbTagInput: EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name= "数据库标识名")]
        [Required(ErrorMessage ="数据库标识名不能为空")]
        public virtual string Name { get; set; }

        [Display(Name ="备注")]
        public virtual string Remark { get; set; }

    }

    [AutoMapFrom(typeof(DbTag))]
    public class DbTagOutput : EntityDto<long>
    {

        public new long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Remark { get; set; }

    }

    public class DbTagSearchInput : SearchInputDto<long>
    {
    }

    public class DbTagSearchOutput : SearchOutputDto<DbTagOutput,long>
    {
        public override Pager Page { get; set; }

        public override IEnumerable<DbTagOutput> Datas { get; set; }
    }
}
