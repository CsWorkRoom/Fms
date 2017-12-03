using Abp.Domain.Entities;
using Easyman.Authorization.Roles;
using Easyman.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 模块
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "MODULE")]
    public class Module : CommonEntityHelper
    {

        [Key, Column("ID")]
        public override long Id { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Module Parent { get; set; }

        public virtual IEnumerable<Module> Children { get; set; }

        /// <summary>
        /// 层次编码
        /// </summary>
        [Column("PATH_ID"), StringLength(1024)]
        public virtual string PathId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Column("LEVEL")]
        public virtual int? Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("SHOW_ORDER")]
        public virtual int? ShowOrder { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Column("TYPE")]
        public virtual int? Type { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        [Column("NAME"),StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 应用类型(APP/PCWEB)
        /// </summary>
        [Column("APPLICATION_TYPE"), StringLength(50)]
        public virtual string ApplicationType { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column("URL"), StringLength(2000)]
        public virtual string Url { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Column("IDENTIFIER"), StringLength(20)]
        public virtual string Identifier { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Column("CODE"), StringLength(100)]
        public virtual string Code { get; set; }

        ///// <summary>
        ///// 调试
        ///// </summary>
        //[Column("IS_DEBUG")]
        //public virtual bool IsDebug { get; set; }

        ///// <summary>
        ///// 显示状态
        ///// </summary>
        //[Column("IS_HIDE")]
        //public virtual bool IsHide { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("DESCRIPTION"), StringLength(2000)]
        public virtual string Description { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Column("IMAGE_URL"), StringLength(2000)]
        public virtual string ImageUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(2000)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column("ICON"),StringLength(50)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        [Column("TENANT_ID")]
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool? IsUse { get; set; }

        public virtual ICollection<Module> ChildModule { get; set; }

        public virtual ICollection<RoleModule> RoleModule { get; set; }
    }
}
