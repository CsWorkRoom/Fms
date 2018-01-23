namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.FM_FILE_UPLOAD",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        USER_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        USER_NAME = c.String(),
                        UPLOAD_TIME = c.DateTime(nullable: false),
                        FILE_NAME = c.String(),
                        FILE_PATH = c.String(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("FMS_CL.FM_FILE_UPLOAD");
        }
    }
}
