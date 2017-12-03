#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：SearchModel
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 16:18:20
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体

using System.Collections.Generic;
using Abp.Application.Services.Dto;



namespace EasyMan.Dtos
{
    public  class SearchOutputDto<T> : EntityDto where T : EntityDto
    {
        public virtual Pager Page { get; set; }

        public virtual IEnumerable<T> Datas { get; set; }
    }

    public class SearchOutputDto<T, TPrimaryKey> : EntityDto<TPrimaryKey> where T : EntityDto<TPrimaryKey>
    {
        public virtual Pager Page { get; set; }

        public virtual IEnumerable<T> Datas { get; set; }
    }
}
#endregion
