using System.Web;
using System.Web.Optimization;

namespace Easyman.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            #region jquery相关  ~/Bundles/jquery/js
            bundles.Add(
            new ScriptBundle("~/Bundles/jquery/js")
                .Include("~/Common/Scripts/custom/aes.js")
                .Include("~/Scripts/jquery-2.2.4.js")
                .Include("~/Scripts/jquery-ui-1.11.4.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.blockUI.js")
                .Include("~/Scripts/jquery.form.js")//表单验证
                .Include("~/Scripts/jquery.signalR-2.2.1.js")
            );
            #endregion

            #region bootstrap相关
            #region ~/Bundles/bootstrap/js
            bundles.Add(
               new ScriptBundle("~/Bundles/bootstrap/js")
                    .Include("~/Scripts/bootstrap.js")
                    .Include("~/Scripts/bootstrapValidator.js")
                    .Include("~/Scripts/bootstrap-typeahead.js")
                    );
            #endregion

            #region ~/Bundles/bootstrap/css
            bundles.Add(
            new StyleBundle("~/Bundles/bootstrap/css")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                );
            #endregion
            #endregion

            #region ~/Bundles/App/main/js （停止使用）
            bundles.Add(
               new ScriptBundle("~/Bundles/App/main/js")
               // .Include("~/Scripts/modernizr-2.8.3.js")
               // .Include("~/Abp/ie10fix.js")
               // .Include("~/Scripts/json2.js")
               //.Include("~/Scripts/bootstrap.js")
               //.Include("~/Scripts/bootstrapValidator.js")
               //.Include("~/Scripts/bootstrap-typeahead.js")

               //.Include("~/Scripts/moment-with-locales.js")//轻量的日期控件
               //.Include("~/Scripts/toastr.js")
               //.Include("~/Scripts/sweetalert/sweet-alert.js")
               //.Include("~/Scripts/others/spinjs/spin.js")
               //.Include("~/Scripts/others/spinjs/jquery.spin.js")
               //.Include("~/Abp/abp.js")
               //.Include("~/Abp/abp.jquery.js")
               //.Include("~/Abp/abp.toastr.js")
               //.Include("~/Abp/abp.blockUI.js")
               //.Include("~/Abp/abp.spin.js")
               //.Include("~/Abp/abp.sweet-alert.js")
               //.Include("~/Scripts/vue.js")
               //.Include("~/Scripts/lodash.js")

               // .Include("~/Scripts/components/ztable/ztable.js")
               // .Include("~/Scripts/components/ztree/jquery.ztree.js")

               //.Include("~/Scripts/components/modal.js")
               //.Include("~/Scripts/components/control-extensions.js")
               //.Include("~/Scripts/components/jquery.fileDownload.js")
               //.Include("~/Scripts/components/icheck/icheck.js")
               //.Include("~/fonts/iconfont/iconfont.js")
               // .Include("~/Common/Scripts/custom/abpHelpers.js")

               //.Include("~/Common/rootUrl.js")
               //.Include("~/Common/Scripts/errorPage/error.js")
               //.Include("~/Common/Scripts/PageMessage/PageMessage.js")
               //.Include("~/Common/Scripts/custom/main.js")
               //.Include("~/Common/Scripts/custom/cache.js")
               //.Include("~/Common/Scripts/custom/boot.js")
               //.Include("~/Common/Scripts/custom/openPage.js")
               //.Include("~/Common/Scripts/custom/tableSearch.js")
               );
            #endregion

            #region ~/Bundles/index/js 主页面index特有
            bundles.Add(
               new ScriptBundle("~/Bundles/index/js")
                    .Include("~/Common/Scripts/custom/main.js")
                    );
            #endregion

            #region ~/Bundles/bootBack/js 校验页面权限+绘制水印
            bundles.Add(
               new ScriptBundle("~/Bundles/bootBack/js")
                    .Include("~/Common/Scripts/custom/bootBack.js")
                    );
            #endregion            

            #region 公共的插件common
            #region ~/Bundles/common/js
            bundles.Add(
               new ScriptBundle("~/Bundles/common/js")
                    .Include("~/Common/rootUrl.js")
                    .Include("~/Common/Scripts/errorPage/error.js")
                    .Include("~/Common/Scripts/PageMessage/PageMessage.js")
                    .Include("~/Common/Scripts/custom/cache.js")
                    .Include("~/Common/Scripts/custom/boot.js")
                    .Include("~/Common/Scripts/custom/openPage.js")
                    .Include("~/Common/Scripts/custom/tableSearch.js")
                    );
            #endregion

            #region ~/Bundles/common/css
            bundles.Add(
               new StyleBundle("~/Bundles/common/css")
                   .Include("~/Content/bootstrapValidator.css", new CssRewriteUrlTransform())//表单验证
                   .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform())//国旗图标
                   .Include("~/Common/css/content.css", new CssRewriteUrlTransform())//主要针对ztree兼容处理
               );
            #endregion
            #endregion

            #region ~/Bundles/controller/js
            bundles.Add(
              new ScriptBundle("~/Bundles/controller/js")
                    .Include("~/Scripts/json2.js")
                    //.Include("~/Scripts/modernizr-2.8.3.js")
                    .Include("~/Scripts/moment-with-locales.js")//轻量的日期控件
                    .Include("~/Scripts/others/spinjs/spin.js")
                    .Include("~/Scripts/others/spinjs/jquery.spin.js")
                    .Include("~/Scripts/components/modal.js")
                    .Include("~/Scripts/components/control-extensions.js")
                    .Include("~/Scripts/components/jquery.fileDownload.js")
                    .Include("~/Scripts/lodash.js")
              );
            #endregion

            #region Abp框架包 ~/Bundles/abp/js
            bundles.Add(
                new ScriptBundle("~/Bundles/abp/js")
                   .Include("~/Abp/ie10fix.js")
                   .Include("~/Abp/abp.js")
                   .Include("~/Abp/abp.jquery.js")
                   .Include("~/Scripts/toastr.js")
                   .Include("~/Abp/abp.toastr.js")
                   .Include("~/Abp/abp.blockUI.js")
                   .Include("~/Abp/abp.spin.js")
                   .Include("~/Abp/abp.sweet-alert.js")
                   .Include("~/Common/Scripts/custom/abpHelpers.js")
                   .Include("~/Scripts/vue.js")
           );
            #endregion

            #region 通知插件
            #region ~/Bundles/toastr/js
            bundles.Add(
               new ScriptBundle("~/Bundles/toastr/js")
                    .Include("~/Scripts/toastr.js")
                    );
            #endregion

            #region ~/Bundles/toastr/css
            bundles.Add(
                new StyleBundle("~/Bundles/toastr/css")
                    .Include("~/Content/toastr.css", new CssRewriteUrlTransform())//通知插件 
                    );
            #endregion
            #endregion

            #region 复选框插件
            #region ~/Bundles/icheck/js
            bundles.Add(
               new ScriptBundle("~/Bundles/icheck/js")
                    .Include("~/Scripts/components/icheck/icheck.js")
                    );
            #endregion

            #region ~/Bundles/icheck/css
            bundles.Add(
                new StyleBundle("~/Bundles/icheck/css")
                    .Include("~/Scripts/components/icheck/flat/blue.css", new CssRewriteUrlTransform())//复选框   
                    );
            #endregion
            #endregion
            
            #region 树插件
            bundles.Add(
               new ScriptBundle("~/Bundles/ztree/js")
                     .Include("~/Scripts/components/ztree/jquery.ztree.js")
                    );
            bundles.Add(
                new StyleBundle("~/Bundles/ztree/css")
                     .Include("~/Scripts/components/ztree/zTreeStyle/zTreeStyle.css", new CssRewriteUrlTransform())//树
                    );
            #endregion

            #region ztable
            //bundles.Add(
            //   new ScriptBundle("~/Bundles/ztable/js")
            //         .Include("~/Scripts/components/ztable/ztable.js")
            //        );
            //bundles.Add(
            //    new StyleBundle("~/Bundles/ztable/css")
            //         .Include("~/Scripts/components/ztable/style/ztable.css", new CssRewriteUrlTransform())
            //        );
            #endregion

            #region 消息弹框插件
            bundles.Add(
               new ScriptBundle("~/Bundles/alert/js")
                    .Include("~/Scripts/sweetalert/sweet-alert.js")
                    );
            bundles.Add(
                new StyleBundle("~/Bundles/alert/css")
                    .Include("~/Scripts/sweetalert/sweet-alert.css", new CssRewriteUrlTransform())//消息弹框插件
                    );
            #endregion

            #region 滚动条插件
            bundles.Add(
               new ScriptBundle("~/Bundles/Scroll/js")
                     .Include("~/Common/Scripts/customerScrollbar/jquery.mousewheel.js")
                     .Include("~/Common/Scripts/customerScrollbar/jquery.mCustomScrollbar.js")
                    );

            bundles.Add(
                new StyleBundle("~/Bundles/Scroll/css")
                    .Include("~/Common/Scripts/customerScrollbar/jquery.mCustomScrollbar.css", new CssRewriteUrlTransform())
                    );
            #endregion

            #region handsontable
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/App/handsontable/css")
                    .Include("~/Common/css/handsontable/handsontable.full.css", new CssRewriteUrlTransform())
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/App/handsontable/js")
                    .Include("~/Common/Scripts/handsontable/handsontable.full.js")
                    .Include("~/Common/Scripts/handsontable/moment.js")
                    );

            #endregion

            #region calendar 日期插件

            //bundles.Add(
            //    new ScriptBundle("~/Bundles/App/calendar/js")
            //        .Include("~/Common/Scripts/calendar/WdatePicker.js")
            //        .Include("~/Common/Scripts/calendar/lang/zh-cn.js")
            //        );

            #endregion

            #region multiple-select 下拉多选插件
            //css
            bundles.Add(
                new StyleBundle("~/Common/css/multipleselect/css")
                 .Include("~/Common/css/multipleselect/multipleselect.css")//下拉框
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/App/multipleselect/js")
                    .Include("~/Common/Scripts/multipleselect/multipleselect.js")
                    );

            #endregion
            
            #region jqgrid相关
            #region ~/Bundles/App/jqgrid/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/jqgrid/css")
                    .Include("~/Content/ui.jqgrid-bootstrap.css", new CssRewriteUrlTransform())
                    .Include("~/Common/css/jqgridTable.css", new CssRewriteUrlTransform())
                    );
            #endregion

            #region ~/Bundles/App/jqgrid/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/jqgrid/js")
                    .Include("~/Scripts/i18n/grid.locale-cn.js")
                    .Include("~/Scripts/jquery.jqGrid.js")
                    .Include("~/Common/Scripts/custom/JqGridWinSet.js")
                    );
            #endregion
            #endregion
            
            #region EditReport 编辑报表
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/App/editReport/css")
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/App/editReport/js")
                    .Include("~/Views/Report/js/EditReport/MainReport.js")
                    .Include("~/Views/Report/js/EditReport/TbMain.js")
                    .Include("~/Views/Report/js/EditReport/TbField.js")
                    .Include("~/Views/Report/js/EditReport/TbEvent.js")
                    .Include("~/Views/Report/js/EditReport/TbTopField.js")
                    .Include("~/Views/Report/js/EditReport/Filter.js")
                    .Include("~/Views/Report/js/EditReport/RdlcMain.js")
                    .Include("~/Views/Report/js/EditReport/ChartMain.js")
                    );

            #endregion

            #region TbReport 表格报表
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/App/TbReport/css")
                    .Include("~/Common/css/TbReport.css", new CssRewriteUrlTransform())
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/App/TbReport/js")
                    .Include("~/Views/Report/js/TbReport.js")
                    );
            #endregion

            #region RdlcReport RDLC报表
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/App/RdlcReport/css")
                    .Include("~/Common/css/RdlcReport.css", new CssRewriteUrlTransform())
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/App/RdlcReport/js")
                    .Include("~/Views/Report/js/RdlcReport.js")
                    );
            #endregion


            #region txtIDE txtIDE文本编辑器
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/txtIDE/css")
                    .Include("~/Common/css/txtIDE/txtIDE.css", new CssRewriteUrlTransform())
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/txtIDE/js")
                    .Include("~/Common/Scripts/txtIDE/txtIDE.js")
                    );
            #endregion

            #region 右键内容插件 ContextMenu
            //css
            bundles.Add(
                new StyleBundle("~/Bundles/ContextMenu/css")
                    .Include("~/Content/jquery.contextMenu.css", new CssRewriteUrlTransform())
                    );
            //script
            bundles.Add(
                new ScriptBundle("~/Bundles/ContextMenu/js")
                    .Include("~/Scripts/jquery.contextMenu.js")
                    );
            #endregion

            //启用压缩：true 或 删除该行代码
            //禁用压缩：false
            BundleTable.EnableOptimizations = false;
        }
    }
}