#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：IDataReaderExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/4/15 16:29:16
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Data;

#region 主体



namespace EasyMan.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public static class IDataReaderExtensions
    {
        public static DataTable ToDataTable(this IDataReader reader)
        {
            var table = new DataTable();
            var fieldCount = reader.FieldCount;

            for (var index = 0; index < fieldCount; ++index)
            {
                table.Columns.Add(reader.GetName(index), reader.GetFieldType(index));
            }

            table.BeginLoadData();

            var values = new object[fieldCount];

            while (reader.Read())
            {
                reader.GetValues(values);
                table.LoadDataRow(values, true);
            }
            reader.Close();
            table.EndLoadData();

            return table;
        }
    }
}
#endregion
