using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Easyman.Web.Models
{
    public class SubInfo
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long Size { get; set; }
    }
}