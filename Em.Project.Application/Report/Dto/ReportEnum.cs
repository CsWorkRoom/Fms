using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Easyman.Dto
{
    public class ReportEnum
    {
        /// <summary>
        /// 子报表类型
        /// </summary>
        public enum ReportType
        {
            [Description("表格报表")]
            Table = 1,
            [Description("键值报表")]
            KeyValue = 2,
            [Description("图形报表")]
            Chart = 3,
            [Description("RDLC报表")]
            Rdlc = 4
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public enum OutEventType
        {
            [Description("行事件")]
            RowEvent = 1,
            [Description("全局事件-表格外")]
            GlobalEvent = 2,
            [Description("内容事件")]
            ContentEvent = 3,
            [Description("列事件-字段")]
            FieldEvent = 4,
            [Description("列事件-多表头")]
            TopFielEvent = 5
        }
        /// <summary>
        /// 事件打开方式
        /// </summary>
        public enum EventOpenWay
        {
            [Description("弹出框（指定大小）")]
            ModeDialog = 1,
            [Description("顶级弹出框（指定大小）")]
            TopModelDialog = 2,
            [Description("当页跳转")]
            Jump = 3,
            [Description("新开网页")]
            NewJump = 4,
            [Description("新开Tab页")]
            Tab = 5,
            [Description("ajax执行")]
            Ajax = 6,
            [Description("弹出子页面")]
            ChildPage = 7
        }
        /// <summary>
        /// 筛选类型管理
        /// </summary>
        public enum FilterType
        {
            [Description("文本框")]
            Text = 1,
            [Description("复选下拉框")]
            CheckDropDown = 2,
            [Description("单选下拉框")]
            RadioDropDown = 3,
            [Description("年月日yyyy-mm-dd")]
            DataYYYYMMDD = 4,
            [Description("年月yyyy-mm")]
            DataYYYYMM = 5
        }
    }
}
