using Abp.EntityFramework;
using Abp.Notifications;
using Abp.Zero.EntityFramework;
using Easyman.Authorization.Roles;
using Easyman.Domain;
using Easyman.MultiTenancy;
using Easyman.Users;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;

namespace Easyman.EntityFramework
{
    public class EmProjectDbContext: EasymanDbContext
    {
        #region 映射

        /*-----------------------------Report----------------------*/
        public virtual IDbSet<Param> Param { get; set; }
        public virtual IDbSet<Report> Report { get; set; }
        public virtual IDbSet<TbReport> TbReport { get; set; }
        public virtual IDbSet<TbReportField> TbReportField { get; set; }
        public virtual IDbSet<TbReportFieldTop> TbReportFieldTop { get; set; }
        public virtual IDbSet<TbReportOutEvent> TbReportOutEvent { get; set; }
        public virtual IDbSet<ReportFilter> ReportFilter { get; set; }
        public virtual IDbSet<GlobalVar> GlobalVar { get; set; }
        public virtual IDbSet<InEvent> InEvent { get; set; }
        public virtual IDbSet<RdlcReport> RdlcReport { get; set; }
        /*-----------------------------Export----------------------*/
        public virtual IDbSet<ExportConfig> ExportConfig { get; set; }
        public virtual IDbSet<ExportData> ExportData { get; set; }
        public virtual IDbSet<DownData> DownData { get; set; }

        /*-----------------------------Target----------------------*/
        public virtual IDbSet<TargetFormula> TargetFormula { get; set; }
        public virtual IDbSet<MonthTargetFormula> MonthTargetFormula { get; set; }

        public virtual IDbSet<MonthBill> MonthBill { get; set; }
        public virtual IDbSet<MonthBillLog> MonthBillLog { get; set; }
        public virtual IDbSet<MonthBonus> MonthBonus { get; set; }
        public virtual IDbSet<MonthTarget> MonthTarget { get; set; }
        public virtual IDbSet<MonthTargetDetail> MonthTargetDetail { get; set; }
        public virtual IDbSet<MonthBonusDetail> MonthBonusDetail { get; set; }

        public virtual IDbSet<Target> Target { get; set; }
        public virtual IDbSet<TargetTag> TargetTag { get; set; }
        public virtual IDbSet<TargetType> TargetType { get; set; }
        public virtual IDbSet<TargetValue> TargetValue { get; set; }

        public virtual IDbSet<Manager> Manager { get; set; }

        public virtual IDbSet<SubitemType> SubitemType { get; set; }
        public virtual IDbSet<Subitem> Subitem { get; set; }
        public virtual IDbSet<MonthSubitemScore> MonthSubitemScore { get; set; }


        #endregion

        #region 配置
        //TODO: Define an IDbSet for your Entities...

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public EmProjectDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in EasymanDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of EmProjectDbContext since ABP automatically handles it.
         */
        public EmProjectDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public EmProjectDbContext(DbConnection connection)
            : base(connection)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var schema = ConfigurationManager.AppSettings["Database.Schema"];
            Configuration.ProxyCreationEnabled = true;
            modelBuilder.HasDefaultSchema(schema);

            //关闭启动校验
            Database.SetInitializer<EmProjectDbContext>(null);

            #region 兼容Oracle命名

            modelBuilder.Entity<NotificationInfo>()
                .Property(x => x.EntityTypeAssemblyQualifiedName).HasColumnName("QualifiedName");
            modelBuilder.Entity<NotificationSubscriptionInfo>()
                .Property(x => x.EntityTypeAssemblyQualifiedName).HasColumnName("QualifiedName");
            modelBuilder.Entity<TenantNotificationInfo>()
                .Property(x => x.EntityTypeAssemblyQualifiedName).HasColumnName("QualifiedName");

            #endregion

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
