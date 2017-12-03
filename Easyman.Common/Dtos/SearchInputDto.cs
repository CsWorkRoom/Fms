#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：InputModel
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 16:13:36
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using EasyMan.EasyQuery;

#region 主体





namespace EasyMan.Dtos
{


    using System.Collections.Generic;
    using Abp.Application.Services.Dto;

    public class SearchInputDto : IEntityDto
    {
        public SearchInputDto()
        {
            SearchList = new List<SearchFilter>();
        }

        public int Id { get; set; }

        public virtual Pager Page { get; set; }

        public virtual Order Order { get; set; }

        public virtual IList<SearchFilter> SearchList { get; set; }
    }

    public class SearchInputDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public SearchInputDto()
        {
            SearchList = new List<SearchFilter>();
        }

        public TPrimaryKey Id { get; set; }

        public virtual Pager Page { get; set; }

        public virtual Order Order { get; set; }

        public virtual IList<SearchFilter> SearchList { get; set; }
    }
}
#endregion
