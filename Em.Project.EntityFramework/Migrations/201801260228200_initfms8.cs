namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("FMS_CL.FM_FILE_LIBRARY", "IS_HIDE", c => c.Decimal(precision: 1, scale: 0));
            AddColumn("FMS_CL.FM_MONIT_FILE", "IS_HIDE", c => c.Decimal(precision: 1, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("FMS_CL.FM_MONIT_FILE", "IS_HIDE");
            DropColumn("FMS_CL.FM_FILE_LIBRARY", "IS_HIDE");
        }
    }
}
