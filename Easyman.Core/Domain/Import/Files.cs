using Abp.Domain.Entities;
using Easyman.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Domain
{
    [Table(SystemConfiguration.TablePrefix + "FILES")]
    public class Files : Entity<long>
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("NAME"), StringLength(50)]
        public virtual string Name { get; set; }

        [Column("TRUE_NAME"), StringLength(50)]
        public virtual string TrueName { get; set; }

        [Column("PATH"), StringLength(200)]
        public virtual string Path { get; set; }

        [Column("USER_ID")]
        public virtual long UserId { get; set; }

        [Column("LENGTH")]
        public virtual long Length { get; set; }

        [Column("UPLOAD_TIME")]
        public virtual DateTime UploadTime { get; set; }

        [Column("REMARK"), StringLength(200)]
        public virtual string Remark { get; set; }

        [Column("URL"),StringLength(254)]
        public virtual string Url { get; set; }

        [Column("FILE_TYPE"), StringLength(200)]
        public virtual string FileType { get; set; }

        public virtual ICollection<ImportLog> ImportLog { get; set; }

    }
}
