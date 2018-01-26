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
    [AutoMap(typeof(FileUpload))]
    public class FileUploadModel : EntityDto<long>
    {
        public new long Id { get; set; }

        public virtual int UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual DateTime UploadTime { get; set; }

        public virtual string FileName { get; set; }

        public virtual string FilePath { get; set; }
    }
}
