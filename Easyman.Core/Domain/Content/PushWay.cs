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
    /// 推送类型
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "PUSH_WAY")]
    public class PushWay : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 推送模式名称
        /// </summary>
        [Column("NAME"), StringLength(200)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 推送模式说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
