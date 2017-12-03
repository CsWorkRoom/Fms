using Easyman.Domain;
using System.Collections.Generic;

namespace Easyman.DefaultData
{
    public static class StaticDatas
    {
        public static class Host
        {
            public static readonly DefaultDataModel AdminRole = new DefaultDataModel("Admin", "超级管理员");

            public static readonly DefaultDataModel CrqRole = new DefaultDataModel("Admin", "管理员");

        }

        public static class Users
        {
            public static readonly DefaultDataModel AdminUser = new DefaultDataModel("Admin", "超级管理员");
            public static readonly DefaultDataModel CrqUser = new DefaultDataModel("Admin", "管理员");
        }

        public static class Tenants
        {
            public static readonly DefaultDataModel Tenant = new DefaultDataModel("Crq", "技术平台组");
        }

        public static class Departments
        {
            public static readonly DefaultDataModel DefaultDepartment = new DefaultDataModel("OOOO", "四川省公司");
            public static readonly DefaultDataModel CrqDepartment = new DefaultDataModel("OOO1", "德阳分公司");
        }

        public static class Navigations
        {
            public static List<Module> DefaultNavs
            {
                get
                {
                    const int tenantId = 1;
                    var result = new List<Module>
                    {
                        new Module//1
                        {
                            Name = "后台管理",
                            Code = "M0000",
                            Icon="fa fa-cog",
                            TenantId = tenantId
                        },
                        new Module//2
                        {
                            Name = "APP后台管理",
                            Code = "appBackM",
                            ApplicationType="APP",
                            TenantId = tenantId
                        },
                        new Module//3
                        {
                            Name = "系统管理",
                            Code = "M0001",
                            ShowOrder = 0,
                            ParentId = 1,
                            Icon="fa fa-cog",
                            TenantId = tenantId
                        },
                        new Module//4
                        {
                            Name = "数据库管理",
                            Code = "dbserverM",
                            ShowOrder = 1,
                            ParentId = 1,
                            Icon="fa fa-database",
                            TenantId = tenantId
                        },
                        new Module//5
                        {
                            Name = "任务管理",
                            Code = "taskM",
                            ShowOrder = 2,
                            ParentId = 1,
                            Icon="fa fa-registered",
                            TenantId = tenantId
                        },
                        new Module//6
                        {
                            Name = "内容管理",
                            Code = "contentM",
                            ShowOrder = 3,
                            ParentId = 1,
                            Icon="fa fa-paste",
                            TenantId = tenantId
                        },
                        new Module//7
                        {
                            Name = "导入管理",
                            Code = "importM",
                            ShowOrder = 4,
                            ParentId = 1,
                            Icon="fa fa-recycle",
                            TenantId = tenantId
                        },
                         new Module//8
                        {
                            Name = "字典管理",
                            Code = "Dictionary",
                            ShowOrder = 5,
                            ParentId = 1,
                            Icon="fa fa-book",
                            TenantId = tenantId
                        },

                        #region 系统管理-3
                        #region old
                        //new Module
                        //{
                        //    Name = "用户管理",
                        //    Code = "M0002",
                        //    Url = "Admin/User",
                        //    ShowOrder = 0,
                        //    ParentId = 3,
                        //    TenantId = tenantId
                        //},
                        //new Module
                        //{
                        //    Name = "菜单管理",
                        //    Code = "M0003",
                        //    Url = "Admin/NavigationPage",
                        //    ShowOrder = 1,
                        //    ParentId = 3,
                        //    TenantId = tenantId
                        //},
                        //new Module
                        //{
                        //    Name = "角色管理",
                        //    Code = "M0004",
                        //    Url = "Admin/RolePage",
                        //    ShowOrder = 2,
                        //    ParentId = 3,
                        //    TenantId = tenantId
                        //},
                        //new Module
                        //{
                        //    Name = "部门管理",
                        //    Code = "M0005",
                        //    Url = "Admin/DepartentPage",
                        //    ShowOrder = 3,
                        //    ParentId = 3,
                        //    TenantId = tenantId
                        //},
#endregion

                        new Module
                        {
                            Name = "部门管理_new",
                            Code = "department_new",
                            Url = "Report/TbReport?code=department_new",
                            ShowOrder = 4,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "报表管理_new",
                            Code = "report_new",
                            Url = "Report/TbReport?code=report_new",
                            ShowOrder = 5,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "用户管理_new",
                            Code = "user_new",
                            Url = "Report/TbReport?code=user_new",
                            ShowOrder = 6,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "菜单管理_new",
                            Code = "module_new",
                            Url = "Report/TbReport?code=module_new",
                            ShowOrder = 7,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "角色管理_new",
                            Code = "role_new",
                            Url = "Report/TbReport?code=role_new",
                            ShowOrder = 8,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "菜单管理APP_new",
                            Code = "moduleApp_new",
                            Url = "Report/TbReport?code=moduleApp_new",
                            ShowOrder = 9,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "图标类型管理_new",
                            Code = "iconType_new",
                            Url = "Report/TbReport?code=iconType_new",
                            ShowOrder = 10,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "图标管理_new",
                            Code = "icon_new",
                            Url = "Report/TbReport?code=icon_new",
                            ShowOrder = 11,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                         new Module
                        {
                            Name = "全局变量管理_new",
                            Code = "globalvar_new",
                            Url = "Report/TbReport?code=globalvar_new",
                            ShowOrder = 12,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "文件下载_new",
                            Code = "down_File",
                            Url = "Report/TbReport?code=down_File",
                            ShowOrder = 13,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "APP版本管理_new",
                            Code = "globalvar_new",
                            Url = "Report/TbReport?code=globalvar_new",
                            ShowOrder = 14,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "APP请求测试",
                            Code = "AppRequest",
                            Url = "AppRequest/Index",
                            ShowOrder = 15,
                            ParentId = 3,
                            TenantId = tenantId
                        },
                        #endregion

                        #region 数据库管理-4
                         new Module
                        {
                            Name = "数据库种类",
                            Code = "dbtype",
                            Url = "Report/TbReport?code=dbtype",
                            ShowOrder = 1,
                            ParentId = 4,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "数据库标识",
                            Code = "dbtag",
                            Url = "Report/TbReport?code=dbtag",
                            ShowOrder = 2,
                            ParentId = 4,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "数据库",
                            Code = "dbserver",
                            Url = "Report/TbReport?code=dbserver",
                            ShowOrder = 3,
                            ParentId = 4,
                            TenantId = tenantId
                        },
                        #endregion

                        #region 任务管理-5
                        new Module
                        {
                            Name = "任务组类型",
                            Code = "scriptType",
                            Url = "Report/TbReport?code=scriptType",
                            ShowOrder = 1,
                            ParentId = 5,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "任务类型",
                            Code = "scriptNodeType",
                            Url = "Report/TbReport?code=scriptNodeType",
                            ShowOrder = 2,
                            ParentId = 5,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "任务定义",
                            Code = "scriptNode",
                            Url = "Report/TbReport?code=scriptNode",
                            ShowOrder = 3,
                            ParentId = 5,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "任务组定义",
                            Code = "script",
                            Url = "Report/TbReport?code=script",
                            ShowOrder = 4,
                            ParentId = 5,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "运行监测",
                            Code = "scriptCase",
                            Url = "Report/TbReport?code=scriptCase",
                            ShowOrder = 5,
                            ParentId = 5,
                            TenantId = tenantId
                        },
                        #endregion

                        #region 内容管理-6  还缺标签管理
                        new Module
                        {
                            Name = "功能定义",
                            Code = "define",
                            Url = "Report/TbReport?code=define",
                            ShowOrder = 1,
                            ParentId = 6,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "内容类型管理",
                            Code = "contentType",
                            Url = "Report/TbReport?code=contentType",
                            ShowOrder = 2,
                            ParentId = 6,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "内容管理",
                            Code = "content",
                            Url = "Report/TbReport?code=content",
                            ShowOrder = 3,
                            ParentId = 6,
                            TenantId = tenantId
                        },
                         new Module
                        {
                            Name = "推送模式",
                            Code = "Pushway",
                            Url = "Report/TbReport?code=Pushway",
                            ShowOrder = 4,
                            ParentId = 6,
                            TenantId = tenantId
                        },
                          new Module
                        {
                            Name = "内容列表",
                            Code = "content_list",
                            Url = "Report/TbReport?code=content_list",
                            ShowOrder = 5,
                            ParentId = 6,
                            TenantId = tenantId
                        },
                        #endregion

                        #region 导入管理-7
                       
                        new Module
                        {
                            Name = "字段数据类型",
                            Code = "dataType",
                            Url = "Report/TbReport?code=dataType",
                            ShowOrder = 1,
                            ParentId = 7,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "外导分类管理",
                            Code = "impType",
                            Url = "Report/TbReport?code=impType",
                            ShowOrder = 2,
                            ParentId = 7,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "正则表达式管理",
                            Code = "regular",
                            Url = "Report/TbReport?code=regular",
                            ShowOrder = 3,
                            ParentId = 7,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "内置字段管理",
                            Code = "dfField",
                            Url = "Report/TbReport?code=dfField",
                            ShowOrder = 4,
                            ParentId = 7,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "外导表定义",
                            Code = "impDb",
                            Url = "Report/TbReport?code=impDb",
                            ShowOrder = 5,
                            ParentId = 7,
                            TenantId = tenantId
                        },
                        #endregion

                        #region 字典管理-8
                         new Module
                        {
                            Name = "字典类型",
                            Code = "DicType",
                            Url = "Report/TbReport?code=DicType",
                            ShowOrder = 1,
                            ParentId = 8,
                            TenantId = tenantId
                        },
                        new Module
                        {
                            Name = "字典管理",
                            Code = "DicManger",
                            Url = "Report/TbReport?code=DicManger",
                            ShowOrder = 2,
                            ParentId = 8,
                            TenantId = tenantId
                        },
                        
                        #endregion
                    };

                    return result;
                }
            }
        }

    }

    public class DefaultDataModel
    {
        public DefaultDataModel(string name, string displayname)
        {
            Name = name;
            DisPlayName = displayname;
        }

        public string Name { get; set; }

        public string DisPlayName { get; set; }
    }
}
