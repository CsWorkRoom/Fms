using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：菜单信息
    /// </summary>
    public class ApiMenuBean
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 类型（click: 事件、view:URL连接地址）
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// Type为click有效，表示是本地功能页面
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Type为view有效，表示是远程URL页面
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string ICON { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SHOW_ORDER { get; set; }

        /// <summary>
        /// 是否是首页
        /// </summary>
        public int isIndex { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public IList<ApiMenuBean> child { get; set; }

    }
}
