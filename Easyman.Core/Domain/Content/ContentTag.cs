using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容标签项目明细
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_TAG")]
    public class ContentTag : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// TYPE
        ///  </summary>
        [Column("TYPE"),StringLength(50)]
        public virtual string Type { get; set; }

        /// <summary>
        /// 标签内容
        /// </summary>
        [Column("INFO"), StringLength(260)]
        public virtual string Info { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [Column("REMARK")]
        public virtual string Remark { get; set; }

        //public virtual ICollection<ContentTag> ChildContentTag { get; set; }
    }
}
