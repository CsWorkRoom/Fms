using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Easyman.Common;

namespace Easyman.Domain
{
    /// <summary>
    /// 节点位置管理FOR实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "NODE_POSITION_FORCASE")]
    public class NodePositionForCase : Entity<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 脚本流ID
        /// </summary>
        [Column("SCRIPT_ID")]
        public virtual long? ScriptId { get; set; }
        /// <summary>
        /// 脚本流实例ID
        /// </summary>
        [Column("SCRIPT_CASE_ID")]
        public virtual long? ScriptCaseId { get; set; }
        /// <summary>
        /// 脚本流实例
        /// </summary>
        [ForeignKey("ScriptCaseId")]
        public virtual ScriptCase ScriptCase { get; set; }
        /// <summary>
        /// 脚本节点ID
        /// </summary>
        [Column("SCRIPT_NODE_ID")]
        public virtual long? ScriptNodeId { get; set; }
        /// <summary>
        /// 节点位置X
        /// </summary>
        [Column("X")]
        public virtual int? X { get; set; }
        /// <summary>
        /// 节点位置Y
        /// </summary>
        [Column("Y")]
        public virtual int? Y{ get; set; }
        /// <summary>
        /// 节点的div的id
        /// </summary>
        [Column("DIV_ID"), StringLength(100)]
        public virtual string DivId { get; set; }
    }
}
