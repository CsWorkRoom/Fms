using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;

namespace Easyman.App.Dto
{
    /// <summary>
    /// APP版本输入对象
    /// </summary>
    [AutoMapFrom(typeof(AppVersion))]
    public class AppVersionInput : EntityDto
    {
        public new int Id { get; set; }

        public long FileId { get; set; }

        [Display(Name = "版本编码")]
        [Required(ErrorMessage = "版本编码不能为空")]
        public string VersionCode { get; set; }

        [Display(Name = "版本名")]
        [Required(ErrorMessage = "版本名不能为空")]
        public string VersionName { get; set; }

        [Display(Name = "APP类型")]
        [Required(ErrorMessage = "APP类型不能为空")]
        public string Type { get; set; }

        [Display(Name = "是否设为最新")]
        public bool IsNew { get; set; }

        [Display(Name = "是否强制更新")]
        public bool IsMust { get; set; }

        [Display(Name = "版本更新日志")]
        public string UpgradeLog { get; set; }

        public DateTime UpdateTime { get; set; }

        [Display(Name = "版本更新地址")]
        [Required(ErrorMessage = "版本更新地址不能为空")]
        public string UpdateUrl { get; set; }
    }
}
