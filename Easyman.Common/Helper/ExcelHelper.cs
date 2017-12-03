using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HPSF;
using NPOI.SS.Util;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using NPOI.XSSF.UserModel;

namespace Easyman.Common
{
    public class MultiSheet
    {
        /// <summary>
        /// 表头名
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 多表头
        /// </summary>
        public Dictionary<string, int> TopTitle { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public Dictionary<string, string> DicTitle { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public DataTable Data { get; set; }
    }

    public static class ExcelHelper
    {
        /// <summary>
        /// list转换为datatable
        /// </summary>
        /// <typeparam name="T">list实体</typeparam>
        /// <param name="data">数据集合</param>
        /// <returns>datatable类型数据</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private static DataTable GetData(Stream stream)
        {
            return GetData(stream, null);
        }

        private static DataTable GetData(Stream stream, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            var sheet = string.IsNullOrEmpty(sheetName) ? workbook.GetSheetAt(0) : workbook.GetSheet(sheetName);
            List<string> cols = new List<string>();
            int colIdx = 0;
            while (sheet.GetRow(0).GetCell(colIdx) != null)
            {
                cols.Add(sheet.GetRow(0).GetCell(colIdx++).StringCellValue.Trim());
            }

            DataTable dt = new DataTable();
            foreach (string colName in cols)
            {
                dt.Columns.Add(colName, typeof(string));
            }
            int end = sheet.LastRowNum;
            int col = dt.Columns.Count;
            for (int i = 1; i <= end; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < col; j++)
                {
                    var cell = sheet.GetRow(i).GetCell(j);
                    dr[j] = cell == null ? string.Empty : cell.StringCellValue.Trim();
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }//03版本excel
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dicTitle">列名称</param>
        /// <param name="data">数据</param>
        /// <param name="sheetName">表头名</param>
        /// <param name="company">公司</param>
        /// <param name="subject">主题</param>
        /// <returns></returns>
        public static MemoryStream CreateExcel<T>(Dictionary<string, string> dicTitle, List<T> data, string sheetName, string company, string subject)
        {
            return CreateExcel<T>(dicTitle, data, sheetName, company, subject, string.Empty);
        }
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dicTitle">列名称</param>
        /// <param name="data">数据</param>
        /// <param name="sheetName">表头名</param>
        /// <param name="company">公司</param>
        /// <param name="subject">主题</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public static MemoryStream CreateExcel<T>(Dictionary<string, string> dicTitle, List<T> data, string sheetName, string company, string subject, string description)
        {
            return CreateExcel<T>(dicTitle, data, sheetName, company, subject, description, null);
        }
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dicTitle">列名称</param>
        /// <param name="data">数据</param>
        /// <param name="sheetName">表头名</param>
        /// <param name="company">公司</param>
        /// <param name="subject">主题</param>
        /// <param name="description">描述</param>
        /// <param name="topTitle">多表头</param>
        /// <returns></returns>
        public static MemoryStream CreateExcel<T>(Dictionary<string, string> dicTitle, List<T> data, string sheetName, string company, string subject, string description, Dictionary<string, int> topTitle)
        {
            PropertyInfo[] properties = new PropertyInfo[dicTitle.Count];
            int idx = 0;
            foreach (KeyValuePair<string, string> kv in dicTitle)
            {
                PropertyInfo property = typeof(T).GetProperty(kv.Key);
                if (property == null)
                {
                    throw new Exception(string.Format("'{0}' not contains propertiy '{1}'", typeof(T).Name, kv.Key));
                }
                properties[idx++] = property;
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = company;
            workbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = subject;
            workbook.SummaryInformation = si;

            var sheet = workbook.CreateSheet(sheetName);
            int r = 0;
            if (topTitle != null)
            {
                var topRow = sheet.CreateRow(r);
                int topIdx = 0;
                foreach (var kv in topTitle)
                {
                    topRow.CreateCell(topIdx, CellType.String).SetCellValue(kv.Key);
                    sheet.AddMergedRegion(new CellRangeAddress(r, r, topIdx, topIdx + kv.Value - 1));
                    topIdx = topIdx + kv.Value;
                }

                r++;
            }
            if (!string.IsNullOrEmpty(description))
            {
                var descRow = sheet.CreateRow(r);
                descRow.CreateCell(0, CellType.String).SetCellValue(description);
                sheet.AddMergedRegion(new CellRangeAddress(r, r, 0, dicTitle.Count - 1));
                r++;
            }
            var row = sheet.CreateRow(r);
            idx = 0;
            foreach (var kv in dicTitle)
            {
                row.CreateCell(idx++, CellType.String).SetCellValue(kv.Value);
            }

            for (int i = 0; i < data.Count; i++)
            {
                row = sheet.CreateRow(i + 1 + r);
                for (int j = 0; j < properties.Length; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(properties[j].GetValue(data[i], null).ToString());
                }
            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <param name="sheets">sheet集合</param>
        /// <param name="company">公司</param>
        /// <param name="subject">主题</param>
        /// <returns>内存流</returns>
        public static MemoryStream CreateExcel(List<MultiSheet> sheets, string company, string subject)
        {
            #region 有效性验证

            StringBuilder error = new StringBuilder();
            if (sheets == null || sheets.Count == 0)
            {
                error.Append(string.Format("不包含任何表单数据!"));
            }
            else
            {
                foreach (MultiSheet ms in sheets)
                {
                    if (ms.DicTitle == null || ms.DicTitle.Count == 0)
                    {
                        error.Append(string.Format("“{0}”中不包含任何列；\r\n", ms.SheetName));
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> kv in ms.DicTitle)
                        {

                            if (ms.Data != null && !ms.Data.Columns.Contains(kv.Key))
                            {
                                error.Append(string.Format("“{0}”中不包含“{1}”列；\r\n", ms.SheetName, kv.Key));
                            }
                        }
                    }
                }
            }
            if (error.Length > 0)
            {
                throw new Exception(error.ToString());
            }

            #endregion

            #region Excel文件信息

            HSSFWorkbook workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = company;
            workbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = subject;
            workbook.SummaryInformation = si;

            //设置字体
            var font = workbook.CreateFont();
            font.Boldweight = 700;
            //设置边框
            var style = workbook.CreateCellStyle();
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            #endregion

            #region 写入每个Sheet

            foreach (MultiSheet ms in sheets)
            {
                var sheet = workbook.CreateSheet(ms.SheetName);
                int rowIdx = 0;
                int colIdx = 0;
                if (ms.TopTitle != null)
                {
                    var topRow = sheet.CreateRow(rowIdx);
                    int topIdx = 0;
                    foreach (var kv in ms.TopTitle)
                    {
                        var cell = topRow.CreateCell(topIdx, CellType.String);
                        cell.SetCellValue(kv.Key);
                        cell.CellStyle = style;
                        sheet.AddMergedRegion(new CellRangeAddress(rowIdx, rowIdx, topIdx, topIdx + kv.Value - 1));
                        topIdx = topIdx + kv.Value;
                    }

                    rowIdx++;
                }
                if (!string.IsNullOrEmpty(ms.Description))
                {
                    var descRow = sheet.CreateRow(rowIdx);
                    descRow.CreateCell(0, CellType.String).SetCellValue(ms.Description);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIdx, rowIdx, 0, ms.DicTitle.Count - 1));
                    rowIdx++;
                }
                var row = sheet.CreateRow(rowIdx);
                row.Height = 25 * 20;

                foreach (var kv in ms.DicTitle)
                {
                    var cell = row.CreateCell(colIdx++, CellType.String);
                    sheet.SetColumnWidth(colIdx, sheet.GetColumnWidth(colIdx) + 10 * 256);
                    cell.SetCellValue(kv.Value);
                    cell.CellStyle = style;
                }

                if (ms.Data != null)
                {
                    for (int i = 0; i < ms.Data.Rows.Count; i++)
                    {
                        row = sheet.CreateRow(i + 1 + rowIdx);
                        colIdx = 0;
                        foreach (string colName in ms.DicTitle.Keys)
                        {
                            string value = ms.Data.Rows[i][colName] == null ? string.Empty : ms.Data.Rows[i][colName].ToString();
                            var cell = row.CreateCell(colIdx++, CellType.String);
                            cell.SetCellValue(value);
                        }
                    }
                }
            }

            #endregion

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        /// <summary>
        /// excel文件转化为datatable
        /// </summary>
        /// <param name="path">excel文件路径</param>
        /// <returns>datatable类型数据</returns>
        public static DataTable ToDataTable(this string path)
        {
            IWorkbook iWorkbook;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                iWorkbook = CreateWorkbook(fileStream);
                //获取第一个sheet
                var sheet = iWorkbook.GetSheetAt(0);
                var dt = new DataTable();
                //默认，第一行是字段
                IRow headRow = sheet.GetRow(0);
                //设置datatable字段
                for (int i = headRow.FirstCellNum, len = headRow.LastCellNum; i < len; i++)
                {
                    dt.Columns.Add(headRow.Cells[i].StringCellValue);
                }
                //遍历数据行
                for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
                {
                    IRow tempRow = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    //遍历一行的每一个单元格
                    for (int r = 0, j = tempRow.FirstCellNum, len2 = tempRow.LastCellNum; j < len2; j++, r++)
                    {
                        ICell cell = tempRow.GetCell(j);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.String:
                                    dataRow[r] = cell.StringCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[r] = cell.NumericCellValue;
                                    break;
                                case CellType.Boolean:
                                    dataRow[r] = cell.BooleanCellValue;
                                    break;
                                default:
                                    dataRow[r] = cell.ToString();
                                    break;
                            }
                           
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }
        }
        /// <summary>
        /// 创建工作簿对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IWorkbook CreateWorkbook(Stream stream)
        {
            try
            {
                return new XSSFWorkbook(stream); //07
            }
            catch
            {
                return new HSSFWorkbook(stream); //03
            }

        }
    }


}