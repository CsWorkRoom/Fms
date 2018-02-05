namespace Easyman.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class initfms9 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("FMS_CL.GP_MONTH_BILL_LOG", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            //DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            //DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            //DropForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            //DropForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            //DropForeignKey("FMS_CL.GP_SUBITEM", "SUBITEM_TYPE_ID", "FMS_CL.GP_SUBITEM_TYPE");
            //DropForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "SUBITEM_ID", "FMS_CL.GP_SUBITEM");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_TARGET_ID", "FMS_CL.GP_MONTH_TARGET");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL");
            //DropForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "TARGET_FORMULA_ID", "FMS_CL.GP_TARGET_FORMULA");
            //DropForeignKey("FMS_CL.GP_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            //DropForeignKey("FMS_CL.GP_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE");
            //DropForeignKey("FMS_CL.GP_TARGET_VALUE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT");
            //DropForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_ID", "FMS_CL.GP_TARGET");
            //DropForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG");
            //DropIndex("FMS_CL.GP_MONTH_BILL_LOG", new[] { "MONTH_BILL_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "MONTH_BILL_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "TARGET_TYPE_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET", new[] { "TARGET_TAG_ID" });
            //DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "MONTH_BILL_ID" });
            //DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "TARGET_TAG_ID" });
            //DropIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", new[] { "DISTRICT_ID" });
            //DropIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", new[] { "SUBITEM_ID" });
            //DropIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", new[] { "DISTRICT_ID" });
            //DropIndex("FMS_CL.GP_SUBITEM", new[] { "SUBITEM_TYPE_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "MONTH_BILL_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "MONTH_TARGET_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "TARGET_TYPE_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "TARGET_TAG_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", new[] { "DISTRICT_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", new[] { "MONTH_BILL_ID" });
            //DropIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", new[] { "TARGET_FORMULA_ID" });
            //DropIndex("FMS_CL.GP_TARGET", new[] { "TARGET_TYPE_ID" });
            //DropIndex("FMS_CL.GP_TARGET", new[] { "TARGET_TAG_ID" });
            //DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "TARGET_ID" });
            //DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "TARGET_TAG_ID" });
            //DropIndex("FMS_CL.GP_TARGET_VALUE", new[] { "DISTRICT_ID" });
            //DropTable("FMS_CL.GP_MANAGER");
            //DropTable("FMS_CL.GP_MONTH_BILL");
            //DropTable("FMS_CL.GP_MONTH_BILL_LOG");
            //DropTable("FMS_CL.GP_MONTH_TARGET");
            //DropTable("FMS_CL.GP_TARGET_TAG",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_TargetTag_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("FMS_CL.GP_TARGET_TYPE",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_TargetType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("FMS_CL.GP_MONTH_BONUS");
            //DropTable("FMS_CL.GP_MONTH_BONUS_DETAIL");
            //DropTable("FMS_CL.GP_MONTH_SUBITEM_SCORE");
            //DropTable("FMS_CL.GP_SUBITEM");
            //DropTable("FMS_CL.GP_SUBITEM_TYPE",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_SubitemType_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("FMS_CL.GP_MONTH_TARGET_DETAIL");
            //DropTable("FMS_CL.GP_MONTH_TARGET_FORMULA",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_MonthTargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("FMS_CL.GP_TARGET_FORMULA",
            //    removedAnnotations: new Dictionary<string, object>
            //    {
            //        { "DynamicFilter_TargetFormula_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
            //    });
            //DropTable("FMS_CL.GP_TARGET");
            //DropTable("FMS_CL.GP_TARGET_VALUE");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
            
            CreateIndex("FMS_CL.GP_TARGET_VALUE", "DISTRICT_ID");
            CreateIndex("FMS_CL.GP_TARGET_VALUE", "TARGET_TAG_ID");
            CreateIndex("FMS_CL.GP_TARGET_VALUE", "TARGET_ID");
            CreateIndex("FMS_CL.GP_TARGET", "TARGET_TAG_ID");
            CreateIndex("FMS_CL.GP_TARGET", "TARGET_TYPE_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", "TARGET_FORMULA_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_FORMULA", "MONTH_BILL_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", "DISTRICT_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TAG_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TYPE_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_TARGET_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_BILL_ID");
            CreateIndex("FMS_CL.GP_SUBITEM", "SUBITEM_TYPE_ID");
            CreateIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", "DISTRICT_ID");
            CreateIndex("FMS_CL.GP_MONTH_SUBITEM_SCORE", "SUBITEM_ID");
            CreateIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", "DISTRICT_ID");
            CreateIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", "TARGET_TAG_ID");
            CreateIndex("FMS_CL.GP_MONTH_BONUS_DETAIL", "MONTH_BILL_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET", "TARGET_TAG_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET", "TARGET_TYPE_ID");
            CreateIndex("FMS_CL.GP_MONTH_TARGET", "MONTH_BILL_ID");
            CreateIndex("FMS_CL.GP_MONTH_BILL_LOG", "MONTH_BILL_ID");
            AddForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG", "ID");
            AddForeignKey("FMS_CL.GP_TARGET_VALUE", "TARGET_ID", "FMS_CL.GP_TARGET", "ID");
            AddForeignKey("FMS_CL.GP_TARGET_VALUE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT", "ID");
            AddForeignKey("FMS_CL.GP_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE", "ID");
            AddForeignKey("FMS_CL.GP_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "TARGET_FORMULA_ID", "FMS_CL.GP_TARGET_FORMULA", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_FORMULA", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_TARGET_ID", "FMS_CL.GP_MONTH_TARGET", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "SUBITEM_ID", "FMS_CL.GP_SUBITEM", "ID");
            AddForeignKey("FMS_CL.GP_SUBITEM", "SUBITEM_TYPE_ID", "FMS_CL.GP_SUBITEM_TYPE", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_SUBITEM_SCORE", "DISTRICT_ID", "FMS_CL.EM_DISTRICT", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_BONUS_DETAIL", "DISTRICT_ID", "FMS_CL.EM_DISTRICT", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TYPE_ID", "FMS_CL.GP_TARGET_TYPE", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET", "TARGET_TAG_ID", "FMS_CL.GP_TARGET_TAG", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_TARGET", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL", "ID");
            AddForeignKey("FMS_CL.GP_MONTH_BILL_LOG", "MONTH_BILL_ID", "FMS_CL.GP_MONTH_BILL", "ID");
        }
    }
}
