using System.Collections.Generic;
using Abp.AutoMapper;
using Easyman.Domain;

namespace Easyman.Dto
{
    [AutoMapFrom(typeof(Module))]
    public class MenuItem
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }

        public string Icon { get; set; }

        public List<MenuItem> Items { get; set; }

        public bool Selected { get; set; }

        public MenuItem Parent { get; set; }

        public long? ParentId { get; set; }
        /// <summary>
        /// 是否在用
        /// </summary>
        public virtual bool? IsUse { get; set; }
    }
}
