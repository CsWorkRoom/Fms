namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("FMS_CL.FM_FILE_LIBRARY", "NAME", c => c.String(maxLength: 2000));
            AlterColumn("FMS_CL.FM_FILE_LIBRARY", "MD5", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("FMS_CL.FM_FILE_LIBRARY", "MD5", c => c.String(maxLength: 100));
            AlterColumn("FMS_CL.FM_FILE_LIBRARY", "NAME", c => c.String(maxLength: 100));
        }
    }
}
