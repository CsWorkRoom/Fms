using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Domain
{
    /// <summary>
    /// 客户经理
    /// </summary>
    [Table("GP_MANAGER")]
    public class Manager : Entity<long>
    {

        [Key, Column("ID")]
        public override long Id { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        [Column("MANAGER_NAME"), StringLength(50)]
        public virtual string ManagerName { get; set; }
        /// <summary>
        /// 客户经理编号（esop工号）
        /// </summary>
        [Column("MANAGER_NO"), StringLength(100)]
        public virtual string ManagerNo { get; set; }

        /// <summary>
        /// BOSS工号
        /// </summary>
        [Column("BOSS_NO"), StringLength(50)]
        public virtual string BossNo { get; set; }
        /// <summary>
        /// 组织-归属区县
        /// </summary>
        [Column("DISTRICT_ID")]
        public virtual long? DistrictId { get; set; }

        /// <summary>
        /// 代码（员工编号）
        /// </summary>
        [Column("CODE"), StringLength(50)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 序列（角色类型）
        /// </summary>
        [Column("ROLE_NAME"), StringLength(50)]
        public virtual string RoleName { get; set; }

        /// <summary>
        /// 员工类别
        /// </summary>
        [Column("PESON_TYPE"), StringLength(50)]
        public virtual string PesonType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("PHONE_NO"), StringLength(20)]
        public virtual string PhoneNo { get; set; }

        /// <summary>
        /// 区县名
        /// </summary>
        [Column("CITY_NAME"), StringLength(100)]
        public virtual string CityName { get; set; }

    }
}
