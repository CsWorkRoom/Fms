using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    public class OneScore
    {
        /// <summary>
        /// 月度指标明细编号
        /// </summary>
        public long? DetailId { get; set; }
        /// <summary>
        /// 领导打分值
        /// </summary>
        public double? MarkScore { get; set; }
        /// <summary>
        /// 最终得分
        /// </summary>
        public double? EndScore { get; set; }
    }
}
