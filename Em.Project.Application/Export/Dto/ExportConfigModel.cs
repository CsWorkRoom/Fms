using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    [AutoMap(typeof(Easyman.Domain.ExportConfig))]
    public class ExportConfigModel
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 归属应用
        /// </summary>
        public virtual string App { get; set; }
        /// <summary>
        /// 导出的文件目录
        /// </summary>
        public virtual string Path { get; set; }
        /// <summary>
        /// 文件有效时间
        /// </summary>
        public virtual int? ValidDay { get; set; }
        /// <summary>
        /// 大文件定义
        /// </summary>
        public virtual int? DataSize { get; set; }
        /// <summary>
        /// 最大导出时长（秒）
        /// </summary>
        public virtual int? MaxTime { get; set; }
        /// <summary>
        /// 单文件最大行数
        /// </summary>
        public virtual int? MaxRowNum { get; set; }
        /// <summary>
        /// 抽样最大等待时长(秒)
        /// </summary>
        public virtual int? WaitTime { get; set; }
    }
}
