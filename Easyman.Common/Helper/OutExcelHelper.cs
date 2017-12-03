using EasyMan.Common.Data;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Easyman.Common.Helper
{

    #region EXCEL导出模型 
    /// <summary>
    /// EXCEL导出模型 
    /// </summary>
    class ColumnJson
    {
        /// <summary>
        /// 列头的值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列头的合并值
        /// </summary>             
        public int[] list { get; set; }
    }
    #endregion

    public class OutExcelHelper
    {
       static int maxRow = 100000;//Excel每个文件最大支持多少条数据 
       static string strFiles = "";///记录压缩包的文件

        #region Excel CSV Txt文件导出

        #region Excel调渡方法
        /// <summary>
        /// DataTable 导成EXCEL
        /// </summary>
        /// <param name="dt">DataTable数据集</param>
        /// <param name="strFileName">文件名称（格式例：数据统计月报表.xlsx），不指定将默认按日期命名</param>
        /// <param name="strMapPath">虚拟路径</param>
        /// <param name="strColumnHeader">列头命名（格式例：[{ name: '名称',list:[0, 1, 1, 1]},{ name: '状态',list:[0, 0, 3, 6]},{ name: '区间值',list:[0, 0, 7, 12]}]）list值：前两段是表示搜引号，后两位表示合并单元格;不指定将按Datatable的列头命名</param>
        /// <param name="blnDown">是否立即下载，默认为是</param>
        public static void ExportExcel(DataTable ExecDt, string strFileName,string strMapPath, string strColumnHeader = "", bool blnDown = true)
        {
            //生成Excel
            IWorkbook book = BuildWorkbook(ExecDt, strColumnHeader);
            #region 保存到本地
            using (FileStream fs = new FileStream(strMapPath, FileMode.Create, FileAccess.Write))
            {
                book.Write(fs);                
                #region 是否下载
                if (blnDown && strFiles == "")
                {
                    //FileStream fsObj = new FileStream(strMapPath, FileMode.Open);
                    //byte[] bytes = new byte[(int)fsObj.Length];
                    //fsObj.Read(bytes, 0, bytes.Length);
                    //fsObj.Close();
                    //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    ////通知浏览器下载文件而不是打开 
                    //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));
                    //System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                    //System.Web.HttpContext.Current.Response.Flush();
                    //System.Web.HttpContext.Current.Response.End();
                }
                #endregion
            }
            #endregion
        }
        #endregion

        #region CSV 文件导出
        /// <summary>
        /// DataTable 导成CSV
        /// </summary>
        /// <param name="dt">DataTable数据集</param>
        /// <param name="fileName">文件名称（格式例：数据统计月报表.CSV），不指定将默认按日期命名</param>
        /// <param name="strColumnHeader">列头命名（格式例：名称|状态|区间值|);不指定将按Datatable的列头命名</param>
        /// <param name="blnDown">是否立即下载，默认为是</param>
        public static void ExportCsv(DataTable ExecDt, string strFileName, string strMapPath, string strColumnHeader = "", bool blnDown = true)
        {
            strFileName = strFileName.Trim();
            #region 列头
            StringBuilder strContent = new StringBuilder();
            string[] clm = new string[(string.IsNullOrEmpty(strColumnHeader) ? ExecDt.Columns.Count : strColumnHeader.Split('|').Length)];
            int i = 0;
            if (string.IsNullOrEmpty(strColumnHeader))
            {
                foreach (DataColumn colm_Item in ExecDt.Columns) //得到标题栏
                {
                    strContent.Append(colm_Item.ColumnName + ",");
                    clm[i] = colm_Item.ColumnName;
                    i++;
                }
            }
            else
            {
                foreach (string colm_Item in strColumnHeader.Split('|')) //得到标题栏
                    strContent.Append(colm_Item + ",");
            }
            strContent.Append("\r\n");
            #endregion

            #region 内容
            foreach (DataRow Row_Item in ExecDt.Rows)//得到内容
            {
                if (string.IsNullOrEmpty(strColumnHeader))
                {
                    foreach (string clm_Item in clm)
                        strContent.Append(Row_Item[clm_Item] + ",");
                }
                else
                {
                    foreach (DataColumn clm_Item in ExecDt.Columns)
                        strContent.Append(Row_Item[clm_Item.ColumnName] + ",");
                }
                strContent.Append("\r\n");
            }
            #endregion

            System.IO.FileInfo oFile = new FileInfo(strMapPath);
            if (!oFile.Exists)
                oFile.Create().Close();
            System.IO.StreamWriter oWrite = new StreamWriter(strMapPath, false, System.Text.Encoding.UTF8);
            oWrite.Write(strContent);
            oWrite.Flush();
            oWrite.Close();

            #region 是否下载
            if (blnDown && strFiles == "")
            {
                //FileStream fsObj = new FileStream(strMapPath, FileMode.Open);
                //byte[] bytes = new byte[(int)fsObj.Length];
                //fsObj.Read(bytes, 0, bytes.Length);
                //fsObj.Close();
                //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                ////通知浏览器下载文件而不是打开 
                //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));
                //System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                //System.Web.HttpContext.Current.Response.Flush();
                //System.Web.HttpContext.Current.Response.End();
            }
            #endregion            
        }
        #endregion

        #region TXT 文件导出
        /// <summary>
        /// DataTable 导成CSV
        /// </summary>
        /// <param name="dt">DataTable数据集</param>
        /// <param name="fileName">文件名称（格式例：数据统计月报表.CSV），不指定将默认按日期命名</param>
        /// <param name="strColumnHeader">列头命名（格式例：名称|状态|区间值|);不指定将按Datatable的列头命名</param>
        /// <param name="blnDown">是否立即下载，默认为是</param>
        public static void ExportTxt(DataTable ExecDt, string strFileName, string strMapPath, string strColumnHeader = "", bool blnDown = true)
        {
            strFileName = strFileName.Trim();
            #region 列头
            StringBuilder strContent = new StringBuilder();
            string[] clm = new string[(string.IsNullOrEmpty(strColumnHeader) ? ExecDt.Columns.Count : strColumnHeader.Split('|').Length)];
            int i = 0;
            if (string.IsNullOrEmpty(strColumnHeader))
            {
                foreach (DataColumn colm_Item in ExecDt.Columns) //得到标题栏
                {
                    strContent.Append(colm_Item.ColumnName + "\t\t");
                    clm[i] = colm_Item.ColumnName;
                    i++;
                }
            }
            else
            {
                foreach (string colm_Item in strColumnHeader.Split('|')) //得到标题栏
                    strContent.Append(colm_Item + "\t\t");
            }
            strContent.Append("\r\n");
            #endregion

            #region 内容
            object strValue = null;
            foreach (DataRow Row_Item in ExecDt.Rows)//得到内容
            {
                if (string.IsNullOrEmpty(strColumnHeader))
                {
                    foreach (string clm_Item in clm)
                    {
                        strValue = Row_Item[clm_Item];
                        strValue = strValue != null || strValue.ToString().Trim() != "" ? strValue : "无值";
                        strContent.Append(strValue + "\t\t");
                    }
                }
                else
                {
                    foreach (DataColumn clm_Item in ExecDt.Columns)
                    {
                        strValue= Row_Item[clm_Item.ColumnName];
                        strValue = strValue != null || strValue.ToString().Trim() != "" ? strValue : "无值";
                        strContent.Append(strValue + "\t\t");
                    }
                }
                strContent.Append("\r\n");
            }
            #endregion

            System.IO.FileInfo oFile = new FileInfo(strMapPath);
            if (!oFile.Exists)
                oFile.Create().Close();
            System.IO.StreamWriter oWrite = new StreamWriter(strMapPath, false, System.Text.Encoding.UTF8);
            oWrite.Write(strContent);
            oWrite.Flush();
            oWrite.Close();

            #region 是否下载
            if (blnDown && strFiles == "")
            {
                //FileStream fsObj = new FileStream(strMapPath, FileMode.Open);
                //byte[] bytes = new byte[(int)fsObj.Length];
                //fsObj.Read(bytes, 0, bytes.Length);
                //fsObj.Close();
                //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                ////通知浏览器下载文件而不是打开 
                //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));
                //System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                //System.Web.HttpContext.Current.Response.Flush();
                //System.Web.HttpContext.Current.Response.End();
            }
            #endregion            
        }
        #endregion

        #region 组装workbook
        /// <summary>
        ///  组装workbook.
        /// </summary>
        /// <param name="dt">dataTable资源</param>
        /// <param name="columnHeader">列头</param>
        /// <returns></returns>
        public static XSSFWorkbook BuildWorkbook(DataTable dt, string strColumnHeader = "")
        {
            var workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(string.IsNullOrWhiteSpace(dt.TableName) ? "Sheet1" : dt.TableName);

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            var arrColWidth = new int[dt.Columns.Count];
            foreach (DataColumn item in dt.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dt.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;//行号
            foreach (DataRow row in dt.Rows)
            {
                #region 表头 列头
                if (rowIndex == 1048576 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    if (strColumnHeader == "")
                    {
                        IRow row0 = sheet.CreateRow(0);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell cell = row0.CreateCell(i, CellType.String);
                            cell.SetCellValue(dt.Columns[i].ColumnName);
                        }
                    }
                    else
                    {
                        List<ColumnJson> objJsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnJson>>(strColumnHeader);
                        #region 设置合并行
                        foreach (ColumnJson item in objJsonList)
                        {
                            if (item.list[1] > rowIndex)
                            {
                                rowIndex = item.list[1];
                            }
                            CellRangeAddress cell = new CellRangeAddress(item.list[0], item.list[1], item.list[2], item.list[3]);
                            sheet.AddMergedRegion(cell);
                        }
                        #endregion

                        #region 对合并行赋值
                        // 定义行样式
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;// 左右居中    
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中 
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (int i = 0; i <= rowIndex; i++)
                        {
                            IRow headerRow = sheet.CreateRow(i);
                            foreach (ColumnJson item in objJsonList)
                            {
                                if (item.list[0] == i)
                                {
                                    headerRow.CreateCell(item.list[2]).SetCellValue(item.Name);
                                    headerRow.GetCell(item.list[2]).CellStyle = headStyle;
                                }
                            }

                        }
                        #endregion
                    }
                    rowIndex++;
                }
                #endregion

                #region 内容
                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            return workbook;
        }
        #endregion

        #endregion
        
        #region 将数据写入指定的文件之中
        /// <summary>
        /// 将数据写入指定的文件之中
        /// </summary>
        /// <param name="strFilepath">文件保存路径(带文件名及后缀名的文件路径)</param>
        /// <param name="strValue">文件保存内容</param>
        /// <param name="charset">文件保存编码方式(为null或为空时系统默认为utf-8)</param>
        public bool WriteFile(string strFilepath, string strValue, string charset)
        {
            if (strFilepath == null || strFilepath.Trim() == "")
                return false;
            try
            {
                if (charset == null || charset.Trim() == "")
                    charset = "utf-8";
                strFilepath = System.Web.HttpContext.Current.Server.MapPath(strFilepath);
                System.IO.FileInfo oFile = new FileInfo(strFilepath);
                if (!oFile.Directory.Exists)
                    oFile.Directory.Create();

                if (!oFile.Exists)
                    oFile.Create().Close();
                System.IO.StreamWriter oWrite = new StreamWriter(strFilepath, false, System.Text.Encoding.GetEncoding(charset));
                oWrite.Write(strValue);
                oWrite.Flush();
                oWrite.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 记录下载文件至数据库
        /// <summary>
        /// 记录下载文件至数据库
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="strMapPath"></param>
        /// <param name="strPath"></param>
        public void WriteFilesDataBase(string strTitle, string strMapPath, string strPath)
        {
            System.IO.FileInfo FileObj = new FileInfo(strMapPath);
            long lngSize = FileObj.Length;//长度
            string strExt = FileObj.Extension;//扩展后缀
            string strSql = "insert into EM_DownloadFile (title,sizeNum,ext,createDate,countNum,constituteDate,urlPath,state)";
            strSql += "values('" + strTitle + "'," + lngSize + ",'" + strExt + "',to_date('" + DateTime.Now.ToString() + "', 'YYYY-MM-DD HH24:MI:SS')" + ",0,to_date('" + DateTime.Now.ToString() + "', 'YYYY-MM-DD HH24:MI:SS'),'" + strPath + "',1)";
            using (var session = DatabaseSession.OpenSession())
            {
                try
                {
                    int intExe = session.Execute(strSql, null, null, null, null);
                    if (intExe <= 0)
                    {
                        // Logger.Debug("str:" + strSql + ",保存失败！");//日志文件
                    }
                }
                catch (Exception ex)
                {
                    //  Logger.Debug("str:" + strSql + ",保存失败！error:" + ex);//日志文件
                }
            }
        }
        #endregion

        #region 记录生成任务
        /// <summary>
        /// 记录生成任务
        /// </summary>
        /// <param name="strColumnHeader"></param>
        /// <param name="strExecSql"></param>
        /// <param name="strExt"></param>
        public void WriteTaskRecord(string strColumnHeader, string strExecSql, string strExt)
        {
            string strSql = "insert into EM_DownloadFile (ext,createDate,countNum,sql,ColumnHeader,state)";
            strSql += "values('" + strExt + "',to_date('" + DateTime.Now.ToString() + "', 'YYYY-MM-DD HH24:MI:SS')" + ",0,'" + strExecSql + "','" + strColumnHeader + "',0)";
            using (var session = DatabaseSession.OpenSession())
            {
                try
                {
                    int intExe = session.Execute(strSql, null, null, null, null);
                    if (intExe <= 0)
                    {
                        //  Logger.Debug("str:" + strSql + ",保存失败！");//日志文件
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Debug("str:" + strSql + ",保存失败！error:" + ex);//日志文件
                }
            }
        }
        #endregion

        #region 获取datatable所占内存大小
        /// <summary>
        /// 获取datatable所占内存大小
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public long GetDataTableSize(DataTable dt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(ms, dt);

            ms.Position = 0;
            return ms.Length;
        }
        #endregion

        #region 根据Datatable集合分文件
        /// <summary>
        /// 对Datatable分文件
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <returns></returns>
        public static DataTable GetPageData(ref DataTable dt)
        {
            DataTable ExecDt = dt.Clone();
            if (dt.Rows.Count > maxRow) //如果大于指定的行就分文件保存
            {
                for (int i = 0; i < maxRow; i++)
                {
                    DataRow row = ExecDt.NewRow();
                    foreach (DataColumn item in dt.Columns)
                    {
                        row[item.ColumnName] = dt.Rows[0][item.ColumnName];
                    }
                    ExecDt.Rows.Add(row);
                    dt.Rows.RemoveAt(0);
                }
            }
            else
            {
                ExecDt = dt.Copy();
                dt.Clear();
             
            }
            return ExecDt;
        }

        #endregion
        
    }
}