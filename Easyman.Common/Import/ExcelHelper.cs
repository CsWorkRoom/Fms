using System;
using System.Data;
using System.IO;
using Syncfusion.XlsIO;
using System.Text;

namespace EasyMan.Import
{
    public class ExcelHelper
    {
        /// <summary>
        /// 将Excel文件里数据读取为DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        public static DataTable ExportExcelDataTable(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath");
            if (!File.Exists(filePath))
                throw new ArgumentException("{0} 文件不存在".FormatWith(filePath));

            var excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(filePath, ExcelOpenType.Automatic);
            IWorksheet sheet = workbook.Worksheets[0];

            DataTable dataTable = sheet.ExportDataTable(sheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);

            workbook.Close();
            excelEngine.Dispose();

            return dataTable;
        }

        /// <summary>
        /// 将Excel文件里数据读取为DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        public static DataTable ExportCsvDataReader(string filePath)
        {
            //if (string.IsNullOrWhiteSpace(filePath))
            //    throw new ArgumentNullException("filePath");
            //if (!File.Exists(filePath))
            //    throw new ArgumentException("{0} 文件不存在".FormatWith(filePath));

            //var dataTable = new DataTable();
            //Encoding r= EncodingType.GetType(filePath);
            //using (var streamReader = new StreamReader(filePath, r))
            //{
            //    var reader = new CsvReader(streamReader, false, '&');
            //    dataTable.Load(reader);
            //}
            //return dataTable;
            return null;
        }
    }
}
