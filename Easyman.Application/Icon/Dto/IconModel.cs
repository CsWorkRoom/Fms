using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;


namespace Easyman.Dto
{
    public class IconModel
    {
        public string Value { get; set; }

        public string Type { get; set; }
    }
    public class IconModels
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public long? TypeId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Icon))]
    public class IconInput : EntityDto<long>
    {
        /// <summary>
        /// 数据库类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 图标类型ID
        /// </summary>
        public virtual long? IconTypeId { get; set; }
        /// <summary>
        /// 图标名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标名称不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "图标名称")]
        public virtual string DisplyName { get; set; }
        /// <summary>
        /// 图标class
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标不能为空")]
        [System.ComponentModel.DataAnnotations.Display(Name = "图标")]
        public virtual string ClassName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
   
   

    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Icon))]
    public class IconOutput : EntityDto<long>
    {
        /// <summary>
        /// 数据库类型ID
        /// </summary>
        public new long Id { get; set; }
        /// <summary>
        /// 图标类型ID
        /// </summary>
        public virtual long? IconTypeId { get; set; }
        /// <summary>
        /// 图标名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标名称不能为空")]
        public virtual string DisplyName { get; set; }
        /// <summary>
        /// 图标class
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "图标不能为空")]
        public virtual string ClassName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IconSearchOutput : SearchOutputDto<IconOutput, long> { }
    /// <summary>
    /// 
    /// </summary>
    public class IconSearchInput : SearchInputDto { }
}
