using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Authorization.Roles;
using Easyman.Common;
using Easyman.Domain;
using EasyMan.Dtos;

namespace Easyman.Content.Dto
{
    [AutoMapFrom(typeof(Domain.Content))]
    public class ContentIndexInput : CommonEntityHelper
    {
       
        public  int Id { get; set; }
       

        /// <summary>
        /// 内容定义ID
        /// </summary>
        public long DefineId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        public  ContentType ContentType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
         [Display(Name = "标题")]
        [Required(ErrorMessage = "标题不能为空")]
        public  string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [Required(ErrorMessage = "摘要不能为空")]
        public  string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "内容不能为空")]
        public  string Info { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public  string Image { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 创建人时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 重要
        /// </summary>
        public  long IsImport { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public  DateTime? BeginTime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public  DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public  bool IsUse { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// 类容定义ID
        /// </summary>
        public int ContentDefineId { get; set; }

        /// <summary>
        /// 指定用户Id，以"," 隔开
        /// </summary>
        public string UserListId { get; set; }

        /// <summary>
        /// 指定用户姓名，以"," 隔开
        /// </summary>
        public string UserListName { get; set; }
        
        
        /// <summary>
        /// 指定角色集合
        /// </summary>
        public List<RoleTemp> Role { get; set; }
        /// <summary>
        /// 限制角色集合
        /// </summary>
        public List<RoleTemp> RoleNo { get; set; }


        /// <summary>
        /// 指定组织集合
        /// </summary>
        public List<DistrictTemp> District { get; set; }
        /// <summary>
        /// 限制组织集合
        /// </summary>
        public List<DistrictTemp> DistrictNo { get; set; }
        /// <summary>
        /// 指定组织Id以"," 隔开
        /// </summary>
        public string DistrictListId { get; set; }

        /// <summary>
        /// 限制组织Id以"," 隔开
        /// </summary>
        public string DistrictListIdNo { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 限制名单,用户Id，以"," 隔开
        /// </summary>
        public string UserNameLimitList { get; set; }

        /// <summary>
        /// 限制用户姓名，以"," 隔开
        /// </summary>
        public string UserListNameNo { get; set; }
        /// <summary>
        /// 指定角色Id以"," 隔开
        /// </summary>
        public string RoleListId { get; set; }

        /// <summary>
        /// 限制角色Id以"," 隔开
        /// </summary>
        public string RoleListIdNo { get; set; }

        /// <summary>
        /// 是否为编辑
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadContent { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 内容点赞次数
        /// </summary>
        public int ContentPraiseCount { get; set; }
       
        /// <summary>
        /// 评论
        /// </summary>
        public List<ReplyModel> Reply { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        public bool IsReoly { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        public bool IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        public bool IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        public bool IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        public bool IsLike { get; set; }

      

        /// <summary>
        /// 允许删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// 内容是否已经点赞
        /// </summary>
        public bool IsOrLike { get; set; }

        /// <summary>
        /// 评论是否已经点赞
        /// </summary>
        public bool IsReolyOrLike { get; set; }

        /// <summary>
        /// 推送类型Id,多个以，号隔开
        /// </summary>
        public string PushId { get; set; }

        /// <summary>
        /// 推送类型
        /// </summary>
        public IList<PushTemp> Push { get; set; }

        /// <summary>
        /// 是否是选择的全部用户
        /// </summary>
        public bool IsAllUser { get; set; }
        /// <summary>
        /// 是否是选择的全部角色
        /// </summary>
        public bool IsAllRole { get; set; }
        /// <summary>
        /// 是否是选择的全部组织
        /// </summary>
        public bool IsAllDistrict { get; set; }

        /// <summary>
        /// 允许选择用户权限
        /// </summary>
        public bool IsCheckUser { get; set; }
        /// <summary>
        /// 允许选择角色权限
        /// </summary>
        public bool IsCheckRole { get; set; }
        /// <summary>
        /// 允许选择组织权限
        /// </summary>
        public bool IsCheckDistrict { get; set; }

        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        public bool IsContentFile { get; set; }

        
        /// <summary>
        /// 文件编号Id，以"," 隔开
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 指定上传附件集合
        /// </summary>
        public List<FileTemp> Files { get; set; }

        /// <summary>
        /// 评论Id,多个以，号隔开
        /// </summary>
        public string RealyLIstId { get; set; }

        /// <summary>
        /// 当前登录人用户名
        /// </summary>
        public string LonigName { get; set; }

        /// <summary>
        /// 部分评论（只显示前12条）
        /// </summary>
        public List<ReplyModel> ReplyNumber { get; set; }

        /// <summary>
        /// 是否显示全部评论
        /// </summary>
        public bool IsAllReplyNumber { get; set; }

    }

    [AutoMapFrom(typeof(Domain.Content))]
    public class ContentIndexOutput : EntityDto
    {

        public int Id { get; set; }


        /// <summary>
        /// 内容定义ID
        /// </summary>
        public long DefineId { get; set; }

        /// <summary>
        /// 建立主外键关系
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [Required(ErrorMessage = "摘要不能为空")]
        public string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "内容不能为空")]
        public string Info { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 创建人时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 重要
        /// </summary>
        public long IsImport { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// 类容定义ID
        /// </summary>
        public int ContentDefineId { get; set; }

        /// <summary>
        /// 指定用户Id，以"," 隔开
        /// </summary>
        public string UserListId { get; set; }

        /// <summary>
        /// 指定用户姓名，以"," 隔开
        /// </summary>
        public string UserListName { get; set; }


        /// <summary>
        /// 指定角色集合
        /// </summary>
        public List<RoleTemp> Role { get; set; }
        /// <summary>
        /// 限制角色集合
        /// </summary>
        public List<RoleTemp> RoleNo { get; set; }


        /// <summary>
        /// 指定组织集合
        /// </summary>
        public List<DistrictTemp> District { get; set; }
        /// <summary>
        /// 限制组织集合
        /// </summary>
        public List<DistrictTemp> DistrictNo { get; set; }
        /// <summary>
        /// 指定组织Id以"," 隔开
        /// </summary>
        public string DistrictListId { get; set; }

        /// <summary>
        /// 限制组织Id以"," 隔开
        /// </summary>
        public string DistrictListIdNo { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 限制名单,用户Id，以"," 隔开
        /// </summary>
        public string UserNameLimitList { get; set; }

        /// <summary>
        /// 限制用户姓名，以"," 隔开
        /// </summary>
        public string UserListNameNo { get; set; }
        /// <summary>
        /// 指定角色Id以"," 隔开
        /// </summary>
        public string RoleListId { get; set; }

        /// <summary>
        /// 限制角色Id以"," 隔开
        /// </summary>
        public string RoleListIdNo { get; set; }

        /// <summary>
        /// 是否为编辑
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadContent { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 内容点赞次数
        /// </summary>
        public int ContentPraiseCount { get; set; }

        /// <summary>
        /// 所有评论
        /// </summary>
        public List<ReplyModel> Reply { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        public bool IsReoly { get; set; }
        /// <summary>
        /// 允许评论上传附件
        /// </summary>
        public bool IsReolyFile { get; set; }

        /// <summary>
        /// 允许回复评论
        /// </summary>
        public bool IsReolyFloor { get; set; }

        /// <summary>
        /// 允许回复评论上传附件
        /// </summary>
        public bool IsReolyFloorFile { get; set; }

        /// <summary>
        /// 允许评论使用富文本
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// 允许点赞
        /// </summary>
        public bool IsLike { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 允许转发
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// 内容是否已经点赞
        /// </summary>
        public bool IsOrLike { get; set; }

        /// <summary>
        /// 评论是否已经点赞
        /// </summary>
        public bool IsReolyOrLike { get; set; }

        /// <summary>
        /// 推送类型Id,多个以，号隔开
        /// </summary>
        public string PushId { get; set; }

        /// <summary>
        /// 推送类型
        /// </summary>
        public IList<PushTemp> Push { get; set; }

        /// <summary>
        /// 是否是选择的全部用户
        /// </summary>
        public bool IsAllUser { get; set; }
        /// <summary>
        /// 是否是选择的全部角色
        /// </summary>
        public bool IsAllRole { get; set; }
        /// <summary>
        /// 是否是选择的全部组织
        /// </summary>
        public bool IsAllDistrict { get; set; }

        /// <summary>
        /// 允许选择用户权限
        /// </summary>
        public bool IsCheckUser { get; set; }
        /// <summary>
        /// 允许选择角色权限
        /// </summary>
        public bool IsCheckRole { get; set; }
        /// <summary>
        /// 允许选择组织权限
        /// </summary>
        public bool IsCheckDistrict { get; set; }


        /// <summary>
        /// 允许内容上传附件
        /// </summary>
        public bool IsContentFile { get; set; }

        /// <summary>
        /// 文件编号Id，以"," 隔开
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 指定上传附件集合
        /// </summary>
        public List<FileTemp> Files { get; set; }


        /// <summary>
        /// 评论Id,多个以，号隔开
        /// </summary>
        public string RealyLIstId { get; set; }

        /// <summary>
        /// 当前登录人用户名
        /// </summary>
        public string LonigName { get; set; }

        /// <summary>
        /// 部分评论（只显示前12条）
        /// </summary>
        public List<ReplyModel> ReplyNumber { get; set; }

        /// <summary>
        /// 是否显示全部评论
        /// </summary>
        public bool IsAllReplyNumber { get; set; }
    }
    public class ContentIndexSearchInput : SearchInputDto
    {
        /// <summary>
        /// 用于菜单配置的内容定义编码参数，该参数用于查询内容定义编码所对应的内容
        /// </summary>
        public string Code { get; set; }

        public string SearchName { get; set; }

    }

    public class ContentIndexSearchOutput : SearchOutputDto<ContentIndexOutput>
    {
        public override Pager Page
        {
            get;
            set;
        }

        public override IEnumerable<ContentIndexOutput> Datas
        {
            get;
            set;
        }

        
    }

    /// <summary>
    /// 推送类型
    /// </summary>
    public class PushTemp
    {
        /// <summary>
        /// 推送Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 推送名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsCheck { get; set; }
    }

    /// <summary>
    /// 角色权限
    /// </summary>
    public class RoleTemp
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsCheck { get; set; }
    }
    /// <summary>
    /// 组织权限
    /// </summary>
    public class DistrictTemp
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsCheck { get; set; }
    }
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserTemp
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsCheck { get; set; }
    }
    /// <summary>
    /// 上传附件
    /// </summary>
    public class FileTemp
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime Uptime { get; set; }
        /// <summary>
        /// 上传路径
        /// </summary>
        public string Upurl { get; set; }

        /// <summary>
        /// 文件大小转换为ＫＢ
        /// </summary>
        public string LengthKb { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public long RealyId { get; set; }
    }
}
