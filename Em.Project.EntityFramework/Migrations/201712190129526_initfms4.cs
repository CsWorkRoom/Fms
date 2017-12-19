namespace Easyman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initfms4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "FMS_CL.FM_CASE_VERSION", name: "ScriptNodeCaseId", newName: "SCRIPT_NODE_CASE_ID");
            RenameColumn(table: "FMS_CL.FM_CASE_VERSION", name: "FolderVersionId", newName: "FOLDER_VERSION_ID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "FMS_CL.FM_CASE_VERSION", name: "FOLDER_VERSION_ID", newName: "FolderVersionId");
            RenameColumn(table: "FMS_CL.FM_CASE_VERSION", name: "SCRIPT_NODE_CASE_ID", newName: "ScriptNodeCaseId");
        }
    }
}
