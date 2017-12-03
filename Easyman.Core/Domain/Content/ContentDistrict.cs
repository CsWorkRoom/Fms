using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Authorization.Roles;
using Easyman.Common;
using Easyman.Users;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容与组织关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_DISTRICT")]
    public class ContentDistrict : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("DISTRICT_ID")]
        public virtual long DistrictId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [Column("IS_ALLOW")]
        public virtual bool? IsAllow { get; set; }

        public ContentDistrict()
        {

        }

        public ContentDistrict(int districtId, long contentId)
        {
            DistrictId = districtId;
            ContentId = contentId;
        }
    }
}
