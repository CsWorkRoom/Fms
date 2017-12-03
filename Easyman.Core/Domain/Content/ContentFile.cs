using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Easyman.Common;
using Easyman.Domain;

namespace Easyman.Domain
{
    /// <summary>
    /// 内容与文件关联表
    /// </summary>
    [Table(SystemConfiguration.TablePrefix + "CONTENT_FILE")]
    public class ContentFile : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("FILE_ID")]
        public virtual long FileId { get; set; }

        [Column("CONTENT_ID")]
        public virtual long ContentId { get; set; }

        [ForeignKey("FileId")]
        public virtual Files Files { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        public ContentFile()
        {

        }

        public ContentFile(int fileId, int contentId)
        {
            FileId = fileId;
            ContentId = contentId;
        }
    }
}
