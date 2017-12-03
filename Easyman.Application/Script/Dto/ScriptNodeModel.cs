using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Domain;
using EasyMan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Easyman.Dto
{
    #region ScriptNodeType 节点类型

    [AutoMap(typeof( ScriptNodeType))]
    public class ScriptNodeTypeInput: EntityDto<long>
    {
        public new long Id { get; set; }

        [Required(ErrorMessage = "节点类型不能为空")]
        [Display(Name = "节点类型")]
        public virtual string Name { get; set; }

        [Display(Name = "备注")]
        public virtual string Remark { get; set; }
    }

    [AutoMap(typeof(ScriptNodeType))]
    public class ScriptNodeTypeOutput : EntityDto<long>
    {

        public new long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Remark { get; set; }

    }

    public class ScriptNodeTypeSearchInput : SearchInputDto<long>
    {
    }

    public class ScriptNodeTypeSearchOutput : SearchOutputDto<ScriptNodeTypeOutput, long>
    {
        public override Pager Page { get; set; }

        public override IEnumerable<ScriptNodeTypeOutput> Datas { get; set; }
    }

    #endregion

    #region ScriptNode 节点

    [AutoMap(typeof(ScriptNode), typeof(ScriptNodeLog))]
    public class ScriptNodeInput : EntityDto<long>
    {
        public new long Id { get; set; }

        [Required(ErrorMessage = "节点名不能为空")]
        [Display(Name = "节点名")]
        public virtual string Name { get; set; }

        public virtual long? ScriptNodeTypeId { get; set; }

        public virtual ScriptNodeType ScriptNodeType { get; set; }

        [Display(Name = "节点代码")]
        public virtual string Code { get; set; }

        public virtual long? DbServerId { get; set; }

        public virtual DbServer DbServer { get; set; }

        [Display(Name = "任务类型")]
        public virtual short? ScriptModel { get; set; }

        [Display(Name = "命令内容")]
        public virtual string Content { get; set; }

        /// <summary>
        /// 修改节点的时候是否修改脚本流实例的节点
        /// 1 是 2否
        /// </summary>
        public virtual short? IsUpdateExample
        {
            get;

            set;
          
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public virtual string Remark { get; set; }

        #region 建表节点特有

        [Display(Name = "表英文名")]
        public virtual string EnglishTabelName { get; set; }

        /// <summary>
        /// 表中文名
        /// </summary>
        [Display(Name = "表中文名")]
        public virtual string ChineseTabelName { get; set; }

        /// <summary>
        /// 表类型（公用表、私有表）
        /// </summary>
        [Display(Name = "表类型")]
        public virtual short? TableType { get; set; }

        /// <summary>
        /// 建表模式（复制、新建）
        /// </summary>
        [Display(Name = "建表模式")]
        public virtual short? TableModel { get; set; }
        /// <summary>
        /// 脚本流特殊需要
        /// </summary>
        public virtual string TaskSpecific { get; set; }
        #endregion
    }

    [AutoMap(typeof(ScriptNode))]
    public class ScriptNodeOutput : EntityDto<long>
    {

        public new long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual long? ScriptNodeTypeId { get; set; }

        //public virtual ScriptNodeType ScriptNodeType { get; set; }

        public virtual string ScriptNodeTypeName { get; set; }

        public virtual string Code { get; set; }

        public virtual long? DbServerId { get; set; }

        //public virtual DbServer DbServer { get; set; }

        public virtual string DbServerName { get; set; }

        public virtual short? ScriptModel { get; set; }

        public virtual string ScriptModelStr
        {
            get
            {
                if (this.ScriptModel == 1)
                {
                    return "建表";
                }
                else if (this.ScriptModel == 2)
                {
                    return "命令段";
                }
                else {
                    return "";
                }
            }
        }

        public virtual string Content { get; set; }

        public virtual string Remark { get; set; }
        /// <summary>
        /// 任务组页面新增节点特有字段
        /// </summary>
        public virtual string TaskSpecific { get; set; }

        #region 建表节点特有
        public virtual string EnglishTabelName { get; set; }

        public virtual string ChineseTabelName { get; set; }

        public virtual short? TableType { get; set; }

        public virtual string TableTypeStr
        {
            get {
                if (this.TableType == 1)
                {
                    return "公有表";
                }
                else if (this.TableType == 0)
                {
                    return "私有表";
                }
                else { return ""; }
            }
        }

        public virtual short? TableModel { get; set; }

        public virtual string TableModelStr
        {
            get
            {
                if (this.TableModel == 1)
                {
                    return "新建";
                }
                else if (this.TableModel == 2)
                {
                    return "复制";
                }
                else { return ""; }
            }
        }
        #endregion
    }

    public class ScriptNodeSearchInput : SearchInputDto<long>
    {
    }

    public class ScriptNodeSearchOutput : SearchOutputDto<ScriptNodeOutput, long>
    {
        public override Pager Page { get; set; }

        public override IEnumerable<ScriptNodeOutput> Datas { get; set; }
    }

    #endregion

}
