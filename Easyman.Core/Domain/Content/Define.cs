using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;
using Easyman.Users;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容定义
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "DEFINE")]
    public class Define : CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        [Column("NAME"), StringLength(150)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 内容定义编码
        /// </summary>
        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }
        
       
    }
}
