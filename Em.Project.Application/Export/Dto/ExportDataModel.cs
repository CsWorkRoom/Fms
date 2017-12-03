using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    [AutoMap(typeof(Easyman.Domain.ExportData))]
    public class ExportDataModel
    {

        public virtual long? Id { get; set; }
        /// <summary>
        /// 模版页ID
        /// </summary>
        public virtual long? ModuleId { get; set; }
        /// <summary>
        /// 报表代码CODE
        /// </summary>
        public virtual string ReportCode { get; set; }
        /// <summary>
        /// 事件来自哪个url
        /// </summary>
        public virtual string FromUrl { get; set; }
        /// <summary>
        /// 导出发起人
        /// </summary>
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 导出类型
        /// </summary>
        public virtual string ExportWay { get; set; }
        /// <summary>
        /// 文件显示名（动态生成）
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 执行sql（解析之后的）
        /// </summary>
        public virtual string Sql { get; set; }
        /// <summary>
        /// 执行库
        /// </summary>
        public virtual long? DbServerId { get; set; }
        /// <summary>
        /// 多表头信息
        /// </summary>
        public virtual object TopFields { get; set; }
        /// <summary>
        /// 多表头信息字符串格式
        /// </summary>
        public virtual string ColumnHeader { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public virtual string FilePath { get; set; }
        /// <summary>
        /// 生成状态（生成中/生成成功/生成失败）
        /// </summary>
        public virtual string Status { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual int? FileSize { get; set; }
        /// <summary>
        /// 文件格式
        /// </summary>
        public virtual string FileFormat { get; set; }

        public virtual long? FilesId { get; set; }
        /// <summary>
        /// 生成开始时间
        /// </summary>
        public virtual DateTime? BeginTime { get; set; }
        /// <summary>
        /// 生成结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }
        /// <summary>
        /// 有效时间(天)
        /// </summary>
        public virtual int? ValidDay { get; set; }
        /// <summary>
        /// 是否失效
        /// </summary>
        public virtual bool? IsInvalid { get; set; }
        /// <summary>
        /// 是否关闭下载
        /// </summary>
        public virtual bool? IsClose { get; set; }
        /// <summary>
        /// 关闭人
        /// </summary>
        public virtual long? Closer { get; set; }
        /// <summary>
        /// 关闭时间
        /// </summary>
        public virtual DateTime? CloseTime { get; set; }
        /// <summary>
        /// 对像参数
        /// </summary>
        public virtual object ObjParam { get; set; }
    }
}
