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
    //[DbConfigurationType(typeof(MyDbConfiguration))]
    public class EmProjectDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        #region 框架映射

        /*-----------------------------Admin----------------------*/
        public virtual IDbSet<District> Districts { get; set; }
        public virtual IDbSet<Department> Departments { get; set; }
        public virtual IDbSet<Function> Functions { get; set; }
        public virtual IDbSet<FunctionRole> FunctionRoles { get; set; }

        /*-----------------------------Log----------------------*/
        public virtual IDbSet<UserPwdLog> UserPwdLog { get; set; }//用户密码修改记录


        /*-----------------------------Dictionary----------------------*/
        public virtual IDbSet<DictionaryType> DictionaryType { get; set; }
        public virtual IDbSet<Dictionary> Dictionary { get; set; }



        /*-----------------------------File----------------------*/


        /*-----------------------------Knowledge----------------------*/


        /*-----------------------------Module----------------------*/
        public virtual IDbSet<Module> Module { get; set; }



        public virtual IDbSet<RoleModule> RoleModule { get; set; }

        public virtual IDbSet<ModuleEvent> ModuleEvent { get; set; }

        public virtual IDbSet<Analysis> Analysis { get; set; }

        public virtual IDbSet<RoleModuleEvent> RoleModuleEvent { get; set; }


        /*-----------------------------DbServer----------------------*/
        public virtual IDbSet<DbTag> DbTag { get; set; }
        public virtual IDbSet<DbServer> DbServer { get; set; }
        /*-----------------------------Script----------------------*/
        public virtual IDbSet<ConnectLine> ConnectLine { get; set; }
        public virtual IDbSet<NodePosition> NodePosition { get; set; }
        public virtual IDbSet<Script> Script { get; set; }
        public virtual IDbSet<ScriptNode> ScriptNode { get; set; }
        public virtual IDbSet<ScriptNodeType> ScriptNodeType { get; set; }
        public virtual IDbSet<ScriptRefNode> ScriptRefNode { get; set; }
        public virtual IDbSet<ScriptType> ScriptType { get; set; }

        public virtual IDbSet<ScriptNodeLog> ScriptNodeLog { get; set; }
        public virtual IDbSet<ScriptFunction> ScriptFunction { get; set; }
        /*-----------------------------ScriptCase----------------------*/
        public virtual IDbSet<ConnectLineForCase> ConnectLineForCase { get; set; }
        public virtual IDbSet<HandRecord> HandRecord { get; set; }
        public virtual IDbSet<NodePositionForCase> NodePositionForCase { get; set; }
        public virtual IDbSet<ScriptCase> ScriptCase { get; set; }
        public virtual IDbSet<ScriptCaseLog> ScriptCaseLog { get; set; }
        public virtual IDbSet<ScriptNodeCase> ScriptNodeCase { get; set; }
        public virtual IDbSet<ScriptNodeCaseLog> ScriptNodeCaseLog { get; set; }
        public virtual IDbSet<ScriptNodeForCase> ScriptNodeForCase { get; set; }
        public virtual IDbSet<ScriptRefNodeForCase> ScriptRefNodeForCase { get; set; }

        /*-----------------------------Icon----------------------*/
        public virtual IDbSet<Icon> Icon { get; set; }
        public virtual IDbSet<IconType> IconType { get; set; }

        /*-----------------------------Content----------------------*/
        public virtual IDbSet<Content> Content { get; set; }
        public virtual IDbSet<ContentCheck> ContentCheck { get; set; }
        public virtual IDbSet<ContentDistrict> ContentDistrict { get; set; }
        public virtual IDbSet<ContentLog> ContentLog { get; set; }
        public virtual IDbSet<ContentPraiseLog> ContentPraiseLog { get; set; }
        public virtual IDbSet<ContentPushWay> ContentPushWay { get; set; }
        public virtual IDbSet<ContentReadLog> ContentReadLog { get; set; }
        public virtual IDbSet<ContentRefTag> ContentRefTag { get; set; }
        public virtual IDbSet<ContentReply> ContentReply { get; set; }
        public virtual IDbSet<ContentRole> ContentRole { get; set; }
        public virtual IDbSet<ContentTag> ContentTag { get; set; }
        public virtual IDbSet<ContentType> ContentType { get; set; }
        public virtual IDbSet<ContentUser> ContentUser { get; set; }
        public virtual IDbSet<Define> Define { get; set; }
        public virtual IDbSet<DefineConfig> DefineConfig { get; set; }
        public virtual IDbSet<PushWay> PushWay { get; set; }
        public virtual IDbSet<ReplyPraiseLog> ReplyPraiseLog { get; set; }
        public virtual IDbSet<ContentFile> ContentFile { get; set; }
        public virtual IDbSet<ContentReplyFile> ContentReplyFile { get; set; }

        /*----------------------------Import----------------------------*/
        public virtual IDbSet<DbType> DbType { get; set; }
        public virtual IDbSet<PreDataType> PreDataType { get; set; }
        public virtual IDbSet<DefaultField> DefaultField { get; set; }
        public virtual IDbSet<Files> Files { get; set; }
        public virtual IDbSet<ImportLog> ImportLog { get; set; }
        public virtual IDbSet<ImpTb> ImpTb { get; set; }
        public virtual IDbSet<ImpTbField> ImpTbField { get; set; }
        public virtual IDbSet<ImpTbCase> ImpTbCase { get; set; }
        public virtual IDbSet<OfflineLog> OfflineLog { get; set; }
        public virtual IDbSet<Regulars> Regulars { get; set; }
        public virtual IDbSet<ImpType> ImpType { get; set; }

        /*----------------------------APP----------------------------*/
        public virtual IDbSet<AppVersion> AppVersion { get; set; }

        #endregion

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

        public virtual IDbSet<ChartReport> ChartReport { get; set; }
        public virtual IDbSet<ChartType> ChartType { get; set; }
        public virtual IDbSet<ChartTemp> ChartTemp { get; set; }

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
            : base(connection, true)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var schema = ConfigurationManager.AppSettings["Database.Schema"];
            Configuration.ProxyCreationEnabled = true;
            modelBuilder.HasDefaultSchema(schema);

            //关闭启动校验
            //Database.SetInitializer<EmProjectDbContext>(null);

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
