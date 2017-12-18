namespace Easyman.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class initfms3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.FM_ATTR",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ATTR_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
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
                    { "DynamicFilter_Attr_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_ATTR_TYPE", t => t.ATTR_TYPE_ID)
                .Index(t => t.ATTR_TYPE_ID);
            
            CreateTable(
                "FMS_CL.FM_ATTR_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
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
                    { "DynamicFilter_AttrType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.FM_CASE_VERSION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ScriptNodeCaseId = c.Decimal(precision: 19, scale: 0),
                        FolderVersionId = c.Decimal(precision: 19, scale: 0),
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
                    { "DynamicFilter_CaseVersion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.FM_COMPUTER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        COMPUTER_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        CODE = c.String(maxLength: 50),
                        IP = c.String(maxLength: 20),
                        USER_NAME = c.String(maxLength: 50),
                        PWD = c.String(maxLength: 100),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_COMPUTER_TYPE", t => t.COMPUTER_TYPE_ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .Index(t => t.COMPUTER_TYPE_ID)
                .Index(t => t.DISTRICT_ID);
            
            CreateTable(
                "FMS_CL.FM_FILE_ATTR",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FILE_LIBRARY_ID = c.Decimal(precision: 19, scale: 0),
                        ATTR_ID = c.Decimal(precision: 19, scale: 0),
                        ATTR_VAL = c.String(maxLength: 300),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_ATTR", t => t.ATTR_ID)
                .ForeignKey("FMS_CL.FM_FILE_LIBRARY", t => t.FILE_LIBRARY_ID)
                .Index(t => t.FILE_LIBRARY_ID)
                .Index(t => t.ATTR_ID);
            
            CreateTable(
                "FMS_CL.FM_FILE_LIBRARY",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FILE_FORMAT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 100),
                        MD5 = c.String(maxLength: 100),
                        SIZE = c.Double(),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_FILE_FORMAT", t => t.FILE_FORMAT_ID)
                .Index(t => t.FILE_FORMAT_ID);
            
            CreateTable(
                "FMS_CL.FM_FILE_FORMAT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        IS_FOLDER = c.Decimal(precision: 1, scale: 0),
                        NAME = c.String(maxLength: 50),
                        ICON = c.String(maxLength: 50),
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
                    { "DynamicFilter_FileFormat_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.FM_FILE_CLAIM",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONIT_FILE_ID = c.Decimal(precision: 19, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        USER_NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_MONIT_FILE", t => t.MONIT_FILE_ID)
                .Index(t => t.MONIT_FILE_ID);
            
            CreateTable(
                "FMS_CL.FM_MONIT_FILE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FOLDER_VERSION_ID = c.Decimal(precision: 19, scale: 0),
                        COMPUTER_ID = c.Decimal(precision: 19, scale: 0),
                        FOLDER_ID = c.Decimal(precision: 19, scale: 0),
                        RELY_MONIT_FILE_ID = c.Decimal(precision: 19, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 100),
                        FILE_FORMAT_ID = c.Decimal(precision: 19, scale: 0),
                        FILE_LIBRARY_ID = c.Decimal(precision: 19, scale: 0),
                        CLIENT_PATH = c.String(maxLength: 100),
                        SERVER_PATH = c.String(maxLength: 100),
                        MD5 = c.String(maxLength: 100),
                        STATUS = c.Decimal(precision: 5, scale: 0),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_FILE_FORMAT", t => t.FILE_FORMAT_ID)
                .ForeignKey("FMS_CL.FM_FOLDER", t => t.FOLDER_ID)
                .ForeignKey("FMS_CL.FM_FOLDER_VERSION", t => t.FOLDER_VERSION_ID)
                .ForeignKey("FMS_CL.FM_MONIT_FILE", t => t.PARENT_ID)
                .Index(t => t.FOLDER_VERSION_ID)
                .Index(t => t.FOLDER_ID)
                .Index(t => t.PARENT_ID)
                .Index(t => t.FILE_FORMAT_ID);
            
            CreateTable(
                "FMS_CL.FM_FOLDER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        COMPUTER_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 100),
                        POWER_MSG = c.String(maxLength: 100),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_COMPUTER", t => t.COMPUTER_ID)
                .Index(t => t.COMPUTER_ID);
            
            CreateTable(
                "FMS_CL.FM_FOLDER_VERSION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FOLDER_ID = c.Decimal(precision: 19, scale: 0),
                        BEGIN_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
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
                    { "DynamicFilter_FolderVersion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.FM_FOLDER", t => t.FOLDER_ID)
                .Index(t => t.FOLDER_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("FMS_CL.FM_FILE_CLAIM", "MONIT_FILE_ID", "FMS_CL.FM_MONIT_FILE");
            DropForeignKey("FMS_CL.FM_MONIT_FILE", "PARENT_ID", "FMS_CL.FM_MONIT_FILE");
            DropForeignKey("FMS_CL.FM_MONIT_FILE", "FOLDER_VERSION_ID", "FMS_CL.FM_FOLDER_VERSION");
            DropForeignKey("FMS_CL.FM_FOLDER_VERSION", "FOLDER_ID", "FMS_CL.FM_FOLDER");
            DropForeignKey("FMS_CL.FM_MONIT_FILE", "FOLDER_ID", "FMS_CL.FM_FOLDER");
            DropForeignKey("FMS_CL.FM_FOLDER", "COMPUTER_ID", "FMS_CL.FM_COMPUTER");
            DropForeignKey("FMS_CL.FM_MONIT_FILE", "FILE_FORMAT_ID", "FMS_CL.FM_FILE_FORMAT");
            DropForeignKey("FMS_CL.FM_FILE_ATTR", "FILE_LIBRARY_ID", "FMS_CL.FM_FILE_LIBRARY");
            DropForeignKey("FMS_CL.FM_FILE_LIBRARY", "FILE_FORMAT_ID", "FMS_CL.FM_FILE_FORMAT");
            DropForeignKey("FMS_CL.FM_FILE_ATTR", "ATTR_ID", "FMS_CL.FM_ATTR");
            DropForeignKey("FMS_CL.FM_COMPUTER", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.FM_COMPUTER", "COMPUTER_TYPE_ID", "FMS_CL.FM_COMPUTER_TYPE");
            DropForeignKey("FMS_CL.FM_ATTR", "ATTR_TYPE_ID", "FMS_CL.FM_ATTR_TYPE");
            DropIndex("FMS_CL.FM_FOLDER_VERSION", new[] { "FOLDER_ID" });
            DropIndex("FMS_CL.FM_FOLDER", new[] { "COMPUTER_ID" });
            DropIndex("FMS_CL.FM_MONIT_FILE", new[] { "FILE_FORMAT_ID" });
            DropIndex("FMS_CL.FM_MONIT_FILE", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.FM_MONIT_FILE", new[] { "FOLDER_ID" });
            DropIndex("FMS_CL.FM_MONIT_FILE", new[] { "FOLDER_VERSION_ID" });
            DropIndex("FMS_CL.FM_FILE_CLAIM", new[] { "MONIT_FILE_ID" });
            DropIndex("FMS_CL.FM_FILE_LIBRARY", new[] { "FILE_FORMAT_ID" });
            DropIndex("FMS_CL.FM_FILE_ATTR", new[] { "ATTR_ID" });
            DropIndex("FMS_CL.FM_FILE_ATTR", new[] { "FILE_LIBRARY_ID" });
            DropIndex("FMS_CL.FM_COMPUTER", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.FM_COMPUTER", new[] { "COMPUTER_TYPE_ID" });
            DropIndex("FMS_CL.FM_ATTR", new[] { "ATTR_TYPE_ID" });
            DropTable("FMS_CL.FM_FOLDER_VERSION",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FolderVersion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.FM_FOLDER");
            DropTable("FMS_CL.FM_MONIT_FILE");
            DropTable("FMS_CL.FM_FILE_CLAIM");
            DropTable("FMS_CL.FM_FILE_FORMAT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FileFormat_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.FM_FILE_LIBRARY");
            DropTable("FMS_CL.FM_FILE_ATTR");
            DropTable("FMS_CL.FM_COMPUTER");
            DropTable("FMS_CL.FM_CASE_VERSION",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CaseVersion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.FM_ATTR_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AttrType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.FM_ATTR",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Attr_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
