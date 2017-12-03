#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：IExportProvider
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/4/11 20:57:46
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using Abp.Dependency;
using System.Data;

#region 主体



namespace EasyMan.Export
{
    using System.Collections.Generic;


    public interface IExportProvider : ISingletonDependency
    {
        byte[] Export(DataTable source);
        byte[] Export(DataTable source, Dictionary<string, string> titleMap);
        byte[] Export(DataTable source, Dictionary<string, string> titleMap, ExportFileType type);
        byte[] Export(DataTable source, Dictionary<string, string> titleMap, ExportFileType type, bool isZip);

        byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap);
        byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap, ExportFileType type);
        byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap, ExportFileType type, bool isZip);

        //主要用户导出量大的数据
        byte[] ExportBig(IDataReader reader);
        byte[] ExportBig(IDataReader reader, ExportFileType type);
        byte[] ExportBig(IDataReader reader, ExportFileType type, bool isZip);

        void ExportOracleFile(string sql, string fileName, List<string> fieldList);
    }
}
#endregion
