using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;

namespace Easyman.App.Dto
{
    /// <summary>
    /// APP版本输出对象
    /// </summary>
    [AutoMapFrom(typeof(AppVersion))]
    public class AppVersionOutput : EntityDto
    {
        public new int Id { get; set; }

        public long FileId { get; set; }

        public virtual long VersionCode { get; set; }

        public virtual string VersionName { get; set; }

        public virtual string Type { get; set; }

        public virtual int IsNew { get; set; }

        public virtual int IsMust { get; set; }

        public virtual string UpgradeLog { get; set; }

        public virtual DateTime UpdateTime { get; set; }

        public virtual string UpdateUrl { get; set; }
    }
}
