using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Easyman.Dto
{
    public  class ChildReportModel
    {
        /// <summary>
        /// 子报表的ID（0=新增；）
        /// </summary>
        public long ChildReportId { get; set; }
        /// <summary>
        /// 子报表类型（1=表格、2=键值、3=图形、4=RDLC）
        /// </summary>
        public short ChildReportType { get; set; }
        /// <summary>
        /// 使用端（APP、PC）
        /// </summary>
        public string ApplicationType { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 报表详细配置
        /// 不同类型报表不一样（表格、图形、RDLC）
        /// </summary>
        public string ChildReportJson { get; set; }
    }
}
