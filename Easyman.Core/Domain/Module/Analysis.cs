using Abp.Domain.Entities;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 解析地址
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "ANALYSIS")]
    public class Analysis : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 解析地址
        /// </summary>
        [Column("URL"),StringLength(200)]
        public virtual string Url { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("NAME"),StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"),StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
