using Aspose.Cells;
using Aspose.Cells.Rendering;
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Easyman.Web.App_Start
{
    public class AsPoseHelper
    {
        //将word文档转换成pdf
        public static string GetPdfFromWord(string soursefilepath,string scode)
        {
            //读取word文档
            try
            {
                using (System.IO.Stream stream = new System.IO.FileStream(soursefilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    Aspose.Words.Document doc = new Aspose.Words.Document(soursefilepath);
                    string htmlName = scode + ".pdf";//Path.GetFileNameWithoutExtension(physicalPath)
                    string outpdfpath = System.Web.HttpContext.Current.Server.MapPath("\\pdfjs\\pdf") + "\\" + htmlName;
                    doc.Save(outpdfpath, Aspose.Words.SaveFormat.Pdf);
                    return outpdfpath;

                }
            }
            catch (Exception ex)
            {
                return "error!"+ex.Message;
            }
        }
        //excel文档转换问题
        public static string GetPdfFromExcel(string soursefilepath, string scode)
        {
            try
            {
                using (System.IO.Stream stream = new System.IO.FileStream(soursefilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    Aspose.Cells.Workbook workbook = new Workbook(stream);
                    PdfSaveOptions pdfSaveOptions = new PdfSaveOptions();
                    pdfSaveOptions.Compliance = PdfCompliance.PdfA1b;
                    string htmlName = scode + ".pdf";//Path.GetFileNameWithoutExtension(physicalPath)
                    string outpdfpath = System.Web.HttpContext.Current.Server.MapPath("\\pdfjs\\pdf") + "\\" + htmlName;
                    workbook.Save(outpdfpath, Aspose.Cells.SaveFormat.Pdf);
                    return outpdfpath;

                }
            }
            catch (Exception ex)
            {
                return "error!" + ex.Message;
            }

        }

        //excel文档转换问题
        public static string GetPdfFromTxt(string soursefilepath, string scode)
        {
            //读取word文档
            try
            {
                using (StreamReader reader = new StreamReader(soursefilepath, Encoding.GetEncoding("gb2312")))
                {
                    string text = reader.ReadToEnd();
                    Aspose.Words.Document doc = new Aspose.Words.Document();
                    Aspose.Words.DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.Write(text);
                    string htmlName = scode + ".pdf";//Path.GetFileNameWithoutExtension(physicalPath)
                    string outpdfpath = System.Web.HttpContext.Current.Server.MapPath("\\pdfjs\\pdf") + "\\" + htmlName;
                    doc.Save(outpdfpath, Aspose.Words.SaveFormat.Pdf);
                    return outpdfpath;
                }
                
            }
            catch (Exception ex)
            {
                return "error!" + ex.Message;
            }
          
        }
        
    }
}
