using Abp.Domain.Entities;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Domain
{
    /// <summary>
    /// 解析地址
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "IMP_TYPE")]
    public class ImpType : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        public virtual ICollection<ImpTb> ImpTb { get; set; }
    }
}
