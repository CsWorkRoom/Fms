namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("FMS_CL.EM_SCRIPT", "IS_SUPERVENE", c => c.Decimal(precision: 5, scale: 0));
            AddColumn("FMS_CL.EM_SCRIPT_CASE", "IS_SUPERVENE", c => c.Decimal(precision: 5, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("FMS_CL.EM_SCRIPT_CASE", "IS_SUPERVENE");
            DropColumn("FMS_CL.EM_SCRIPT", "IS_SUPERVENE");
        }
    }
}
