#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：FilterExtension
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/3 18:52:34
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion


#region 主体



namespace EasyMan.EasyQuery
{
    using System;
    using System.Collections.Generic;


    public static class FilterExtension
    {
        private static readonly Dictionary<OperatorType, string> NumberTypeExpressions = new Dictionary<OperatorType, string>
        {
            { OperatorType.Equal, "{0} = {1}"},
            { OperatorType.NotEqual, "{0} != {1}"},
            { OperatorType.MoreThan, "{0} > {1}"},
            { OperatorType.LessThan, "{0} < {1}"},
        };

        private static readonly Dictionary<OperatorType, string> StringTypeExpressions = new Dictionary<OperatorType, string>
        {
            { OperatorType.Equal, "{0} = {1}"},
            { OperatorType.NotEqual, "{0} != {1}"},
            { OperatorType.Contain, "{0}.Contains({1})"},
            { OperatorType.NotContain, "!{0}.Contains({1})"},
            { OperatorType.BeginWidth, "{0}.StartsWith({1})"},
            { OperatorType.EndWidth, "{0}.EndsWith({1})"},
        };

        private static readonly Dictionary<OperatorType, string> BoolTypeExpressions = new Dictionary<OperatorType, string>
        {
            { OperatorType.Equal, "{0} = {1}"},
            { OperatorType.NotEqual, "{0} != {1}"},
        };

        public static string GetExpression(this SearchFilter filter)
        {
            if (filter.Type == typeof(bool))
            {
                return BoolTypeExpressions[filter.Operator];
            }
            else if (filter.Type == typeof(double) || filter.Type == typeof(int))
            {
                return NumberTypeExpressions[filter.Operator];
            }
            else if (filter.Type == typeof(DateTime))
            {
                return NumberTypeExpressions[filter.Operator];
            }
            else
            {
                return StringTypeExpressions[filter.Operator];
            }
        }
    }
}
#endregion
