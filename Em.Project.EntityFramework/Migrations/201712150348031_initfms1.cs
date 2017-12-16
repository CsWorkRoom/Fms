namespace Easyman.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class initfms1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FMS_CL.EM_ANALYSIS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        URL = c.String(maxLength: 200),
                        NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_APP_VERSION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FILE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        VERSION_CODE = c.Decimal(nullable: false, precision: 19, scale: 0),
                        VERSION_NAME = c.String(),
                        TYPE = c.String(maxLength: 20),
                        IS_NEW = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IS_MUST = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UPGRADE_LOG = c.String(maxLength: 1000),
                        UPDATE_TIME = c.DateTime(nullable: false),
                        UPDATE_URL = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_FILES", t => t.FILE_ID, cascadeDelete: true)
                .Index(t => t.FILE_ID);
            
            CreateTable(
                "FMS_CL.EM_FILES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        TRUE_NAME = c.String(maxLength: 50),
                        PATH = c.String(maxLength: 200),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        LENGTH = c.Decimal(nullable: false, precision: 19, scale: 0),
                        UPLOAD_TIME = c.DateTime(nullable: false),
                        REMARK = c.String(maxLength: 200),
                        URL = c.String(maxLength: 254),
                        FILE_TYPE = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_IMPORT_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 100),
                        CODE = c.String(maxLength: 100),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IMP_TB_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IMP_TB_CASE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CASE_TABLE_NAME = c.String(maxLength: 50),
                        DURATION = c.Decimal(nullable: false, precision: 19, scale: 0),
                        FILE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IMP_MODE = c.String(maxLength: 50),
                        FILE_NAME = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_FILES", t => t.FILE_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_IMP_TB", t => t.IMP_TB_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_IMP_TB_CASE", t => t.IMP_TB_CASE_ID, cascadeDelete: true)
                .Index(t => t.IMP_TB_ID)
                .Index(t => t.IMP_TB_CASE_ID)
                .Index(t => t.FILE_ID);
            
            CreateTable(
                "FMS_CL.EM_IMP_TB",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CODE = c.String(maxLength: 100),
                        CN_TABLE_NAME = c.String(maxLength: 100),
                        EN_TABLE_NAME = c.String(maxLength: 100),
                        RULE = c.String(maxLength: 50),
                        SQL = c.String(),
                        DB_SERVER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IMP_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_SERVER", t => t.DB_SERVER_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_IMP_TYPE", t => t.IMP_TYPE_ID, cascadeDelete: true)
                .Index(t => t.DB_SERVER_ID)
                .Index(t => t.IMP_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_DB_SERVER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        BYNAME = c.String(maxLength: 50),
                        DB_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        DB_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        IP = c.String(maxLength: 20),
                        PORT = c.Decimal(precision: 10, scale: 0),
                        DATA_CASE = c.String(maxLength: 20),
                        USER = c.String(maxLength: 50),
                        PASSWORD = c.String(maxLength: 100),
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
                    { "DynamicFilter_DbServer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_TAG", t => t.DB_TAG_ID)
                .ForeignKey("FMS_CL.EM_DB_TYPE", t => t.DB_TYPE_ID)
                .Index(t => t.DB_TAG_ID)
                .Index(t => t.DB_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_DB_TAG",
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
                    { "DynamicFilter_DbTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_DB_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_PRE_DATA_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        DATA_TYPE = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        DB_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_TYPE", t => t.DB_TYPE_ID, cascadeDelete: true)
                .Index(t => t.DB_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_DEFAULT_FIELD",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FIELD_CODE = c.String(maxLength: 64),
                        FIELD_NAME = c.String(maxLength: 64),
                        DATA_TYPE = c.String(maxLength: 50),
                        DEFAULT_VALUE = c.String(maxLength: 100),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        DB_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_IMP_TB_CASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CASE_TABLE_NAME = c.String(maxLength: 50),
                        IMP_TB_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_IMP_TB", t => t.IMP_TB_ID, cascadeDelete: true)
                .Index(t => t.IMP_TB_ID);
            
            CreateTable(
                "FMS_CL.EM_IMP_TB_FIELD",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FIELD_CODE = c.String(maxLength: 64),
                        FIELD_NAME = c.String(maxLength: 64),
                        DATA_TYPE = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        IMP_TB_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        REGULAR_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_IMP_TB", t => t.IMP_TB_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_REGULAR", t => t.REGULAR_ID, cascadeDelete: true)
                .Index(t => t.IMP_TB_ID)
                .Index(t => t.REGULAR_ID);
            
            CreateTable(
                "FMS_CL.EM_REGULAR",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        REGULAR = c.String(maxLength: 100),
                        ERROR_MSG = c.String(maxLength: 100),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_IMP_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_OFFLINE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        BEGIN_TIME = c.DateTime(nullable: false),
                        END_TIME = c.DateTime(nullable: false),
                        STATUS = c.String(maxLength: 50),
                        RESULT = c.String(maxLength: 50),
                        IMPORT_LOG_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_IMPORT_LOG", t => t.IMPORT_LOG_ID, cascadeDelete: true)
                .Index(t => t.IMPORT_LOG_ID);
            
            CreateTable(
                "FMS_CL.ABP_AUDITLOGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        SERVICE_NAME = c.String(maxLength: 256),
                        METHOD_NAME = c.String(maxLength: 256),
                        PARAMETERS = c.String(maxLength: 1024),
                        EXECUTION_TIME = c.DateTime(nullable: false),
                        EXECUTION_DURATION = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CLIENT_IP_ADDRESS = c.String(maxLength: 64),
                        CLIENT_NAME = c.String(maxLength: 128),
                        BROWSER_INFO = c.String(maxLength: 256),
                        EXCEPTION = c.String(maxLength: 2000),
                        IMPERSONATOR_USER_ID = c.Decimal(precision: 19, scale: 0),
                        IMPERSONATOR_TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        CUSTOM_DATA = c.String(maxLength: 2000),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AuditLog_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.AbpBackgroundJobs",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        JobType = c.String(nullable: false, maxLength: 512),
                        JobArgs = c.String(nullable: false),
                        TryCount = c.Decimal(nullable: false, precision: 5, scale: 0),
                        NextTryTime = c.DateTime(nullable: false),
                        LastTryTime = c.DateTime(),
                        IsAbandoned = c.Decimal(nullable: false, precision: 1, scale: 0),
                        Priority = c.Decimal(nullable: false, precision: 3, scale: 0),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.IsAbandoned, t.NextTryTime });
            
            CreateTable(
                "FMS_CL.EM_CHART_REPORT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        APPLICATION_TYPE = c.String(maxLength: 50),
                        IS_OPEN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_SHOW_FILTER = c.Decimal(precision: 1, scale: 0),
                        MAKE_WAY = c.Decimal(precision: 5, scale: 0),
                        CHART_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        CHART_TEMP_ID = c.Decimal(precision: 19, scale: 0),
                        END_CODE = c.String(),
                        REMARK = c.String(),
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
                    { "DynamicFilter_ChartReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CHART_TYPE", t => t.CHART_TYPE_ID)
                .ForeignKey("FMS_CL.EM_REPORT", t => t.REPORT_ID)
                .Index(t => t.REPORT_ID)
                .Index(t => t.CHART_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_CHART_TYPE",
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
                    { "DynamicFilter_ChartType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_REPORT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        CODE = c.String(maxLength: 50),
                        SQL = c.String(),
                        REMARK = c.String(maxLength: 200),
                        FIELD_JSON = c.String(),
                        IS_PLACEHOLDER = c.Decimal(precision: 1, scale: 0),
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
                    { "DynamicFilter_Report_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_SERVER", t => t.DB_SERVER_ID)
                .Index(t => t.DB_SERVER_ID);
            
            CreateTable(
                "FMS_CL.EM_CHART_TEMP",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CHART_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        TEMP_TYPE = c.Decimal(precision: 5, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 50),
                        TEMP_CODE = c.String(),
                        REMARK = c.String(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CHART_TYPE", t => t.CHART_TYPE_ID)
                .Index(t => t.CHART_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_CONNECT_LINE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FROM_DIV_ID = c.String(maxLength: 100),
                        FROM_POINT_ANCHORS = c.String(maxLength: 50),
                        TO_DIV_ID = c.String(maxLength: 100),
                        TO_POINT_ANCHORS = c.String(maxLength: 50),
                        CONTENT = c.String(maxLength: 200),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_CONNECT_LINE_FORCASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        FROM_DIV_ID = c.String(maxLength: 100),
                        FROM_POINT_ANCHORS = c.String(maxLength: 50),
                        TO_DIV_ID = c.String(maxLength: 100),
                        TO_POINT_ANCHORS = c.String(maxLength: 50),
                        CONTENT = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DEFINE_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        DEFINE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        TITLE = c.String(maxLength: 150),
                        SUMMARY = c.String(maxLength: 500),
                        INFO = c.String(),
                        IMAGE = c.String(),
                        IS_IMPORT = c.Decimal(precision: 1, scale: 0),
                        BEGIN_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        IS_URGENT = c.Decimal(precision: 1, scale: 0),
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
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DEFINE", t => t.DEFINE_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_CONTENT_TYPE", t => t.DEFINE_TYPE_ID, cascadeDelete: true)
                .Index(t => t.DEFINE_TYPE_ID)
                .Index(t => t.DEFINE_ID);
            
            CreateTable(
                "FMS_CL.EM_DEFINE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 150),
                        CODE = c.String(maxLength: 50),
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
                    { "DynamicFilter_Define_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_TYPE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DEFINE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        NAME = c.String(maxLength: 150),
                        PARENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        PATH_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        LEVEL = c.Decimal(nullable: false, precision: 19, scale: 0),
                        SHOW_ORDER = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        DELETE_UID = c.Decimal(precision: 19, scale: 0),
                        DELETE_TIME = c.DateTime(),
                        IS_DELETE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                        ContentType_Id = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContentType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT_TYPE", t => t.ContentType_Id)
                .ForeignKey("FMS_CL.EM_DEFINE", t => t.DEFINE_ID, cascadeDelete: true)
                .Index(t => t.DEFINE_ID)
                .Index(t => t.ContentType_Id);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_CHECK",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IS_CHECK_USER = c.Decimal(precision: 1, scale: 0),
                        IS_CHECK_ROLE = c.Decimal(precision: 1, scale: 0),
                        IS_CHECK_DISTRICT = c.Decimal(precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_DISTRICT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DISTRICT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IS_ALLOW = c.Decimal(precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID, cascadeDelete: true)
                .Index(t => t.DISTRICT_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_DISTRICT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        OBJECT_TYPE = c.String(maxLength: 50),
                        NAME = c.String(maxLength: 200),
                        CODE = c.String(maxLength: 50),
                        ICON = c.String(maxLength: 50),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        CUR_LEVEL = c.Decimal(precision: 10, scale: 0),
                        ID_PATH = c.String(maxLength: 200),
                        NAME_PATH = c.String(maxLength: 1000),
                        LINK_MAN = c.String(maxLength: 20),
                        LINK_PHONE = c.String(maxLength: 20),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_District_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.PARENT_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_FILE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FILE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_FILES", t => t.FILE_ID, cascadeDelete: true)
                .Index(t => t.FILE_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DEFINE_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        TITLE = c.String(maxLength: 150),
                        SUMMARY = c.String(),
                        INFO = c.String(),
                        IMAGE = c.String(),
                        IS_IMPORT = c.Decimal(precision: 1, scale: 0),
                        BEGIN_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        IS_URGENT = c.Decimal(precision: 1, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT_TYPE", t => t.DEFINE_TYPE_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.CREATE_UID, cascadeDelete: true)
                .Index(t => t.DEFINE_TYPE_ID)
                .Index(t => t.CREATE_UID);
            
            CreateTable(
                "FMS_CL.ABP_USERS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        PHONE_NO = c.String(maxLength: 100),
                        DEPARTMENT_ID = c.Decimal(precision: 19, scale: 0),
                        MODIFY_PWD = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PROJECT_FLY = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LOGIN_FAIL_COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        LOCKED_REASON = c.String(),
                        AUTHENTICATION_SOURCE = c.String(maxLength: 64),
                        NAME = c.String(nullable: false, maxLength: 32),
                        SURNAME = c.String(nullable: false, maxLength: 32),
                        PASSWORD = c.String(nullable: false, maxLength: 128),
                        IS_EMAIL_CONFIRMED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        EMAIL_CONFIRMATION_CODE = c.String(maxLength: 328),
                        PASSWORD_RESET_CODE = c.String(maxLength: 328),
                        LOCKOUT_END_DATE_UTC = c.DateTime(),
                        ACCESS_FAILED_COUNT = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IS_LOCKOUT_ENABLED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PHONE_NUMBER = c.String(),
                        IS_PHONE_NUMBER_CONFIRMED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SECURITY_STAMP = c.String(),
                        IS_TWO_FACTOR_ENABLED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_ACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        USER_NAME = c.String(nullable: false, maxLength: 32),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        EMAIL_ADDRESS = c.String(nullable: false, maxLength: 256),
                        LAST_LOGIN_TIME = c.DateTime(),
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
                    { "DynamicFilter_User_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_User_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.CREATE_UID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.DELETE_UID)
                .ForeignKey("FMS_CL.EM_DEPARTMENT", t => t.DEPARTMENT_ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.UPDATE_UID)
                .Index(t => t.DISTRICT_ID)
                .Index(t => t.DEPARTMENT_ID)
                .Index(t => t.CREATE_UID)
                .Index(t => t.DELETE_UID)
                .Index(t => t.UPDATE_UID);
            
            CreateTable(
                "FMS_CL.ABP_USER_CLAIMS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CLAIM_TYPE = c.String(),
                        CLAIM_VALUE = c.String(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.EM_DEPARTMENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        OBJECT_TYPE = c.String(maxLength: 50),
                        NAME = c.String(maxLength: 200),
                        CODE = c.String(maxLength: 50),
                        ICON = c.String(maxLength: 50),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
                        CUR_LEVEL = c.Decimal(precision: 10, scale: 0),
                        ID_PATH = c.String(maxLength: 200),
                        NAME_PATH = c.String(maxLength: 1000),
                        LINK_MAN = c.String(maxLength: 20),
                        LINK_PHONE = c.String(maxLength: 20),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Department_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DEPARTMENT", t => t.PARENT_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.ABP_USER_LOGINS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        LOGIN_PROVIDER = c.String(nullable: false, maxLength: 128),
                        PROVIDER_KEY = c.String(nullable: false, maxLength: 256),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserLogin_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.ABP_PERMISSIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 128),
                        IS_GRANTED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        ROLE_ID = c.Decimal(precision: 10, scale: 0),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_RolePermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserPermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.ROLE_ID, cascadeDelete: true)
                .Index(t => t.USER_ID)
                .Index(t => t.ROLE_ID);
            
            CreateTable(
                "FMS_CL.ABP_USER_ROLES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        ROLE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserRole_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.ABP_SETTINGS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 256),
                        VALUE = c.String(maxLength: 2000),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Setting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_PRAISE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.CONTENT_ID)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_PUSH_WAY",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        PUSH_WAY_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_PUSH_WAY", t => t.PUSH_WAY_ID, cascadeDelete: true)
                .Index(t => t.PUSH_WAY_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_PUSH_WAY",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 200),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_READ_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.CONTENT_ID)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_REF_TAG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TAG_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_CONTENT_TAG", t => t.TAG_ID, cascadeDelete: true)
                .Index(t => t.TAG_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_TAG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TYPE = c.String(maxLength: 50),
                        INFO = c.String(maxLength: 260),
                        REMARK = c.String(),
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
                    { "DynamicFilter_ContentTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_REPLY",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        INFO = c.String(maxLength: 500),
                        REPLY_UID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        IP_ADDR = c.String(maxLength: 50),
                        IPROMISE = c.String(maxLength: 50),
                        IS_DELETE = c.Decimal(precision: 1, scale: 0),
                        DELETE_TIME = c.DateTime(nullable: false),
                        DELETE_UID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        DELETE_REASON = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT_REPLY", t => t.PARENT_ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.DELETE_UID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.REPLY_UID, cascadeDelete: true)
                .Index(t => t.CONTENT_ID)
                .Index(t => t.PARENT_ID)
                .Index(t => t.REPLY_UID)
                .Index(t => t.DELETE_UID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_REPLY_FILE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FILE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_REPLY_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT_REPLY", t => t.CONTENT_REPLY_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_FILES", t => t.FILE_ID, cascadeDelete: true)
                .Index(t => t.FILE_ID)
                .Index(t => t.CONTENT_REPLY_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_ROLE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ROLE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IS_ALLOW = c.Decimal(precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.ROLE_ID, cascadeDelete: true)
                .Index(t => t.ROLE_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.ABP_ROLES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PARENT_ID = c.Decimal(precision: 10, scale: 0),
                        DISPLAY_NAME = c.String(nullable: false, maxLength: 64),
                        IS_STATIC = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_DEFAULT = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 32),
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
                    { "DynamicFilter_Role_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Role_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.PARENT_ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.CREATE_UID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.DELETE_UID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.UPDATE_UID)
                .Index(t => t.PARENT_ID)
                .Index(t => t.CREATE_UID)
                .Index(t => t.DELETE_UID)
                .Index(t => t.UPDATE_UID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_USER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CONTENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        IS_ALLOW = c.Decimal(precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT", t => t.CONTENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.USER_ID)
                .Index(t => t.CONTENT_ID);
            
            CreateTable(
                "FMS_CL.EM_DEFINE_CONFIG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DEFINE_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 150),
                        IS_CONTENT_FILE = c.Decimal(precision: 1, scale: 0),
                        IS_REPLY = c.Decimal(precision: 1, scale: 0),
                        IS_REPLY_FILE = c.Decimal(precision: 1, scale: 0),
                        IS_REPLY_FLOOR = c.Decimal(precision: 1, scale: 0),
                        IS_REPLY_FLOOR_FILE = c.Decimal(precision: 1, scale: 0),
                        IS_TEXT = c.Decimal(precision: 1, scale: 0),
                        IS_LIKE = c.Decimal(precision: 1, scale: 0),
                        IS_DELETE = c.Decimal(precision: 1, scale: 0),
                        IS_SHARE = c.Decimal(precision: 1, scale: 0),
                        IS_CHECK_USER = c.Decimal(precision: 1, scale: 0),
                        IS_CHECK_ROLE = c.Decimal(precision: 1, scale: 0),
                        IS_CHECK_DISTRICT = c.Decimal(precision: 1, scale: 0),
                        CREATE_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DEFINE", t => t.DEFINE_ID)
                .Index(t => t.DEFINE_ID);
            
            CreateTable(
                "FMS_CL.EM_DICTIONARY",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        DICTIONARY_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DICTIONARY_TYPE", t => t.DICTIONARY_TYPE_ID)
                .ForeignKey("FMS_CL.EM_DICTIONARY", t => t.PARENT_ID)
                .Index(t => t.DICTIONARY_TYPE_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.EM_DICTIONARY_TYPE",
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
                    { "DynamicFilter_DictionaryType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_DOWN_DATA",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        EXPORT_DATA_ID = c.Decimal(precision: 19, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        DOWN_BEGIN_TIME = c.DateTime(),
                        DOWN_END_TIME = c.DateTime(),
                        STATUS = c.String(maxLength: 50),
                        DISPLAY_NAME = c.String(maxLength: 50),
                        FILE_NAME = c.String(maxLength: 50),
                        FILE_PATH = c.String(maxLength: 200),
                        FILE_SIZE = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_EXPORT_DATA", t => t.EXPORT_DATA_ID)
                .Index(t => t.EXPORT_DATA_ID);
            
            CreateTable(
                "FMS_CL.EM_EXPORT_DATA",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MODULE_ID = c.Decimal(precision: 19, scale: 0),
                        REPORT_CODE = c.String(maxLength: 50),
                        FROM_URL = c.String(maxLength: 200),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        EXPORT_WAY = c.String(maxLength: 10),
                        DISPLAY_NAME = c.String(maxLength: 50),
                        SQL = c.String(),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        TOP_FIELDS = c.String(),
                        FILE_NAME = c.String(maxLength: 50),
                        FILE_PATH = c.String(maxLength: 200),
                        STATUS = c.String(maxLength: 50),
                        FILE_SIZE = c.Decimal(precision: 10, scale: 0),
                        FILE_FORMAT = c.String(maxLength: 20),
                        FILES_ID = c.Decimal(precision: 19, scale: 0),
                        BEGIN_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        VALID_DAY = c.Decimal(precision: 10, scale: 0),
                        IS_INVALID = c.Decimal(precision: 1, scale: 0),
                        IS_CLOSE = c.Decimal(precision: 1, scale: 0),
                        CLOSER = c.Decimal(precision: 19, scale: 0),
                        CLOSE_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_MODULE", t => t.MODULE_ID)
                .Index(t => t.MODULE_ID);
            
            CreateTable(
                "FMS_CL.EM_MODULE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        PATH_ID = c.String(maxLength: 1024),
                        LEVEL = c.Decimal(precision: 10, scale: 0),
                        SHOW_ORDER = c.Decimal(precision: 10, scale: 0),
                        TYPE = c.Decimal(precision: 10, scale: 0),
                        NAME = c.String(maxLength: 100),
                        APPLICATION_TYPE = c.String(maxLength: 50),
                        URL = c.String(maxLength: 2000),
                        IDENTIFIER = c.String(maxLength: 20),
                        CODE = c.String(maxLength: 100),
                        DESCRIPTION = c.String(maxLength: 2000),
                        IMAGE_URL = c.String(maxLength: 2000),
                        REMARK = c.String(maxLength: 2000),
                        ICON = c.String(maxLength: 50),
                        TENANT_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IS_USE = c.Decimal(precision: 1, scale: 0),
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
                    { "DynamicFilter_Module_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_MODULE", t => t.PARENT_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.EM_ROLE_MODULE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ROLE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        MODULE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_MODULE", t => t.MODULE_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.ROLE_ID, cascadeDelete: true)
                .Index(t => t.ROLE_ID)
                .Index(t => t.MODULE_ID);
            
            CreateTable(
                "FMS_CL.ABP_FEATURES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(nullable: false, maxLength: 128),
                        VALUE = c.String(nullable: false, maxLength: 2000),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        EDITION_ID = c.Decimal(precision: 10, scale: 0),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantFeatureSetting_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_EDITIONS", t => t.EDITION_ID, cascadeDelete: true)
                .Index(t => t.EDITION_ID);
            
            CreateTable(
                "FMS_CL.ABP_EDITIONS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NAME = c.String(nullable: false, maxLength: 32),
                        DISPLAY_NAME = c.String(nullable: false, maxLength: 64),
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
                    { "DynamicFilter_Edition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_EXPORT_CONFIG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        APP = c.String(maxLength: 50),
                        PATH = c.String(maxLength: 200),
                        VALID_DAY = c.Decimal(precision: 10, scale: 0),
                        DATA_SIZE = c.Decimal(precision: 10, scale: 0),
                        MAX_TIME = c.Decimal(precision: 10, scale: 0),
                        MAX_ROW_NUM = c.Decimal(precision: 10, scale: 0),
                        WAIT_TIME = c.Decimal(precision: 10, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_FUNCTION_ROLE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        FUNCTION_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ROLE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CREATION_TIME = c.DateTime(nullable: false),
                        CREATOR_USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_FUNCTION", t => t.FUNCTION_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.ROLE_ID, cascadeDelete: true)
                .Index(t => t.FUNCTION_ID)
                .Index(t => t.ROLE_ID);
            
            CreateTable(
                "FMS_CL.EM_FUNCTION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CODE = c.String(nullable: false, maxLength: 100),
                        NAME = c.String(maxLength: 100),
                        DISCRIBITION = c.String(maxLength: 1000),
                        TYPE = c.String(maxLength: 100),
                        IS_DELETED = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CREATION_TIME = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Function_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Function_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_GLOBAL_VAR",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        VALUE = c.String(),
                        REMARK = c.String(maxLength: 500),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_HAND_RECORD",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        HAND_TYPE = c.Decimal(precision: 5, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        OBJECT_ID = c.Decimal(precision: 19, scale: 0),
                        ADD_TIME = c.DateTime(),
                        IS_CANCEL = c.Decimal(precision: 5, scale: 0),
                        CANCEL_REASON = c.String(maxLength: 200),
                        START_TIME = c.DateTime(),
                        OBJECT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_ICON",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ICON_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        DISPLAY_NAME = c.String(maxLength: 50),
                        CLASS_NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_ICON_TYPE", t => t.ICON_TYPE_ID)
                .Index(t => t.ICON_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_ICON_TYPE",
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
                    { "DynamicFilter_IconType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_IN_EVENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        REPORT_TYPE = c.Decimal(nullable: false, precision: 5, scale: 0),
                        DISPLAY_NAME = c.String(maxLength: 50),
                        BTN_HTML = c.String(),
                        BTN_JS = c.String(),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.ABP_LANGUAGES",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 10),
                        DISPLAY_NAME = c.String(nullable: false, maxLength: 64),
                        ICON = c.String(maxLength: 128),
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
                    { "DynamicFilter_ApplicationLanguage_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ApplicationLanguage_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.ABP_LANGUAGE_TEXTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        LANGUAGE_NAME = c.String(nullable: false, maxLength: 10),
                        SOURCE = c.String(nullable: false, maxLength: 128),
                        KEY = c.String(nullable: false, maxLength: 256),
                        VALUE = c.String(nullable: false),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApplicationLanguageText_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_MANAGER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MANAGER_NAME = c.String(maxLength: 50),
                        MANAGER_NO = c.String(maxLength: 100),
                        BOSS_NO = c.String(maxLength: 50),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        CODE = c.String(maxLength: 50),
                        ROLE_NAME = c.String(maxLength: 50),
                        PESON_TYPE = c.String(maxLength: 50),
                        PHONE_NO = c.String(maxLength: 20),
                        CITY_NAME = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_MODULE_EVENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ANALYSIS_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CODE = c.String(maxLength: 50),
                        EVENT_TYPE = c.String(maxLength: 20),
                        EVENT_FROM = c.String(maxLength: 20),
                        EVENT_NAME = c.String(maxLength: 50),
                        SOURCE_TABLE = c.String(maxLength: 50),
                        SOURCE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        URL = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_ANALYSIS", t => t.ANALYSIS_ID, cascadeDelete: true)
                .Index(t => t.ANALYSIS_ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_BILL",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH = c.String(maxLength: 6),
                        BONUS_VALUE = c.Double(),
                        CUR_WAY = c.String(maxLength: 20),
                        CUR_USER_ID = c.Decimal(precision: 19, scale: 0),
                        CUR_TIME = c.DateTime(),
                        STATUS = c.String(maxLength: 50),
                        IS_USE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        STAGE_STATUS = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_BILL_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH_BILL_ID = c.Decimal(precision: 19, scale: 0),
                        LOG = c.String(),
                        LOG_TIME = c.DateTime(),
                        LOG_RESULT = c.String(maxLength: 50),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.GP_MONTH_BILL", t => t.MONTH_BILL_ID)
                .Index(t => t.MONTH_BILL_ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_TARGET",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH = c.String(maxLength: 6),
                        MONTH_BILL_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_NAME = c.String(maxLength: 50),
                        CHOOSE_TYPE = c.String(maxLength: 20),
                        WEIGHT = c.Double(),
                        REMARK = c.String(maxLength: 200),
                        END_TABLE = c.String(maxLength: 100),
                        MAIN_FIELD = c.String(maxLength: 100),
                        CRISIS_VALUE = c.Double(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.GP_MONTH_BILL", t => t.MONTH_BILL_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TAG", t => t.TARGET_TAG_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TYPE", t => t.TARGET_TYPE_ID)
                .Index(t => t.MONTH_BILL_ID)
                .Index(t => t.TARGET_TYPE_ID)
                .Index(t => t.TARGET_TAG_ID);
            
            CreateTable(
                "FMS_CL.GP_TARGET_TAG",
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
                    { "DynamicFilter_TargetTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_TARGET_TYPE",
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
                    { "DynamicFilter_TargetType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_BONUS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH = c.String(nullable: false, maxLength: 6),
                        BONUS_VALUE = c.Double(nullable: false),
                        IN_WAY = c.String(maxLength: 20),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_BONUS_DETAIL",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH_BILL_ID = c.Decimal(precision: 19, scale: 0),
                        MONTH = c.String(maxLength: 6),
                        TARGET_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        MANAGER_NO = c.String(maxLength: 50),
                        MANAGER_NAME = c.String(maxLength: 50),
                        TARGET_SCORE = c.Double(),
                        MARK_SCORE = c.Double(),
                        MONTH_SCORE = c.Double(),
                        BONUS_RATIO = c.Double(),
                        BONUS_VALUE = c.Double(),
                        REMARK = c.String(maxLength: 300),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .ForeignKey("FMS_CL.GP_MONTH_BILL", t => t.MONTH_BILL_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TAG", t => t.TARGET_TAG_ID)
                .Index(t => t.MONTH_BILL_ID)
                .Index(t => t.TARGET_TAG_ID)
                .Index(t => t.DISTRICT_ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_SUBITEM_SCORE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH = c.String(maxLength: 6),
                        SUBITEM_ID = c.Decimal(precision: 19, scale: 0),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        MANAGER_NO = c.String(maxLength: 50),
                        MANAGER_NAME = c.String(maxLength: 50),
                        MARK_SCORE = c.Double(),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .ForeignKey("FMS_CL.GP_SUBITEM", t => t.SUBITEM_ID)
                .Index(t => t.SUBITEM_ID)
                .Index(t => t.DISTRICT_ID);
            
            CreateTable(
                "FMS_CL.GP_SUBITEM",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SUBITEM_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 50),
                        WEIGHT = c.Double(nullable: false),
                        REMARK = c.String(maxLength: 200),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.GP_SUBITEM_TYPE", t => t.SUBITEM_TYPE_ID)
                .Index(t => t.SUBITEM_TYPE_ID);
            
            CreateTable(
                "FMS_CL.GP_SUBITEM_TYPE",
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
                    { "DynamicFilter_SubitemType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_TARGET_DETAIL",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH = c.String(maxLength: 6),
                        MONTH_BILL_ID = c.Decimal(precision: 19, scale: 0),
                        MONTH_TARGET_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_ID = c.Decimal(precision: 19, scale: 0),
                        END_TABLE = c.String(maxLength: 100),
                        MAIN_FIELD = c.String(maxLength: 100),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        MANAGER_NO = c.String(maxLength: 50),
                        MANAGER_NAME = c.String(maxLength: 50),
                        WEIGHT = c.Double(),
                        YEAR_TVALUE = c.Double(),
                        TVALUE = c.Double(),
                        RESULT_VALUE = c.Double(),
                        CRISIS_VALUE = c.Double(),
                        SCORE = c.Double(),
                        REMARK = c.String(maxLength: 300),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .ForeignKey("FMS_CL.GP_MONTH_BILL", t => t.MONTH_BILL_ID)
                .ForeignKey("FMS_CL.GP_MONTH_TARGET", t => t.MONTH_TARGET_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TAG", t => t.TARGET_TAG_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TYPE", t => t.TARGET_TYPE_ID)
                .Index(t => t.MONTH_BILL_ID)
                .Index(t => t.MONTH_TARGET_ID)
                .Index(t => t.TARGET_TYPE_ID)
                .Index(t => t.TARGET_TAG_ID)
                .Index(t => t.DISTRICT_ID);
            
            CreateTable(
                "FMS_CL.GP_MONTH_TARGET_FORMULA",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        MONTH_BILL_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_FORMULA_ID = c.Decimal(precision: 19, scale: 0),
                        TYPE = c.String(maxLength: 50),
                        NAME = c.String(maxLength: 50),
                        CN_EXPRESSION = c.String(maxLength: 100),
                        EN_EXPRESSION = c.String(maxLength: 100),
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
                    { "DynamicFilter_MonthTargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.GP_MONTH_BILL", t => t.MONTH_BILL_ID)
                .ForeignKey("FMS_CL.GP_TARGET_FORMULA", t => t.TARGET_FORMULA_ID)
                .Index(t => t.MONTH_BILL_ID)
                .Index(t => t.TARGET_FORMULA_ID);
            
            CreateTable(
                "FMS_CL.GP_TARGET_FORMULA",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TYPE = c.String(maxLength: 50),
                        NAME = c.String(nullable: false, maxLength: 50),
                        CN_EXPRESSION = c.String(nullable: false, maxLength: 100),
                        EN_EXPRESSION = c.String(maxLength: 100),
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
                    { "DynamicFilter_TargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_NODE_POSITION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        X = c.Decimal(precision: 10, scale: 0),
                        Y = c.Decimal(precision: 10, scale: 0),
                        DIV_ID = c.String(maxLength: 100),
                        DIV_HIGH = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_SCRIPT", t => t.SCRIPT_ID)
                .Index(t => t.SCRIPT_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        SCRIPT_TYPE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CRON = c.String(maxLength: 50),
                        STATUS = c.Decimal(precision: 5, scale: 0),
                        RETRY_TIME = c.Decimal(precision: 10, scale: 0),
                        REMARK = c.String(maxLength: 500),
                        DIV_HIGH = c.Decimal(precision: 10, scale: 0),
                        DIV_WIDE = c.Decimal(precision: 10, scale: 0),
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
                    { "DynamicFilter_Script_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_SCRIPT_TYPE", t => t.SCRIPT_TYPE_ID, cascadeDelete: true)
                .Index(t => t.SCRIPT_TYPE_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_TYPE",
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
                    { "DynamicFilter_ScriptType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_NODE_POSITION_FORCASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        X = c.Decimal(precision: 10, scale: 0),
                        Y = c.Decimal(precision: 10, scale: 0),
                        DIV_ID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_SCRIPT_CASE", t => t.SCRIPT_CASE_ID)
                .Index(t => t.SCRIPT_CASE_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_CASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        RETRY_TIME = c.Decimal(precision: 10, scale: 0),
                        START_TIME = c.DateTime(),
                        START_MODEL = c.Decimal(precision: 5, scale: 0),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        RUN_STATUS = c.Decimal(precision: 5, scale: 0),
                        IS_HAVE_FAIL = c.Decimal(precision: 5, scale: 0),
                        RETURN_CODE = c.Decimal(precision: 5, scale: 0),
                        END_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.AbpNotifications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotificationName = c.String(nullable: false, maxLength: 96),
                        Data = c.String(),
                        DataTypeName = c.String(maxLength: 512),
                        EntityTypeName = c.String(maxLength: 250),
                        QualifiedName = c.String(maxLength: 512),
                        EntityId = c.String(maxLength: 96),
                        Severity = c.Decimal(nullable: false, precision: 3, scale: 0),
                        UserIds = c.String(),
                        ExcludedUserIds = c.String(),
                        TenantIds = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "FMS_CL.AbpNotificationSubscriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Decimal(precision: 10, scale: 0),
                        UserId = c.Decimal(nullable: false, precision: 19, scale: 0),
                        NotificationName = c.String(maxLength: 96),
                        EntityTypeName = c.String(maxLength: 250),
                        QualifiedName = c.String(maxLength: 512),
                        EntityId = c.String(maxLength: 96),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotificationSubscriptionInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.NotificationName, t.EntityTypeName, t.EntityId, t.UserId });
            
            CreateTable(
                "FMS_CL.ABP_ORGANIZATION_UNITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        CODE = c.String(nullable: false, maxLength: 95),
                        DISPLAY_NAME = c.String(nullable: false, maxLength: 128),
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
                    { "DynamicFilter_OrganizationUnit_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_OrganizationUnit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_ORGANIZATION_UNITS", t => t.PARENT_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.EM_PARAM",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TB_REPORT_OUTEVENT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        IS_FIELD = c.Decimal(nullable: false, precision: 1, scale: 0),
                        FIELD_CODE = c.String(maxLength: 50),
                        P_VALUE = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        ORDER_NUM = c.Decimal(precision: 10, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_TB_REPORT_OUTEVENT", t => t.TB_REPORT_OUTEVENT_ID)
                .Index(t => t.TB_REPORT_OUTEVENT_ID);
            
            CreateTable(
                "FMS_CL.EM_TB_REPORT_OUTEVENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TB_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        EVENT_TYPE = c.String(maxLength: 20),
                        FIELD_CODE = c.String(maxLength: 50),
                        DISPLAY_WAY = c.String(maxLength: 20),
                        DISPLAY_CONDITION = c.String(maxLength: 500),
                        OPEN_WAY = c.String(maxLength: 50),
                        URL = c.String(maxLength: 2000),
                        DISPLAY_NAME = c.String(maxLength: 20),
                        ICON = c.String(maxLength: 50),
                        STYLE = c.String(maxLength: 50),
                        TITLE = c.String(maxLength: 50),
                        HEIGHT = c.Decimal(precision: 10, scale: 0),
                        WIDTH = c.Decimal(precision: 10, scale: 0),
                        GROUP_NAME = c.String(maxLength: 50),
                        ORDER_NUM = c.Decimal(precision: 10, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_TB_REPORT", t => t.TB_REPORT_ID)
                .Index(t => t.TB_REPORT_ID);
            
            CreateTable(
                "FMS_CL.EM_TB_REPORT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        APPLICATION_TYPE = c.String(maxLength: 50),
                        REPORT_TYPE = c.Decimal(nullable: false, precision: 5, scale: 0),
                        REPORT_STYLE = c.String(maxLength: 50),
                        IS_CHECK = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_AUTO_LOAD = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_BIGDATA_LOAD = c.Decimal(nullable: false, precision: 1, scale: 0),
                        MAX_EXPORT_COUNT = c.Decimal(precision: 10, scale: 0),
                        IS_PAINATION = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_SCROLL = c.Decimal(nullable: false, precision: 1, scale: 0),
                        EMPTY_RECORD = c.String(maxLength: 200),
                        ROW_NUM = c.Decimal(precision: 10, scale: 0),
                        ROW_LIST = c.String(maxLength: 50),
                        ROW_STYLE = c.String(maxLength: 50),
                        SELECT_COLOR = c.String(maxLength: 50),
                        IS_OPEN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_ROWNUMBER = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ROWNUM_WIDTH = c.Decimal(precision: 10, scale: 0),
                        MERGE_CELL_JSON = c.String(),
                        MULTIBOX_ONLY = c.Decimal(precision: 1, scale: 0),
                        IS_MULTI_SORT = c.Decimal(precision: 1, scale: 0),
                        IS_SHOW_HEADER = c.Decimal(precision: 1, scale: 0),
                        IS_SHOW_FILTER = c.Decimal(precision: 1, scale: 0),
                        JS_FUN = c.String(),
                        REMARK = c.String(),
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
                    { "DynamicFilter_TbReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_REPORT", t => t.REPORT_ID)
                .Index(t => t.REPORT_ID);
            
            CreateTable(
                "FMS_CL.EM_RDLC_REPORT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        APPLICATION_TYPE = c.String(maxLength: 50),
                        ROW_NUM = c.Decimal(precision: 10, scale: 0),
                        IS_OPEN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        RDLC_XML = c.String(),
                        IS_SHOW_FILTER = c.Decimal(precision: 1, scale: 0),
                        REMARK = c.String(),
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
                    { "DynamicFilter_RdlcReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_REPORT", t => t.REPORT_ID)
                .Index(t => t.REPORT_ID);
            
            CreateTable(
                "FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        CONTENT_REPLY_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_CONTENT_REPLY", t => t.CONTENT_REPLY_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.USER_ID, cascadeDelete: true)
                .Index(t => t.CONTENT_REPLY_ID)
                .Index(t => t.USER_ID);
            
            CreateTable(
                "FMS_CL.EM_REPORT_FILTER",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TB_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        RDLC_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        CHART_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        FIELD_CODE = c.String(maxLength: 50),
                        FIELD_PARAM = c.String(maxLength: 50),
                        FIELD_NAME = c.String(maxLength: 50),
                        REGULAR_ID = c.Decimal(precision: 19, scale: 0),
                        DEFAULT_VALUE = c.String(maxLength: 100),
                        DATA_TYPE = c.String(maxLength: 20),
                        FILTER_TYPE = c.String(maxLength: 20),
                        FILTER_SQL = c.String(),
                        ORDER_NUM = c.Decimal(precision: 10, scale: 0),
                        IS_QUICK = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_SEARCH = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PLACEHOLDER = c.String(maxLength: 100),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_REGULAR", t => t.REGULAR_ID)
                .Index(t => t.REGULAR_ID);
            
            CreateTable(
                "FMS_CL.EM_ROLE_MODULE_EVENT",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        ROLE_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        MODULE_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        EVENT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_MODULE", t => t.MODULE_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_MODULE_EVENT", t => t.EVENT_ID, cascadeDelete: true)
                .ForeignKey("FMS_CL.ABP_ROLES", t => t.ROLE_ID, cascadeDelete: true)
                .Index(t => t.ROLE_ID)
                .Index(t => t.MODULE_ID)
                .Index(t => t.EVENT_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_CASE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        LOG_TIME = c.DateTime(),
                        LOG_LEVEL = c.Decimal(precision: 5, scale: 0),
                        LOG_MSG = c.String(),
                        SQL_MSG = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_FUNCTION",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        CONTENT = c.String(),
                        REMARK = c.String(maxLength: 500),
                        STATUS = c.Decimal(precision: 5, scale: 0),
                        CREATE_TIME = c.DateTime(),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        COMPILE_STATUS = c.Decimal(precision: 5, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        NAME = c.String(maxLength: 50),
                        SCRIPT_NODE_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        CODE = c.String(maxLength: 50),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_MODEL = c.Decimal(precision: 5, scale: 0),
                        CONTENT = c.String(),
                        REMARK = c.String(maxLength: 200),
                        TASK_SPECIFIC = c.String(maxLength: 200),
                        E_TABLE_NAME = c.String(maxLength: 50),
                        C_TABLE_NAME = c.String(maxLength: 50),
                        TABLE_TYPE = c.Decimal(precision: 5, scale: 0),
                        TABLE_MODEL = c.Decimal(precision: 5, scale: 0),
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
                    { "DynamicFilter_ScriptNode_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_SERVER", t => t.DB_SERVER_ID)
                .ForeignKey("FMS_CL.EM_SCRIPT_NODE_TYPE", t => t.SCRIPT_NODE_TYPE_ID)
                .Index(t => t.SCRIPT_NODE_TYPE_ID)
                .Index(t => t.DB_SERVER_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE_TYPE",
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
                    { "DynamicFilter_ScriptNodeType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE_CASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_MODEL = c.Decimal(precision: 5, scale: 0),
                        CONTENT = c.String(),
                        COMPILE_CONTENT = c.String(),
                        REMARK = c.String(maxLength: 200),
                        E_TABLE_NAME = c.String(maxLength: 50),
                        C_TABLE_NAME = c.String(maxLength: 50),
                        TABLE_TYPE = c.Decimal(precision: 5, scale: 0),
                        TABLE_MODEL = c.Decimal(precision: 5, scale: 0),
                        CREATE_TIME = c.DateTime(),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        TABLE_SUFFIX = c.Decimal(precision: 19, scale: 0),
                        START_TIME = c.DateTime(),
                        RUN_STATUS = c.Decimal(precision: 5, scale: 0),
                        RETURN_CODE = c.Decimal(precision: 5, scale: 0),
                        RETRY_TIME = c.Decimal(precision: 10, scale: 0),
                        END_TIME = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_SCRIPT_CASE", t => t.SCRIPT_CASE_ID)
                .Index(t => t.SCRIPT_CASE_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE_CASE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_NODE_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        LOG_TIME = c.DateTime(),
                        LOG_LEVEL = c.Decimal(precision: 5, scale: 0),
                        LOG_MSG = c.String(),
                        SQL_MSG = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE_FORCASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_NODE_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        CODE = c.String(maxLength: 50),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_MODEL = c.Decimal(precision: 5, scale: 0),
                        CONTENT = c.String(),
                        REMARK = c.String(maxLength: 200),
                        E_TABLE_NAME = c.String(maxLength: 50),
                        C_TABLE_NAME = c.String(maxLength: 50),
                        TABLE_TYPE = c.Decimal(precision: 5, scale: 0),
                        TABLE_MODEL = c.Decimal(precision: 5, scale: 0),
                        CREATE_TIME = c.DateTime(),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_NODE_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        LOG_MODEL = c.Decimal(precision: 5, scale: 0),
                        NAME = c.String(maxLength: 50),
                        SCRIPT_NODE_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        CODE = c.String(maxLength: 50),
                        DB_SERVER_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_MODEL = c.Decimal(precision: 5, scale: 0),
                        CONTENT = c.String(),
                        REMARK = c.String(maxLength: 200),
                        E_TABLE_NAME = c.String(maxLength: 50),
                        C_TABLE_NAME = c.String(maxLength: 50),
                        TABLE_TYPE = c.Decimal(precision: 5, scale: 0),
                        TABLE_MODEL = c.Decimal(precision: 5, scale: 0),
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
                    { "DynamicFilter_ScriptNodeLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DB_SERVER", t => t.DB_SERVER_ID)
                .ForeignKey("FMS_CL.EM_SCRIPT_NODE_TYPE", t => t.SCRIPT_NODE_TYPE_ID)
                .Index(t => t.SCRIPT_NODE_TYPE_ID)
                .Index(t => t.DB_SERVER_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_REF_NODE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        PARENT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        CURR_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_SCRIPT", t => t.SCRIPT_ID)
                .Index(t => t.SCRIPT_ID);
            
            CreateTable(
                "FMS_CL.EM_SCRIPT_REF_NODE_FORCASE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        SCRIPT_ID = c.Decimal(precision: 19, scale: 0),
                        SCRIPT_CASE_ID = c.Decimal(precision: 19, scale: 0),
                        PARENT_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        CURR_NODE_ID = c.Decimal(precision: 19, scale: 0),
                        REMARK = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.GP_TARGET",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TARGET_TYPE_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 50),
                        CHOOSE_TYPE = c.String(maxLength: 20),
                        WEIGHT = c.Double(nullable: false),
                        REMARK = c.String(maxLength: 200),
                        IS_USE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        END_TABLE = c.String(nullable: false, maxLength: 100),
                        MAIN_FIELD = c.String(maxLength: 100),
                        CRISIS_VALUE = c.Double(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.GP_TARGET_TAG", t => t.TARGET_TAG_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TYPE", t => t.TARGET_TYPE_ID)
                .Index(t => t.TARGET_TYPE_ID)
                .Index(t => t.TARGET_TAG_ID);
            
            CreateTable(
                "FMS_CL.GP_TARGET_VALUE",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        YEAR_MONTH = c.String(maxLength: 6),
                        TARGET_ID = c.Decimal(precision: 19, scale: 0),
                        TARGET_TAG_ID = c.Decimal(precision: 19, scale: 0),
                        DISTRICT_ID = c.Decimal(precision: 19, scale: 0),
                        MANAGER_NO = c.String(maxLength: 50),
                        MANAGER_NAME = c.String(maxLength: 50),
                        TVALUE = c.Double(),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_DISTRICT", t => t.DISTRICT_ID)
                .ForeignKey("FMS_CL.GP_TARGET", t => t.TARGET_ID)
                .ForeignKey("FMS_CL.GP_TARGET_TAG", t => t.TARGET_TAG_ID)
                .Index(t => t.TARGET_ID)
                .Index(t => t.TARGET_TAG_ID)
                .Index(t => t.DISTRICT_ID);
            
            CreateTable(
                "FMS_CL.EM_TB_REPORT_FIELD",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        TB_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        TB_REPORT_FIELD_TOP_ID = c.Decimal(precision: 19, scale: 0),
                        FIELD_CODE = c.String(maxLength: 50),
                        FIELD_NAME = c.String(maxLength: 50),
                        DATA_TYPE = c.String(maxLength: 20),
                        IS_ORDER = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_SHOW = c.Decimal(nullable: false, precision: 1, scale: 0),
                        WIDTH = c.Decimal(precision: 10, scale: 0),
                        IS_SEARCH = c.Decimal(nullable: false, precision: 1, scale: 0),
                        IS_FROZEN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        ALIGN = c.String(maxLength: 20),
                        ORDER_NUM = c.Decimal(precision: 10, scale: 0),
                        REMARK = c.String(maxLength: 200),
                        TB_REPORT_OUTEVENT_ID = c.Decimal(precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_REPORT", t => t.REPORT_ID)
                .ForeignKey("FMS_CL.EM_TB_REPORT", t => t.TB_REPORT_ID)
                .ForeignKey("FMS_CL.EM_TB_REPORT_FIELD_TOP", t => t.TB_REPORT_FIELD_TOP_ID)
                .Index(t => t.REPORT_ID)
                .Index(t => t.TB_REPORT_ID)
                .Index(t => t.TB_REPORT_FIELD_TOP_ID);
            
            CreateTable(
                "FMS_CL.EM_TB_REPORT_FIELD_TOP",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        PARENT_ID = c.Decimal(precision: 19, scale: 0),
                        TB_REPORT_ID = c.Decimal(precision: 19, scale: 0),
                        NAME = c.String(maxLength: 50),
                        REMARK = c.String(maxLength: 200),
                        TB_REPORT_OUTEVENT_ID = c.Decimal(precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.EM_TB_REPORT_FIELD_TOP", t => t.PARENT_ID)
                .Index(t => t.PARENT_ID);
            
            CreateTable(
                "FMS_CL.AbpTenantNotifications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Decimal(precision: 10, scale: 0),
                        NotificationName = c.String(nullable: false, maxLength: 96),
                        Data = c.String(),
                        DataTypeName = c.String(maxLength: 512),
                        EntityTypeName = c.String(maxLength: 250),
                        QualifiedName = c.String(maxLength: 512),
                        EntityId = c.String(maxLength: 96),
                        Severity = c.Decimal(nullable: false, precision: 3, scale: 0),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantNotificationInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "FMS_CL.ABP_TENANTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        EDITION_ID = c.Decimal(precision: 10, scale: 0),
                        NAME = c.String(nullable: false, maxLength: 128),
                        IS_ACTIVE = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TENANCY_NAME = c.String(nullable: false, maxLength: 64),
                        CONNECTION_STRING = c.String(maxLength: 1024),
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
                    { "DynamicFilter_Tenant_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.CREATE_UID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.DELETE_UID)
                .ForeignKey("FMS_CL.ABP_EDITIONS", t => t.EDITION_ID)
                .ForeignKey("FMS_CL.ABP_USERS", t => t.UPDATE_UID)
                .Index(t => t.EDITION_ID)
                .Index(t => t.CREATE_UID)
                .Index(t => t.DELETE_UID)
                .Index(t => t.UPDATE_UID);
            
            CreateTable(
                "FMS_CL.ABP_USER_ACCOUNTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        USER_LINK_ID = c.Decimal(precision: 19, scale: 0),
                        USER_NAME = c.String(),
                        EMAIL_ADDRESS = c.String(),
                        LAST_LOGIN_TIME = c.DateTime(),
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
                    { "DynamicFilter_UserAccount_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.ABP_USER_LOGIN_ATTEMPTS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        TENANCY_NAME = c.String(maxLength: 64),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        USER_NAME_OR_EMAIL_ADDRESS = c.String(maxLength: 255),
                        CLIENT_IP_ADDRESS = c.String(maxLength: 64),
                        CLIENT_NAME = c.String(maxLength: 128),
                        BROWDER_INFO = c.String(maxLength: 256),
                        RESULT = c.Decimal(nullable: false, precision: 3, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserLoginAttempt_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID)
                .Index(t => new { t.USER_ID, t.TENANT_ID }, name: "IX_UserId_TenantId")
                .Index(t => new { t.TENANCY_NAME, t.USER_NAME_OR_EMAIL_ADDRESS, t.RESULT }, name: "IX_TenancyName_UserNameOrEmailAddress_Result");
            
            CreateTable(
                "FMS_CL.AbpUserNotifications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Decimal(precision: 10, scale: 0),
                        UserId = c.Decimal(nullable: false, precision: 19, scale: 0),
                        TenantNotificationId = c.Guid(nullable: false),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CreationTime = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserNotificationInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserId, t.State, t.CreationTime });
            
            CreateTable(
                "FMS_CL.ABP_USER_ORGANIZATION_UNITS",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        TENANT_ID = c.Decimal(precision: 10, scale: 0),
                        USER_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        ORGANIZATION_UNIT_ID = c.Decimal(nullable: false, precision: 19, scale: 0),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserOrganizationUnit_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.EM_USER_PWD_LOG",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 19, scale: 0, identity: true),
                        USER_ID = c.Decimal(precision: 19, scale: 0),
                        OLD_PWD = c.String(maxLength: 128),
                        NEW_PWD = c.String(maxLength: 128),
                        CREATE_TIME = c.DateTime(nullable: false),
                        CREATE_UID = c.Decimal(precision: 19, scale: 0),
                        UPDATE_TIME = c.DateTime(),
                        UPDATE_UID = c.Decimal(precision: 19, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "FMS_CL.DefaultFieldImpTbs",
                c => new
                    {
                        DefaultField_Id = c.Decimal(nullable: false, precision: 19, scale: 0),
                        ImpTb_Id = c.Decimal(nullable: false, precision: 19, scale: 0),
                    })
                .PrimaryKey(t => new { t.DefaultField_Id, t.ImpTb_Id })
                .ForeignKey("FMS_CL.EM_DEFAULT_FIELD", t => t.DefaultField_Id, cascadeDelete: true)
                .ForeignKey("FMS_CL.EM_IMP_TB", t => t.ImpTb_Id, cascadeDelete: true)
                .Index(t => t.DefaultField_Id)
                .Index(t => t.ImpTb_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("FMS_CL.ABP_TENANTS", "UPDATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_TENANTS", "EDITION_ID", "FMS_CL.ABP_EDITIONS");
            DropForeignKey("FMS_CL.ABP_TENANTS", "DELETE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_TENANTS", "CREATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_TB_REPORT_FIELD", "TB_REPORT_FIELD_TOP_ID", "FMS_CL.EM_TB_REPORT_FIELD_TOP");
            DropForeignKey("FMS_CL.EM_TB_REPORT_FIELD_TOP", "PARENT_ID", "FMS_CL.EM_TB_REPORT_FIELD_TOP");
            DropForeignKey("FMS_CL.EM_TB_REPORT_FIELD", "TB_REPORT_ID", "FMS_CL.EM_TB_REPORT");
            DropForeignKey("FMS_CL.EM_TB_REPORT_FIELD", "REPORT_ID", "FMS_CL.EM_REPORT");
            DropForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            DropForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_ID", "FMS_CL.GP_TARGET");
            DropForeignKey("FMS_CL.GP_TARGET_VALUE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.GP_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            DropForeignKey("FMS_CL.GP_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            DropForeignKey("FMS_CL.EM_SCRIPT_REF_NODE", "SCRIPT_ID", "FMS_CL.EM_SCRIPT");
            DropForeignKey("FMS_CL.EM_SCRIPT_NODE_LOG", "SCRIPT_NODE_TYPE_ID", "FMS_CL.EM_SCRIPT_NODE_TYPE");
            DropForeignKey("FMS_CL.EM_SCRIPT_NODE_LOG", "DB_SERVER_ID", "FMS_CL.EM_DB_SERVER");
            DropForeignKey("FMS_CL.EM_SCRIPT_NODE_CASE", "SCRIPT_CASE_ID", "FMS_CL.EM_SCRIPT_CASE");
            DropForeignKey("FMS_CL.EM_SCRIPT_NODE", "SCRIPT_NODE_TYPE_ID", "FMS_CL.EM_SCRIPT_NODE_TYPE");
            DropForeignKey("FMS_CL.EM_SCRIPT_NODE", "DB_SERVER_ID", "FMS_CL.EM_DB_SERVER");
            DropForeignKey("FMS_CL.EM_ROLE_MODULE_EVENT", "ROLE_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.EM_ROLE_MODULE_EVENT", "EVENT_ID", "FMS_CL.EM_MODULE_EVENT");
            DropForeignKey("FMS_CL.EM_ROLE_MODULE_EVENT", "MODULE_ID", "FMS_CL.EM_MODULE");
            DropForeignKey("FMS_CL.EM_REPORT_FILTER", "REGULAR_ID", "FMS_CL.EM_REGULAR");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG", "CONTENT_REPLY_ID", "FMS_CL.EM_CONTENT_REPLY");
            DropForeignKey("FMS_CL.EM_RDLC_REPORT", "REPORT_ID", "FMS_CL.EM_REPORT");
            DropForeignKey("FMS_CL.EM_PARAM", "TB_REPORT_OUTEVENT_ID", "FMS_CL.EM_TB_REPORT_OUTEVENT");
            DropForeignKey("FMS_CL.EM_TB_REPORT_OUTEVENT", "TB_REPORT_ID", "FMS_CL.EM_TB_REPORT");
            DropForeignKey("FMS_CL.EM_TB_REPORT", "REPORT_ID", "FMS_CL.EM_REPORT");
            DropForeignKey("FMS_CL.ABP_ORGANIZATION_UNITS", "PARENT_ID", "FMS_CL.ABP_ORGANIZATION_UNITS");
            DropForeignKey("FMS_CL.EM_NODE_POSITION_FORCASE", "SCRIPT_CASE_ID", "FMS_CL.EM_SCRIPT_CASE");
            DropForeignKey("FMS_CL.EM_NODE_POSITION", "SCRIPT_ID", "FMS_CL.EM_SCRIPT");
            DropForeignKey("FMS_CL.EM_SCRIPT", "SCRIPT_TYPE_ID", "FMS_CL.EM_SCRIPT_TYPE");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "TARGET_FORMULA_ID", "FMS_CL.GP_TARGET_FORMULA");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_TARGET_ID", "FMS_CL.GP_MONTH_TARGET");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "SUBITEM_ID", "FMS_CL.GP_SUBITEM");
            DropForeignKey("FMS_CL.GP_SUBITEM", "SUBITEM_TYPE_ID", "FMS_CL.GP_SUBITEM_TYPE");
            DropForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            DropForeignKey("FMS_CL.GP_MONTH_TARGET", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            DropForeignKey("FMS_CL.GP_MONTH_BILL_LOG", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            DropForeignKey("FMS_CL.EM_MODULE_EVENT", "ANALYSIS_ID", "FMS_CL.EM_ANALYSIS");
            DropForeignKey("FMS_CL.EM_ICON", "ICON_TYPE_ID", "FMS_CL.EM_ICON_TYPE");
            DropForeignKey("FMS_CL.EM_FUNCTION_ROLE", "ROLE_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.EM_FUNCTION_ROLE", "FUNCTION_ID", "FMS_CL.EM_FUNCTION");
            DropForeignKey("FMS_CL.ABP_FEATURES", "EDITION_ID", "FMS_CL.ABP_EDITIONS");
            DropForeignKey("FMS_CL.EM_DOWN_DATA", "EXPORT_DATA_ID", "FMS_CL.EM_EXPORT_DATA");
            DropForeignKey("FMS_CL.EM_EXPORT_DATA", "MODULE_ID", "FMS_CL.EM_MODULE");
            DropForeignKey("FMS_CL.EM_ROLE_MODULE", "ROLE_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.EM_ROLE_MODULE", "MODULE_ID", "FMS_CL.EM_MODULE");
            DropForeignKey("FMS_CL.EM_MODULE", "PARENT_ID", "FMS_CL.EM_MODULE");
            DropForeignKey("FMS_CL.EM_DICTIONARY", "PARENT_ID", "FMS_CL.EM_DICTIONARY");
            DropForeignKey("FMS_CL.EM_DICTIONARY", "DICTIONARY_TYPE_ID", "FMS_CL.EM_DICTIONARY_TYPE");
            DropForeignKey("FMS_CL.EM_DEFINE_CONFIG", "DEFINE_ID", "FMS_CL.EM_DEFINE");
            DropForeignKey("FMS_CL.EM_CONTENT_USER", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_USER", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_ROLE", "ROLE_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.ABP_PERMISSIONS", "ROLE_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.ABP_ROLES", "UPDATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_ROLES", "DELETE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_ROLES", "CREATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_ROLES", "PARENT_ID", "FMS_CL.ABP_ROLES");
            DropForeignKey("FMS_CL.EM_CONTENT_ROLE", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY_FILE", "FILE_ID", "FMS_CL.EM_FILES");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY_FILE", "CONTENT_REPLY_ID", "FMS_CL.EM_CONTENT_REPLY");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY", "REPLY_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY", "DELETE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_REPLY", "PARENT_ID", "FMS_CL.EM_CONTENT_REPLY");
            DropForeignKey("FMS_CL.EM_CONTENT_REF_TAG", "TAG_ID", "FMS_CL.EM_CONTENT_TAG");
            DropForeignKey("FMS_CL.EM_CONTENT_REF_TAG", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_READ_LOG", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_READ_LOG", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_PUSH_WAY", "PUSH_WAY_ID", "FMS_CL.EM_PUSH_WAY");
            DropForeignKey("FMS_CL.EM_CONTENT_PUSH_WAY", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_PRAISE_LOG", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_PRAISE_LOG", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_LOG", "CREATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_SETTINGS", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USER_ROLES", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_PERMISSIONS", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USER_LOGINS", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USERS", "UPDATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USERS", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.ABP_USERS", "DEPARTMENT_ID", "FMS_CL.EM_DEPARTMENT");
            DropForeignKey("FMS_CL.EM_DEPARTMENT", "PARENT_ID", "FMS_CL.EM_DEPARTMENT");
            DropForeignKey("FMS_CL.ABP_USERS", "DELETE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USERS", "CREATE_UID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.ABP_USER_CLAIMS", "USER_ID", "FMS_CL.ABP_USERS");
            DropForeignKey("FMS_CL.EM_CONTENT_LOG", "DEFINE_TYPE_ID", "FMS_CL.EM_CONTENT_TYPE");
            DropForeignKey("FMS_CL.EM_CONTENT_FILE", "FILE_ID", "FMS_CL.EM_FILES");
            DropForeignKey("FMS_CL.EM_CONTENT_FILE", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_DISTRICT", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.EM_DISTRICT", "PARENT_ID", "FMS_CL.EM_DISTRICT");
            DropForeignKey("FMS_CL.EM_CONTENT_DISTRICT", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT_CHECK", "CONTENT_ID", "FMS_CL.EM_CONTENT");
            DropForeignKey("FMS_CL.EM_CONTENT", "DEFINE_TYPE_ID", "FMS_CL.EM_CONTENT_TYPE");
            DropForeignKey("FMS_CL.EM_CONTENT_TYPE", "DEFINE_ID", "FMS_CL.EM_DEFINE");
            DropForeignKey("FMS_CL.EM_CONTENT_TYPE", "ContentType_Id", "FMS_CL.EM_CONTENT_TYPE");
            DropForeignKey("FMS_CL.EM_CONTENT", "DEFINE_ID", "FMS_CL.EM_DEFINE");
            DropForeignKey("FMS_CL.EM_CHART_TEMP", "CHART_TYPE_ID", "FMS_CL.EM_CHART_TYPE");
            DropForeignKey("FMS_CL.EM_CHART_REPORT", "REPORT_ID", "FMS_CL.EM_REPORT");
            DropForeignKey("FMS_CL.EM_REPORT", "DB_SERVER_ID", "FMS_CL.EM_DB_SERVER");
            DropForeignKey("FMS_CL.EM_CHART_REPORT", "CHART_TYPE_ID", "FMS_CL.EM_CHART_TYPE");
            DropForeignKey("FMS_CL.EM_APP_VERSION", "FILE_ID", "FMS_CL.EM_FILES");
            DropForeignKey("FMS_CL.EM_OFFLINE_LOG", "IMPORT_LOG_ID", "FMS_CL.EM_IMPORT_LOG");
            DropForeignKey("FMS_CL.EM_IMP_TB", "IMP_TYPE_ID", "FMS_CL.EM_IMP_TYPE");
            DropForeignKey("FMS_CL.EM_IMP_TB_FIELD", "REGULAR_ID", "FMS_CL.EM_REGULAR");
            DropForeignKey("FMS_CL.EM_IMP_TB_FIELD", "IMP_TB_ID", "FMS_CL.EM_IMP_TB");
            DropForeignKey("FMS_CL.EM_IMP_TB_CASE", "IMP_TB_ID", "FMS_CL.EM_IMP_TB");
            DropForeignKey("FMS_CL.EM_IMPORT_LOG", "IMP_TB_CASE_ID", "FMS_CL.EM_IMP_TB_CASE");
            DropForeignKey("FMS_CL.EM_IMPORT_LOG", "IMP_TB_ID", "FMS_CL.EM_IMP_TB");
            DropForeignKey("FMS_CL.DefaultFieldImpTbs", "ImpTb_Id", "FMS_CL.EM_IMP_TB");
            DropForeignKey("FMS_CL.DefaultFieldImpTbs", "DefaultField_Id", "FMS_CL.EM_DEFAULT_FIELD");
            DropForeignKey("FMS_CL.EM_IMP_TB", "DB_SERVER_ID", "FMS_CL.EM_DB_SERVER");
            DropForeignKey("FMS_CL.EM_DB_SERVER", "DB_TYPE_ID", "FMS_CL.EM_DB_TYPE");
            DropForeignKey("FMS_CL.EM_PRE_DATA_TYPE", "DB_TYPE_ID", "FMS_CL.EM_DB_TYPE");
            DropForeignKey("FMS_CL.EM_DB_SERVER", "DB_TAG_ID", "FMS_CL.EM_DB_TAG");
            DropForeignKey("FMS_CL.EM_IMPORT_LOG", "FILE_ID", "FMS_CL.EM_FILES");
            DropIndex("FMS_CL.DefaultFieldImpTbs", new[] { "ImpTb_Id" });
            DropIndex("FMS_CL.DefaultFieldImpTbs", new[] { "DefaultField_Id" });
            DropIndex("FMS_CL.AbpUserNotifications", new[] { "UserId", "State", "CreationTime" });
            DropIndex("FMS_CL.ABP_USER_LOGIN_ATTEMPTS", "IX_TenancyName_UserNameOrEmailAddress_Result");
            DropIndex("FMS_CL.ABP_USER_LOGIN_ATTEMPTS", "IX_UserId_TenantId");
            DropIndex("FMS_CL.ABP_TENANTS", new[] { "UPDATE_UID" });
            DropIndex("FMS_CL.ABP_TENANTS", new[] { "DELETE_UID" });
            DropIndex("FMS_CL.ABP_TENANTS", new[] { "CREATE_UID" });
            DropIndex("FMS_CL.ABP_TENANTS", new[] { "EDITION_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT_FIELD_TOP", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT_FIELD", new[] { "TB_REPORT_FIELD_TOP_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT_FIELD", new[] { "TB_REPORT_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT_FIELD", new[] { "REPORT_ID" });
            DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "TARGET_TAG_ID" });
            DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "TARGET_ID" });
            DropIndex("FMS_CL.GP_TARGET", new[] { "TARGET_TAG_ID" });
            DropIndex("FMS_CL.GP_TARGET", new[] { "TARGET_TYPE_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_REF_NODE", new[] { "SCRIPT_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_NODE_LOG", new[] { "DB_SERVER_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_NODE_LOG", new[] { "SCRIPT_NODE_TYPE_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_NODE_CASE", new[] { "SCRIPT_CASE_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_NODE", new[] { "DB_SERVER_ID" });
            DropIndex("FMS_CL.EM_SCRIPT_NODE", new[] { "SCRIPT_NODE_TYPE_ID" });
            DropIndex("FMS_CL.EM_ROLE_MODULE_EVENT", new[] { "EVENT_ID" });
            DropIndex("FMS_CL.EM_ROLE_MODULE_EVENT", new[] { "MODULE_ID" });
            DropIndex("FMS_CL.EM_ROLE_MODULE_EVENT", new[] { "ROLE_ID" });
            DropIndex("FMS_CL.EM_REPORT_FILTER", new[] { "REGULAR_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG", new[] { "USER_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG", new[] { "CONTENT_REPLY_ID" });
            DropIndex("FMS_CL.EM_RDLC_REPORT", new[] { "REPORT_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT", new[] { "REPORT_ID" });
            DropIndex("FMS_CL.EM_TB_REPORT_OUTEVENT", new[] { "TB_REPORT_ID" });
            DropIndex("FMS_CL.EM_PARAM", new[] { "TB_REPORT_OUTEVENT_ID" });
            DropIndex("FMS_CL.ABP_ORGANIZATION_UNITS", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.AbpNotificationSubscriptions", new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });
            DropIndex("FMS_CL.EM_NODE_POSITION_FORCASE", new[] { "SCRIPT_CASE_ID" });
            DropIndex("FMS_CL.EM_SCRIPT", new[] { "SCRIPT_TYPE_ID" });
            DropIndex("FMS_CL.EM_NODE_POSITION", new[] { "SCRIPT_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", new[] { "TARGET_FORMULA_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", new[] { "MONTH_BILL_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "TARGET_TAG_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "TARGET_TYPE_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "MONTH_TARGET_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "MONTH_BILL_ID" });
            DropIndex("FMS_CL.GP_SUBITEM", new[] { "SUBITEM_TYPE_ID" });
            DropIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", new[] { "SUBITEM_ID" });
            DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "TARGET_TAG_ID" });
            DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "MONTH_BILL_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "TARGET_TAG_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "TARGET_TYPE_ID" });
            DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "MONTH_BILL_ID" });
            DropIndex("FMS_CL.GP_MONTH_BILL_LOG", new[] { "MONTH_BILL_ID" });
            DropIndex("FMS_CL.EM_MODULE_EVENT", new[] { "ANALYSIS_ID" });
            DropIndex("FMS_CL.EM_ICON", new[] { "ICON_TYPE_ID" });
            DropIndex("FMS_CL.EM_FUNCTION_ROLE", new[] { "ROLE_ID" });
            DropIndex("FMS_CL.EM_FUNCTION_ROLE", new[] { "FUNCTION_ID" });
            DropIndex("FMS_CL.ABP_FEATURES", new[] { "EDITION_ID" });
            DropIndex("FMS_CL.EM_ROLE_MODULE", new[] { "MODULE_ID" });
            DropIndex("FMS_CL.EM_ROLE_MODULE", new[] { "ROLE_ID" });
            DropIndex("FMS_CL.EM_MODULE", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_EXPORT_DATA", new[] { "MODULE_ID" });
            DropIndex("FMS_CL.EM_DOWN_DATA", new[] { "EXPORT_DATA_ID" });
            DropIndex("FMS_CL.EM_DICTIONARY", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_DICTIONARY", new[] { "DICTIONARY_TYPE_ID" });
            DropIndex("FMS_CL.EM_DEFINE_CONFIG", new[] { "DEFINE_ID" });
            DropIndex("FMS_CL.EM_CONTENT_USER", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_USER", new[] { "USER_ID" });
            DropIndex("FMS_CL.ABP_ROLES", new[] { "UPDATE_UID" });
            DropIndex("FMS_CL.ABP_ROLES", new[] { "DELETE_UID" });
            DropIndex("FMS_CL.ABP_ROLES", new[] { "CREATE_UID" });
            DropIndex("FMS_CL.ABP_ROLES", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_ROLE", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_ROLE", new[] { "ROLE_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY_FILE", new[] { "CONTENT_REPLY_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY_FILE", new[] { "FILE_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY", new[] { "DELETE_UID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY", new[] { "REPLY_UID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REPLY", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REF_TAG", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_REF_TAG", new[] { "TAG_ID" });
            DropIndex("FMS_CL.EM_CONTENT_READ_LOG", new[] { "USER_ID" });
            DropIndex("FMS_CL.EM_CONTENT_READ_LOG", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_PUSH_WAY", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_PUSH_WAY", new[] { "PUSH_WAY_ID" });
            DropIndex("FMS_CL.EM_CONTENT_PRAISE_LOG", new[] { "USER_ID" });
            DropIndex("FMS_CL.EM_CONTENT_PRAISE_LOG", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.ABP_SETTINGS", new[] { "USER_ID" });
            DropIndex("FMS_CL.ABP_USER_ROLES", new[] { "USER_ID" });
            DropIndex("FMS_CL.ABP_PERMISSIONS", new[] { "ROLE_ID" });
            DropIndex("FMS_CL.ABP_PERMISSIONS", new[] { "USER_ID" });
            DropIndex("FMS_CL.ABP_USER_LOGINS", new[] { "USER_ID" });
            DropIndex("FMS_CL.EM_DEPARTMENT", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.ABP_USER_CLAIMS", new[] { "USER_ID" });
            DropIndex("FMS_CL.ABP_USERS", new[] { "UPDATE_UID" });
            DropIndex("FMS_CL.ABP_USERS", new[] { "DELETE_UID" });
            DropIndex("FMS_CL.ABP_USERS", new[] { "CREATE_UID" });
            DropIndex("FMS_CL.ABP_USERS", new[] { "DEPARTMENT_ID" });
            DropIndex("FMS_CL.ABP_USERS", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_LOG", new[] { "CREATE_UID" });
            DropIndex("FMS_CL.EM_CONTENT_LOG", new[] { "DEFINE_TYPE_ID" });
            DropIndex("FMS_CL.EM_CONTENT_FILE", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_FILE", new[] { "FILE_ID" });
            DropIndex("FMS_CL.EM_DISTRICT", new[] { "PARENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_DISTRICT", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_DISTRICT", new[] { "DISTRICT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_CHECK", new[] { "CONTENT_ID" });
            DropIndex("FMS_CL.EM_CONTENT_TYPE", new[] { "ContentType_Id" });
            DropIndex("FMS_CL.EM_CONTENT_TYPE", new[] { "DEFINE_ID" });
            DropIndex("FMS_CL.EM_CONTENT", new[] { "DEFINE_ID" });
            DropIndex("FMS_CL.EM_CONTENT", new[] { "DEFINE_TYPE_ID" });
            DropIndex("FMS_CL.EM_CHART_TEMP", new[] { "CHART_TYPE_ID" });
            DropIndex("FMS_CL.EM_REPORT", new[] { "DB_SERVER_ID" });
            DropIndex("FMS_CL.EM_CHART_REPORT", new[] { "CHART_TYPE_ID" });
            DropIndex("FMS_CL.EM_CHART_REPORT", new[] { "REPORT_ID" });
            DropIndex("FMS_CL.AbpBackgroundJobs", new[] { "IsAbandoned", "NextTryTime" });
            DropIndex("FMS_CL.EM_OFFLINE_LOG", new[] { "IMPORT_LOG_ID" });
            DropIndex("FMS_CL.EM_IMP_TB_FIELD", new[] { "REGULAR_ID" });
            DropIndex("FMS_CL.EM_IMP_TB_FIELD", new[] { "IMP_TB_ID" });
            DropIndex("FMS_CL.EM_IMP_TB_CASE", new[] { "IMP_TB_ID" });
            DropIndex("FMS_CL.EM_PRE_DATA_TYPE", new[] { "DB_TYPE_ID" });
            DropIndex("FMS_CL.EM_DB_SERVER", new[] { "DB_TYPE_ID" });
            DropIndex("FMS_CL.EM_DB_SERVER", new[] { "DB_TAG_ID" });
            DropIndex("FMS_CL.EM_IMP_TB", new[] { "IMP_TYPE_ID" });
            DropIndex("FMS_CL.EM_IMP_TB", new[] { "DB_SERVER_ID" });
            DropIndex("FMS_CL.EM_IMPORT_LOG", new[] { "FILE_ID" });
            DropIndex("FMS_CL.EM_IMPORT_LOG", new[] { "IMP_TB_CASE_ID" });
            DropIndex("FMS_CL.EM_IMPORT_LOG", new[] { "IMP_TB_ID" });
            DropIndex("FMS_CL.EM_APP_VERSION", new[] { "FILE_ID" });
            DropTable("FMS_CL.DefaultFieldImpTbs");
            DropTable("FMS_CL.EM_USER_PWD_LOG");
            DropTable("FMS_CL.ABP_USER_ORGANIZATION_UNITS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserOrganizationUnit_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.AbpUserNotifications",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserNotificationInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USER_LOGIN_ATTEMPTS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserLoginAttempt_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USER_ACCOUNTS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserAccount_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_TENANTS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tenant_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.AbpTenantNotifications",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantNotificationInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_TB_REPORT_FIELD_TOP");
            DropTable("FMS_CL.EM_TB_REPORT_FIELD");
            DropTable("FMS_CL.GP_TARGET_VALUE");
            DropTable("FMS_CL.GP_TARGET");
            DropTable("FMS_CL.EM_SCRIPT_REF_NODE_FORCASE");
            DropTable("FMS_CL.EM_SCRIPT_REF_NODE");
            DropTable("FMS_CL.EM_SCRIPT_NODE_LOG",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScriptNodeLog_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_SCRIPT_NODE_FORCASE");
            DropTable("FMS_CL.EM_SCRIPT_NODE_CASE_LOG");
            DropTable("FMS_CL.EM_SCRIPT_NODE_CASE");
            DropTable("FMS_CL.EM_SCRIPT_NODE_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScriptNodeType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_SCRIPT_NODE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScriptNode_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_SCRIPT_FUNCTION");
            DropTable("FMS_CL.EM_SCRIPT_CASE_LOG");
            DropTable("FMS_CL.EM_ROLE_MODULE_EVENT");
            DropTable("FMS_CL.EM_REPORT_FILTER");
            DropTable("FMS_CL.EM_CONTENT_REPLY_PRAISE_LOG");
            DropTable("FMS_CL.EM_RDLC_REPORT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RdlcReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_TB_REPORT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TbReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_TB_REPORT_OUTEVENT");
            DropTable("FMS_CL.EM_PARAM");
            DropTable("FMS_CL.ABP_ORGANIZATION_UNITS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OrganizationUnit_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_OrganizationUnit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.AbpNotificationSubscriptions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotificationSubscriptionInfo_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.AbpNotifications");
            DropTable("FMS_CL.EM_SCRIPT_CASE");
            DropTable("FMS_CL.EM_NODE_POSITION_FORCASE");
            DropTable("FMS_CL.EM_SCRIPT_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScriptType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_SCRIPT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Script_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_NODE_POSITION");
            DropTable("FMS_CL.GP_TARGET_FORMULA",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.GP_MONTH_TARGET_FORMULA",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MonthTargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.GP_MONTH_TARGET_DETAIL");
            DropTable("FMS_CL.GP_SUBITEM_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubitemType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.GP_SUBITEM");
            DropTable("FMS_CL.GP_MONTH_SUBITEM_SCORE");
            DropTable("FMS_CL.GP_MONTH_BONUS_DETAIL");
            DropTable("FMS_CL.GP_MONTH_BONUS");
            DropTable("FMS_CL.GP_TARGET_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TargetType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.GP_TARGET_TAG",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TargetTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.GP_MONTH_TARGET");
            DropTable("FMS_CL.GP_MONTH_BILL_LOG");
            DropTable("FMS_CL.GP_MONTH_BILL");
            DropTable("FMS_CL.EM_MODULE_EVENT");
            DropTable("FMS_CL.GP_MANAGER");
            DropTable("FMS_CL.ABP_LANGUAGE_TEXTS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApplicationLanguageText_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_LANGUAGES",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApplicationLanguage_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ApplicationLanguage_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_IN_EVENT");
            DropTable("FMS_CL.EM_ICON_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_IconType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_ICON");
            DropTable("FMS_CL.EM_HAND_RECORD");
            DropTable("FMS_CL.EM_GLOBAL_VAR");
            DropTable("FMS_CL.EM_FUNCTION",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Function_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Function_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_FUNCTION_ROLE");
            DropTable("FMS_CL.EM_EXPORT_CONFIG");
            DropTable("FMS_CL.ABP_EDITIONS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Edition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_FEATURES",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TenantFeatureSetting_MustHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_ROLE_MODULE");
            DropTable("FMS_CL.EM_MODULE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Module_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_EXPORT_DATA");
            DropTable("FMS_CL.EM_DOWN_DATA");
            DropTable("FMS_CL.EM_DICTIONARY_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DictionaryType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_DICTIONARY");
            DropTable("FMS_CL.EM_DEFINE_CONFIG");
            DropTable("FMS_CL.EM_CONTENT_USER");
            DropTable("FMS_CL.ABP_ROLES",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Role_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Role_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONTENT_ROLE");
            DropTable("FMS_CL.EM_CONTENT_REPLY_FILE");
            DropTable("FMS_CL.EM_CONTENT_REPLY");
            DropTable("FMS_CL.EM_CONTENT_TAG",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContentTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONTENT_REF_TAG");
            DropTable("FMS_CL.EM_CONTENT_READ_LOG");
            DropTable("FMS_CL.EM_PUSH_WAY");
            DropTable("FMS_CL.EM_CONTENT_PUSH_WAY");
            DropTable("FMS_CL.EM_CONTENT_PRAISE_LOG");
            DropTable("FMS_CL.ABP_SETTINGS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Setting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USER_ROLES",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserRole_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_PERMISSIONS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_RolePermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserPermissionSetting_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USER_LOGINS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserLogin_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_DEPARTMENT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Department_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USER_CLAIMS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserClaim_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.ABP_USERS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_User_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONTENT_LOG");
            DropTable("FMS_CL.EM_CONTENT_FILE");
            DropTable("FMS_CL.EM_DISTRICT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_District_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONTENT_DISTRICT");
            DropTable("FMS_CL.EM_CONTENT_CHECK");
            DropTable("FMS_CL.EM_CONTENT_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContentType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_DEFINE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Define_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONTENT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Content_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CONNECT_LINE_FORCASE");
            DropTable("FMS_CL.EM_CONNECT_LINE");
            DropTable("FMS_CL.EM_CHART_TEMP");
            DropTable("FMS_CL.EM_REPORT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Report_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CHART_TYPE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ChartType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_CHART_REPORT",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ChartReport_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.AbpBackgroundJobs");
            DropTable("FMS_CL.ABP_AUDITLOGS",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AuditLog_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_OFFLINE_LOG");
            DropTable("FMS_CL.EM_IMP_TYPE");
            DropTable("FMS_CL.EM_REGULAR");
            DropTable("FMS_CL.EM_IMP_TB_FIELD");
            DropTable("FMS_CL.EM_IMP_TB_CASE");
            DropTable("FMS_CL.EM_DEFAULT_FIELD");
            DropTable("FMS_CL.EM_PRE_DATA_TYPE");
            DropTable("FMS_CL.EM_DB_TYPE");
            DropTable("FMS_CL.EM_DB_TAG",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DbTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_DB_SERVER",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DbServer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("FMS_CL.EM_IMP_TB");
            DropTable("FMS_CL.EM_IMPORT_LOG");
            DropTable("FMS_CL.EM_FILES");
            DropTable("FMS_CL.EM_APP_VERSION");
            DropTable("FMS_CL.EM_ANALYSIS");
        }
    }
}
