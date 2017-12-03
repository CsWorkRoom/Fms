using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    public class ExcelTop
    {
        public virtual string name { get; set; }

        public virtual int[] list { get; set; }
    }
}
