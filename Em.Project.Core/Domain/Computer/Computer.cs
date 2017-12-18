using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 终端管理
    /// </summary>
    [Table("FM_COMPUTER")]
    public class Computer : NotDeleteEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
       
        /// <summary>
        /// 终端类型ID
        /// </summary>
        [Column("COMPUTER_TYPE_ID")]
        public virtual long? ComputerTypeId { get; set; }
        [ForeignKey("ComputerTypeId")]
        public virtual ComputerType ComputerType { get; set; }

        /// <summary>
        /// 归属组织ID
        /// </summary>
        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }
        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }
      
        [Column("IP"), StringLength(20)]
        public virtual string Ip { get; set; }

        [Column("USER_NAME"), StringLength(50)]
        public virtual string UserName { get; set; }

        [Column("PWD"), StringLength(100)]
        public virtual string Pwd { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool? IsUse { get; set; }
        /// <summary>
        /// 终端说明
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }


        public virtual ICollection<ComputerFolder> ComputerShareFolder { get; set; }
    }
}
