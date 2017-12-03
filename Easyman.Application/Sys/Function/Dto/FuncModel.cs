using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(Function))]
    public class FunctionInput : EntityDto
    {
        public FunctionInput()
        {
            RoleIds = "";
            TenantId = null;
        }

        public new int Id { get; set; }

        [Required]
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual string Discribition { get; set; }

        public virtual string Type { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public string RoleIds { get; set; }
    }

    [AutoMapFrom(typeof(Function))]
    public class FunctionOutput : EntityDto
    {
        public new string Id { get; set; }

        public int TenantId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public virtual string Discribition { get; set; }

        public virtual string Type { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }

    public class FunctionSerachInput : SearchInputDto
    {
    }

    public class FunctionSerachOutput : SearchOutputDto<FunctionOutput>
    {

    }
}
