using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    [AutoMap(typeof(Easyman.Domain.GlobalVar))]
    public class GlobalVarModel
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual string Remark { get; set; }
    }
}
