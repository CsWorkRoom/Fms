namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.FM_MONIT_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CASE_VERSION_ID = c.Decimal(precision: 19, scale: 0),
                        MONIT_FILE_ID = c.Decimal(precision: 19, scale: 0),
                        LOG_TYPE = c.Decimal(precision: 5, scale: 0),
                        LOG_MSG = c.String(),
                        LOG_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("FMS_CL.FM_CASE_VERSION", "BEGIN_TIME", c => c.DateTime());
            AddColumn("FMS_CL.FM_CASE_VERSION", "END_TIME", c => c.DateTime());
            AddColumn("FMS_CL.FM_FILE_LIBRARY", "IS_COPY", c => c.Decimal(precision: 1, scale: 0));
            AddColumn("FMS_CL.FM_MONIT_FILE", "CASE_VERSION_ID", c => c.Decimal(precision: 19, scale: 0));
            AddColumn("FMS_CL.FM_MONIT_FILE", "COPY_STATUS", c => c.Decimal(precision: 5, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("FMS_CL.FM_MONIT_FILE", "COPY_STATUS");
            DropColumn("FMS_CL.FM_MONIT_FILE", "CASE_VERSION_ID");
            DropColumn("FMS_CL.FM_FILE_LIBRARY", "IS_COPY");
            DropColumn("FMS_CL.FM_CASE_VERSION", "END_TIME");
            DropColumn("FMS_CL.FM_CASE_VERSION", "BEGIN_TIME");
            DropTable("FMS_CL.FM_MONIT_LOG");
        }
    }
}
