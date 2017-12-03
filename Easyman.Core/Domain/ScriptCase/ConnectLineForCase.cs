using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Easyman.Common;
using Abp.Domain.Entities;

namespace Easyman.Domain
{
    /// <summary>
    /// 连接线管理-实例
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONNECT_LINE_FORCASE")]
    public class ConnectLineForCase : Entity<long>
    {
        //[Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        /// 起点DIV的(html)ID
        /// </summary>
        [Column("FROM_DIV_ID"),StringLength(100)]
        public virtual string FromDivId {get;set;}
        /// <summary>
        /// 起点位置(插件需要知道从DIV的哪个位置开始画线)
        /// </summary>
        [Column("FROM_POINT_ANCHORS"),StringLength(50)]
        public virtual string FromPointAnchors { get; set; }
        /// <summary>
        /// 结束点DIV的(html)ID
        /// </summary>
        [Column("TO_DIV_ID"),StringLength(100)]
        public virtual string ToDivId { get; set; }
        /// <summary>
        /// 结束点  插件位置
        /// </summary>
        [Column("TO_POINT_ANCHORS"),StringLength(50)]
        public virtual string ToPintAnchors{ get; set; }
        /// <summary>
        /// 连接线说明
        /// </summary>
        [Column("CONTENT"),StringLength(200)]
        public virtual string Content { get; set; }

    }
}
