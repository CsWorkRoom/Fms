using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：回复
    /// </summary>
    public class ApiContentReviewBean
    {
        public long ID { get; set; }

        public long CONTENT_ID { get; set; }

        public long PARENT_ID { get; set; }

        public string INFO { get; set; }

        public long REPLY_UID { get; set; }

        public string ReplyUserName { get; set; }

        public string ReplyUserIco { get; set; }

        public DateTime CreateTime { get; set; }

        public List<ApiContentReviewBean> ChildReview { get; set; }

        public long ChildReviewNum { get; set; }
    }
}
