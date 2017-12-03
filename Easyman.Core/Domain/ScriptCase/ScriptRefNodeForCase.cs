using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Easyman.Domain
{
    /// <summary>
    /// 脚本流节点配置for实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix+ "SCRIPT_REF_NODE_FORCASE")]
    public class ScriptRefNodeForCase:Entity<long>
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
        /// 脚本流ID
        /// </summary>
        [Column("SCRIPT_CASE_ID")]
        public virtual long? ScriptCaseId { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        [Column("PARENT_NODE_ID")]
        public virtual long? ParentNodeId{ get; set; }
        /// <summary>
        /// 当前节点ID
        /// </summary>
        [Column("CURR_NODE_ID")]
        public virtual long? CurrNodeId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
