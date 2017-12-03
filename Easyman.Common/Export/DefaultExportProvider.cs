#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：DefaultExportProvider
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/4/11 20:59:05
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using EasyMan.Extensions;

#region 主体



namespace EasyMan.Export
{
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using Syncfusion.XlsIO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;


    public class DefaultExportProvider : IExportProvider
    {

        #region IExportService 成员

        public byte[] Export(System.Data.DataTable source)
        {
            return Export(source, null);
        }

        public byte[] Export(System.Data.DataTable source, Dictionary<string, string> titleMap)
        {
            return Export(source, titleMap, ExportFileType.Excel);
        }

        public byte[] Export(System.Data.DataTable source, Dictionary<string, string> titleMap, ExportFileType type)
        {
            return Export(source, titleMap, ExportFileType.Excel, false);
        }

        public byte[] Export(System.Data.DataTable source, Dictionary<string, string> titleMap, ExportFileType type, bool isZip)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");
                if (titleMap != null)
                {
                    titleMap.Keys.Each((key, i) =>
                    {
                        worksheet.Cells[1, i + 1].Value = titleMap[key];

                        for (int j = 0; j < source.Rows.Count; j++)
                        {
                            try
                            {
                                worksheet.Cells[2 + j, i + 1].Value = source.Rows[j][key];
                            }
                            catch
                            { }
                        }
                    });
                }
                else
                {
                    for (int i = 0; i < source.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = source.Columns[i].ColumnName;

                        for (int j = 0; j < source.Rows.Count; j++)
                        {
                            worksheet.Cells[2 + j, i + 1].Value = source.Rows[j][i];
                        }
                    }
                }

                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap)
        {
            return Export<T>(source, titleMap, ExportFileType.Excel);
        }

        public byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap, ExportFileType type)
        {
            return Export<T>(source, titleMap, ExportFileType.Excel, false);
        }

        public byte[] Export<T>(IEnumerable<T> source, Dictionary<string, string> titleMap, ExportFileType type, bool isZip)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("sheet1");
                if (titleMap != null)
                {
                    titleMap.Keys.Each((key, i) =>
                    {
                        worksheet.Cells[1, i + 1].Value = titleMap[key];

                        worksheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.PatternColor.SetColor(Color.Gray);
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);


                        for (int j = 0; j < source.Count(); j++)
                        {
                            try
                            {
                                var sour = source.Skip(j).Take(1).First();
                                worksheet.Cells[2 + j, i + 1].Value = GetValue(sour, key);
                            }
                            catch
                            {
                                throw new Exception("对象找不到名称为{0}的属性".FormatWith(key));
                            }
                        }
                    });
                }

                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public byte[] ExportBig(IDataReader reader)
        {
            return ExportBig(reader, ExportFileType.Csv);
        }

        public byte[] ExportBig(IDataReader reader, ExportFileType type)
        {
            return ExportBig(reader, type, false);
        }

        public byte[] ExportBig(IDataReader reader, ExportFileType type, bool isZip)
        {
            if (type == ExportFileType.Excel)
            {
                using (var engine = new ExcelEngine())
                {

                    var workbook = engine.Excel.Workbooks.Create(1);
                    workbook.Version = ExcelVersion.Excel2010;
                    workbook.BuiltInDocumentProperties.Author = "EasyMan";
                    workbook.BuiltInDocumentProperties.Company = "成都联宇创新科技股份有限公司";
                    workbook.BuiltInDocumentProperties.CreationDate = DateTime.Now;
                    var sheet = workbook.Worksheets[0];


                    sheet.ImportDataTable(reader.ToDataTable(), true, 1, 1);
                    sheet.UsedRange.CellStyle.Font.FontName = "宋体";
                    sheet.UsedRange.CellStyle.Font.Size = 11.0;
                    sheet.UsedRange.CellStyle.Locked = true;

                    var stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    return stream.ToArray();
                }
            }
            else
            {
                var table = reader.GetSchemaTable();
                var buffer = new object[table.Rows.Count];
                var format = GetColumnFormat(table.Rows, ",");
                var stream = new MemoryStream();
                var sw = new StreamWriter(stream, Encoding.Default);
                WriteTextHeader(sw, table, ",");

                while (reader.Read())
                {
                    reader.GetValues(buffer);
                    sw.Write(format, buffer);
                    sw.WriteLine();
                }
                sw.Flush();

                return stream.GetBuffer();
            }
        }

        public void ExportOracleFile(string sql, string fileName, List<string> fieldList)
        {
            var exportSql = @"declare
                                        outfile utl_file.file_type;
                                    begin
                                        outfile := utl_file.fopen('DATAEXPORTPATH','data.txt','W');
                                        for rec in ({0})
                                        loop
                                          utl_file.put_line(outfile, rec.id||','||rec.reward_date||','||rec.company_code||','||rec.company_name||','||rec.channel_type_id||','||rec.channel_type_name||','||rec.channel_type_name||','||rec.DISTRICT_ID||','||rec.DISTRICT_NAME||','||rec.channel_type_name);
                                        end loop;
                                        utl_file.fclose(outfile);
                                    end;";
            sql = sql.Replace(Environment.NewLine, "\n");

        }

        #endregion

        private static string GetValue<T>(T t, string attributeName)
        {
            var property = typeof(T).GetProperty(attributeName);

            try
            {
                return property.GetValue(t, null).ToString();
            }
            catch (Exception)
            {

                return "";
            }
        }

        private static string GetColumnFormat(ICollection rowCollection, string fieldSeparator)
        {
            var result = "";

            for (var i = 0; i < rowCollection.Count; i++)
            {
                result += fieldSeparator + "{{{0}}}".FormatWith(i);
            }

            return result.HasValue() ? result.Substring(1) : "";
        }

        private static void WriteTextHeader(TextWriter writer, DataTable schema, string fieldSeparator)
        {
            var count = 0;

            foreach (DataRow row in schema.Rows)
            {
                if (count > 0)
                    writer.Write(fieldSeparator);

                writer.Write(row[0].ToString());
                count++;
            }

            writer.WriteLine();
        }
    }
}
#endregion
