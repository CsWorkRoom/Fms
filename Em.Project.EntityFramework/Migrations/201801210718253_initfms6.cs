namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.FM_MONIT_LOG_VERSION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONIT_FILE_ID = c.Decimal(precision: 19, scale: 0),
                        LOG_TYPE = c.Decimal(precision: 5, scale: 0),
                        STATUS = c.Decimal(precision: 5, scale: 0),
                        BEGIN_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("FMS_CL.FM_MONIT_FILE", "COPY_STATUS_TIME", c => c.DateTime());
            AddColumn("FMS_CL.FM_MONIT_LOG", "MONIT_LOG_VERSION_ID", c => c.Decimal(precision: 19, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("FMS_CL.FM_MONIT_LOG", "MONIT_LOG_VERSION_ID");
            DropColumn("FMS_CL.FM_MONIT_FILE", "COPY_STATUS_TIME");
            DropTable("FMS_CL.FM_MONIT_LOG_VERSION");
        }
    }
}
