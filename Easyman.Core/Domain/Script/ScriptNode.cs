using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{

    /// <summary>
    /// 数据库标识
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "SCRIPT_NODE")]
    public class ScriptNode : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("SCRIPT_NODE_TYPE_ID")]
        public virtual long? ScriptNodeTypeId { get; set; }

        [ForeignKey("ScriptNodeTypeId")]
        public virtual ScriptNodeType ScriptNodeType { get; set; }

        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }

        [Column("DB_SERVER_ID")]
        public virtual long? DbServerId { get; set; }

        [ForeignKey("DbServerId")]
        public virtual DbServer DbServer { get; set; }

        [Column("SCRIPT_MODEL")]
        public virtual short? ScriptModel { get; set; }

        [Column("CONTENT")]
        public virtual string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 任务组配置页面需要字段
        /// 配置任务组需要添加任务
        /// 二〇一七年六月十一日 陈才添加
        /// </summary>
        [Column("TASK_SPECIFIC"),StringLength(200)]
        public virtual string TaskSpecific { get; set; }

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
    }
}
