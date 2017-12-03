using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Easyman.Dto
{
    public class MarkScore
    {
        public string Month { get; set; }
        public virtual long? TargetTypeId { get; set; }
        /// <summary>
        /// 指标标识ID
        /// </summary>
        public virtual long? TargetTagId { get; set; }
        /// 指标ID
        /// </summary>
        public virtual long? TargetId { get; set; }

        /// 层级
        /// </summary>
        public virtual long? DistrictId { get; set; }

        /// <summary>
        /// 客户经理编号
        /// </summary>
        public virtual string ManagerNo { get; set; }
        /// <summary>
        /// 客户经理名称
        /// </summary>
        public virtual string ManagerName { get; set; }

        /// <summary>
        /// 指标类型列表
        /// </summary>
        public List<SelectListItem> TargetTypeList { get; set; }
        /// <summary>
        /// 指标列表
        /// </summary>
        public List<SelectListItem> TargetList { get; set; }
    }
}
