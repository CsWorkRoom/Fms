namespace Easyman.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class initfms2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.FM_COMPUTER_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(nullable: false, maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        DELETE_UID = c.Decimal(precision: 19, scale: 0),
                        DELETE_TIME = c.DateTime(),
                        IS_DELETE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ComputerType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("FMS_CL.FM_COMPUTER_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ComputerType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
