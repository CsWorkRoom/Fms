using Easyman.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Easyman.Common.Configuration
{
    [Table(SystemConfiguration.TablePrefix + "REPORT_QUERY")]
    public class ReporyQuery : DateTimeEntityHelper
    {
        [Key, Column("ID")]
        public override long Id { get; set; }

        [Column("REPORT_ID")]
        public virtual long ReportId { get; set; }

        [Column("FIELD_CODE"),StringLength(50)]
        public virtual string FieldCode { get; set; }

        [Column("FIELD_NAME")]
        public virtual string FieldName { get; set; }

        [Column("FIELD_PARAM")]
        public virtual string FieldParam { get; set; }

        //输入类型     *枚举*
        [Column("INPUT_TYPE")]
        public virtual int InputType { get; set; }

        //输入验证正则表达式
        [Column("INPUT_REG"),StringLength(50)]
        public virtual string InputReg { get; set; }

        //查询表达式
        [Column("QUERYEXP")]
        public virtual string QueryExp { get; set; }

        [Column("DEFAULT_VALUE")]
        public virtual string DefaultValue { get; set; }

        //字典编码
        [Column("DD_CODE")]
        public virtual string DdCode { get; set; }

        //查询值来源于sql
        [Column("DATA_SQL")]
        public virtual string DataSql { get; set; }

        //是否快捷查询
        [Column("IS_SHORTCUT")]
        public virtual bool IsShortcur { get; set; }

        [Column("ORDER")]
        public virtual int Order { get; set; }


    }
}
