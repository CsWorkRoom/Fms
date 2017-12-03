using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Common;
using Easyman.Domain;
using EasyMan.Dtos;

namespace Easyman.Content.Dto
{
    [AutoMapFrom(typeof(ContentType))]
    public class ContentTypeInput : CommonEntityHelper
    {
        public  int Id { get; set; }

        /// <summary>
        /// 内容定义ID
        /// </summary>
        public  int DefineId { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        public string DefineName { get; set; }
        

        /// <summary>
        /// 类别名
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不能为空")]
        public  string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public  int ParentId { get; set; }

        /// <summary>
        /// 父节名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 层次编码
        /// </summary>
        public  int PathId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public  int Level { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public  int ShowOrder { get; set; }

        /// <summary>
        /// 是否为编辑
        /// </summary>
        public bool IsEdit { get; set; }


        public int PIdType { get; set; }
    }

    [AutoMapFrom(typeof(ContentType))]
    public class ContentTypeOutput : EntityDto
    {
        public  int Id { get; set; }
        
        /// <summary>
        /// 内容定义ID
        /// </summary>
        public int ContentDefineId { get; set; }

        /// <summary>
        /// 内容定义名称
        /// </summary>
        public string DefineName { get; set; }
        

        /// <summary>
        /// 类别名
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父节名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 层次编码
        /// </summary>
        public int PathId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int ShowOrder { get; set; }

        /// <summary>
        /// 是否为编辑
        /// </summary>
        public bool IsEdit { get; set; }


        public int PIdType { get; set; }
    }
    public class ContentTypeSearchInput : SearchInputDto
    {

    }

    public class ContentTypeSearchOutput : SearchOutputDto<ContentTypeOutput>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<ContentTypeOutput> Datas
        {
            get;
            set;
        }
    }
}
