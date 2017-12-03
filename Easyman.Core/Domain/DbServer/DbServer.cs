using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "DB_SERVER")]
    public class DbServer: CommonEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 数据库别名
        /// </summary>
        [Column("BYNAME"), StringLength(50)]
        public virtual string ByName { get; set; }

        [Column("DB_TAG_ID")]
        public virtual long? DbTagId { get; set; }

        //[ForeignKey("DB_TAG_ID")]
        [ForeignKey("DbTagId")]
        public virtual DbTag DbTag { get; set; }

        /// <summary>
        /// 数据库种类
        /// </summary>
        [Column("DB_TYPE_ID")]
        public virtual long? DbTypeId { get; set; }
        [ForeignKey("DbTypeId")]
        public virtual DbType DbType { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Column("IP"), StringLength(20)]
        public virtual string Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [Column("PORT")]
        public virtual int? Port { get; set; }

        /// <summary>
        /// 数据库实例名
        /// </summary>
        [Column("DATA_CASE"), StringLength(20)]
        public virtual string DataCase { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column("USER"), StringLength(50)]
        public virtual string User { get; set; }

        /// <summary>
        /// 密码（加密）
        /// </summary>
        [Column("PASSWORD"), StringLength(100)]
        public virtual string Password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
