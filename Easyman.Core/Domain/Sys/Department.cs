using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    [Table("EM_DEPARTMENT")]
    public class Department : NotDeleteEntityHelper, IMayHaveTenant
    {
        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 租户
        /// </summary>
        [Column("TENANT_ID")]
        public virtual int? TenantId { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        [Column("PARENT_ID")]
        public virtual long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Department Parent { get; set; }
        /// <summary>
        /// 子级集合
        /// </summary>
        public virtual ICollection<Department> Children { get; set; }
        /// <summary>
        /// 部门分类
        /// </summary>
        [Column("OBJECT_TYPE"), StringLength(50)]
        public virtual string ObjectType { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>
        [Column("NAME"), StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 部门代码
        /// </summary>
        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Column("ICON"), StringLength(50)]
        public virtual string Icon { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        [Column("IS_USE")]
        public virtual bool? IsUse { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [Column("CUR_LEVEL")]
        public virtual int? CurLevel { get; set; }
        /// <summary>
        /// ID层级结构
        /// </summary>
        [Column("ID_PATH"), StringLength(200)]
        public virtual string IdPath { get; set; }
        /// <summary>
        /// NAME层级结构
        /// </summary>
        [Column("NAME_PATH"), StringLength(1000)]
        public virtual string NamePath { get; set; }
        /// <summary>
        /// 部门联系人
        /// </summary>
        [Column("LINK_MAN"), StringLength(20)]
        public virtual string LinkMan { get; set; }
        /// <summary>
        /// 部门联系人电话
        /// </summary>
        [Column("LINK_PHONE"), StringLength(20)]
        public virtual string LinkPhone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }
    }
}
