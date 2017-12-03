using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.App.Dto
{
    /// <summary>
    /// API对象：用户信息
    /// </summary>
    public class ApiUserBean
    {
        #region 用户基本信息

        /// <summary>
        /// 登录成功后的授权码
        /// </summary>
        public string authToken { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        public string iconURL { get; set; }

        #endregion

        #region 用户角色、归属、所属区域等信息

        /// <summary>
        /// 用户归属
        /// </summary>
        public string belonging { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// 管理区域
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 所属区域ID
        /// </summary>
        public int distictId { get; set; }

        /// <summary>
        /// 所属区域名称
        /// </summary>
        public string distictName { get; set; }

        /// <summary>
        /// 所属区域层级(1市 ,2区县，3片区，4乡镇等等)
        /// </summary>
        public int distictLevel { get; set; }

        #endregion

        #region 用户功能菜单配置项

        /// <summary>
        /// 首页背景图地址
        /// </summary>
        public string indexPic { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public IList<ApiMenuBean> menu { get; set; }

        /// <summary>
        /// 首页菜单
        /// </summary>
        public ApiMenuBean defaultAct { get; set; }

        #endregion
    }
}
