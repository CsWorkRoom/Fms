#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：SearchColumn
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 17:39:07
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体

using System;

namespace EasyMan.EasyQuery
{
    public class SearchFilter
    {
        public virtual string Name { get; set; }

        public virtual object Value { get; set; }

        public virtual OperatorType Operator { get; set; }

        public virtual string TypeString { get; set; }
        public Type Type
        {
            get
            {
                switch (TypeString.ToLower())
                {
                    case "bool":
                        TValue = Convert.ToBoolean(Value);
                        return typeof(bool);
                    case "double":
                        TValue = Convert.ToDouble(Value);
                        return typeof(double);
                    case "number":
                        TValue = Convert.ToInt32(Value);
                        return typeof(int);
                    case "date":
                        TValue = Convert.ToDateTime(Value);
                        return typeof(int);
                    default:
                        TValue = Value.ToString();
                        return typeof(string);
                }
            }
        }

        public object TValue { get; set; }
    }

    public enum OperatorType : byte
    {
        //等于
        Equal,
        //不等于
        NotEqual,
        //大于
        MoreThan,
        //小于
        LessThan,
        //包含
        Contain,
        //不包含
        NotContain,
        //开始于
        BeginWidth,
        //结束于
        EndWidth
    }
}
#endregion
