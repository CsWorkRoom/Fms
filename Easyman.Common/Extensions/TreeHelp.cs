using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMan.Extensions
{
    public class TreeHelp
    {
        public class TreeItem
        {
            public TreeItem()
            {
                Children = new List<TreeItem>();
                IsParent = "false";
            }
            
            public string Id { get; set; }
            /// <summary>
            /// 节点名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 编码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 等级
            /// </summary>
            public string Level { get; set; }
            /// <summary>
            /// 序列
            /// </summary>
            public string SeqNo { get; set; }
            /// <summary>
            /// 父级ID
            /// </summary>
            public string ParentId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Abbr { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string Descript { get; set; }
            /// <summary>
            /// 是否有下级
            /// </summary>
            public string IsParent { get; set; }
            /// <summary>
            /// 子节点集合
            /// </summary>
            public List<TreeItem> Children { get; set; }
        }


    }
}
