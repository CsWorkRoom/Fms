using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 脚本节点for实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_NODE_FORCASE")]
    public class ScriptNodeForCase : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        [Column("SCRIPT_NODE_ID")]
        public virtual long? ScriptNodeId { get; set; }
        [Column("SCRIPT_NODE_TYPE_ID")]
        public virtual long? ScriptNodeTypeId { get; set; }
        [Column("SCRIPT_CASE_ID")]
        public virtual long? ScriptCaseId { get; set; }
        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }

        [Column("DB_SERVER_ID")]
        public virtual long? DbServerId { get; set; }

        [Column("SCRIPT_MODEL")]
        public virtual short? ScriptModel { get; set; }

        [Column("CONTENT")]
        public virtual string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        #region 建表节点特有
        /// <summary>
        /// 表英文名
        /// </summary>
        [Column("E_TABLE_NAME"), StringLength(50)]
        public virtual string EnglishTabelName { get; set; }

        /// <summary>
        /// 表中文名
        /// </summary>
        [Column("C_TABLE_NAME"), StringLength(50)]
        public virtual string ChineseTabelName { get; set; }

        /// <summary>
        /// 表类型（公用表、私有表）
        /// </summary>
        [Column("TABLE_TYPE")]
        public virtual short? TableType { get; set; }

        /// <summary>
        /// 建表模式（复制、新建）
        /// </summary>
        [Column("TABLE_MODEL")]
        public virtual short? TableModel { get; set; }
        #endregion
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME")]
        public virtual DateTime? CreationTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATE_UID")]
        public virtual long? CreatorUserId { get; set; }
    }
}
