namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("FMS_CL.FM_MONIT_FILE", "CLIENT_PATH", c => c.String(maxLength: 2000));
            AlterColumn("FMS_CL.FM_MONIT_FILE", "SERVER_PATH", c => c.String(maxLength: 2000));
            AlterColumn("FMS_CL.FM_MONIT_FILE", "MD5", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("FMS_CL.FM_MONIT_FILE", "MD5", c => c.String(maxLength: 100));
            AlterColumn("FMS_CL.FM_MONIT_FILE", "SERVER_PATH", c => c.String(maxLength: 100));
            AlterColumn("FMS_CL.FM_MONIT_FILE", "CLIENT_PATH", c => c.String(maxLength: 100));
        }
    }
}
