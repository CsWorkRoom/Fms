using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;
using Abp.Domain.Entities;

namespace Easyman.Domain
{
    /// <summary>
    /// APP版本信息
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "APP_VERSION")]
    public class AppVersion : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("FILE_ID")]
        public virtual long FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual Files File { get; set; }

        [Column("VERSION_CODE")]
        public virtual long VersionCode { get; set; }

        [Column("VERSION_NAME")]
        public virtual string VersionName { get; set; }

        [Column("TYPE"), StringLength(20)]
        public virtual string Type { get; set; }

        [Column("IS_NEW")]
        public virtual int IsNew { get; set; }

        [Column("IS_MUST")]
        public virtual int IsMust { get; set; }

        [Column("UPGRADE_LOG"), StringLength(1000)]
        public virtual string UpgradeLog { get; set; }

        [Column("UPDATE_TIME")]
        public virtual DateTime UpdateTime { get; set; }

        [Column("UPDATE_URL"), StringLength(200)]
        public virtual string UpdateUrl { get; set; }

        public AppVersion()
        {

        }

        public AppVersion(int fileId)
        {
            FileId = fileId;
        }
    }
}
