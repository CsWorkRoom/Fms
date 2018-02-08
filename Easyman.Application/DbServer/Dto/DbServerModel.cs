using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{

    [AutoMap(typeof(DbServer))]
    public class DbServerInput : EntityDto<long>
    {

        public new long Id { get; set; }

        [Display(Name = "别名")]
        [Required(ErrorMessage = "别名不能为空")]
        public virtual string ByName { get; set; }

        public virtual long? DbTagId { get; set; }

        //[Display(Name = "数据库标识")]
        //public virtual DbTag DbTag { get; set; }

        public virtual long? DbTypeId { get; set; }

        //[Display(Name = "数据库种类")]
        //public virtual DbType DbType { get; set; }

        [Display(Name = "IP")]
        public virtual string Ip { get; set; }

        [Display(Name = "端口")]
        public virtual int? Port { get; set; }

        [Display(Name = "实例名")]
        public virtual string DataCase { get; set; }

        [Display(Name = "用户名")]
        public virtual string User { get; set; }

        [Display(Name = "密码")]
        public virtual string Password { get; set; }

        [Display(Name = "备注")]
        public virtual string Remark { get; set; }

    }

    [AutoMap(typeof(DbServer))]
    public class DbServerOutput : EntityDto<long>
    {

        public new long Id { get; set; }

        public virtual string ByName { get; set; }

        public virtual long? DbTagId { get; set; }

        //public virtual DbTag DbTag { get; set; }

        public virtual string DbTagName { get; set; }

        public virtual long? DbTypeId { get; set; }

        //[Display(Name = "数据库种类")]
        //public virtual DbType DbType { get; set; }

        public virtual string DbTypeName { get; set; }

        public virtual string Ip { get; set; }

        public virtual int? Port { get; set; }

        public virtual string DataCase { get; set; }

        public virtual string User { get; set; }

        public virtual string Password { get; set; }

        public virtual string Remark { get; set; }

    }

    public class DbServerSearchInput : SearchInputDto<long>
    {
    }

    public class DbServerSearchOutput : SearchOutputDto<DbServerOutput, long>
    {
        public override Pager Page { get; set; }

        public override IEnumerable<DbServerOutput> Datas { get; set; }
    }
}
