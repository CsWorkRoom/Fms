using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Web;

namespace Em.Project.Common.Helper
{
    /// <summary>
    /// 工具帮助类
    /// </summary>
    public static class ToolHelper
    {
        /// <summary>
        /// 生成图片水印
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static byte[] CreateWatermark(string message, string userName)
        {
            var mesByte = Encoding.UTF8.GetBytes(message);
            Bitmap msgTxt = new Bitmap(240, 60);
            Graphics gTxt = Graphics.FromImage(msgTxt);
            try
            {
                gTxt.Clear(Color.White);
                gTxt.DrawString(message, new Font("黑体", 16), new SolidBrush(Color.FromArgb(219, 219, 234)), 0, 0);
                gTxt.DrawString(userName + "    " + DateTime.Now.ToString("yyyy-MM-dd"), new Font("Arial", 12),
                    new SolidBrush(Color.FromArgb(219, 219, 234)), 0, 30);

                PixelFormat pf = msgTxt.PixelFormat;
                Bitmap tmp = new Bitmap(msgTxt.Width, msgTxt.Height, pf);
                Graphics g = Graphics.FromImage(tmp);
                g.DrawImageUnscaled(msgTxt, 16, 10);
                g.Dispose();

                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new RectangleF(0f, 0f, msgTxt.Width, msgTxt.Height));
                Matrix mtrx = new Matrix();
                mtrx.Rotate(45);
                RectangleF rct = path.GetBounds(mtrx);

                Bitmap dst = new Bitmap(240, 240, pf);
                g = Graphics.FromImage(dst);
                g.Clear(Color.White);
                g.TranslateTransform(-rct.Y, 135);
                g.RotateTransform(-45);
                g.DrawImageUnscaled(tmp, 0, 0);
                g.Dispose();

                //msgTxt = KiRotate(msgTxt, 30);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                dst.Save(stream, ImageFormat.Png);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                gTxt.Dispose();
                msgTxt.Dispose();
            }
        }

        public static void GenerateVerifyCode()
        {
            int codeW = 80;
            int codeH = 22;
            int fontSize = 16;
            string chkCode = string.Empty;

            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };

            //字体列表，用于验证码 
            string[] font = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };

            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();

            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }

            //写入Session
            HttpContext.Current.Session["VerifyCode"] = chkCode;

            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            //画噪线 
            for (int i = 0; i < 1; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }

            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18 + 2, (float)0);
            }

            //画噪点 
            for (int i = 0; i < 100; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }

            //清除该页输出缓存，设置该页无缓存 
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");

            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Png";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                //显式释放资源 
                bmp.Dispose();
                g.Dispose();
            }
        }

        public static bool MatchVerifyCode(string verifyCode)
        {
            if (string.IsNullOrEmpty(verifyCode))
            {
                //HttpContext.Current.Session.Remove("VerifyCode");
                return false;
            }

            var sessionCode = HttpContext.Current.Session["VerifyCode"].ToString();

            if (string.IsNullOrEmpty(sessionCode))
            {
                return false;
            }

            if (verifyCode.ToLower().Equals(sessionCode.ToLower()))
            {
                HttpContext.Current.Session.Remove("VerifyCode");
                return true;
            }

            HttpContext.Current.Session.Remove("VerifyCode");
            return false;
        }
    }
}
