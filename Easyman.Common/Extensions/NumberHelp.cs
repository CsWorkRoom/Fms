using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMan.Extensions
{
    public class NumberHelp
    {
        /// <summary> 
        /// 输入Float格式数字，将其转换为货币表达方式 
        /// </summary> 
        /// <param name="ftype">货币表达类型：0=带￥的货币表达方式；1=不带￥的货币表达方式；其它=带￥的货币表达方式</param> 
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public string Rmoney(int ftype, double fmoney)
        {
            string rmoney;
            try
            {
                switch (ftype)
                {
                    case 0:
                        rmoney = $"{fmoney:C2}";
                        break;
                    case 1:
                        rmoney = $"{fmoney:N2}";
                        break;
                    default:
                        rmoney = $"{fmoney:C2}";
                        break;
                }
            }
            catch
            {
                rmoney = "";
            }
            return rmoney;
        }

        /// <summary> 
        /// 输入Float格式数字，将其转换为货币表达方式 
        /// </summary>
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public static string ConvertCurrency(decimal fmoney)
        {
            int ftype = 4;
            string rmoney;
            try
            {
                CultureInfo cul = null;
                switch (ftype)
                {
                    case 0:
                        cul = new CultureInfo("zh-CN");//中国大陆 
                        rmoney = fmoney.ToString("c", cul);
                        break;
                    case 1:
                        cul = new CultureInfo("zh-HK");//香港 
                        rmoney = fmoney.ToString("c", cul);
                        break;
                    case 2:
                        cul = new CultureInfo("en-US");//美国 
                        rmoney = fmoney.ToString("c", cul);
                        break;
                    case 3:
                        cul = new CultureInfo("en-GB");//英国 
                        rmoney = fmoney.ToString("c", cul);
                        break;
                    case 4:
                        rmoney = $"{fmoney:n}";//没有货币符号 
                        break;

                    default:
                        rmoney = $"{fmoney:n}";
                        break;
                }
            }
            catch
            {
                rmoney = "";
            }
            return rmoney;
        }


        private static char ChrToNum(char x)
        {
            string chnNames = "零壹贰叁肆伍陆柒捌玖";
            string numNames = "0123456789";
            return chnNames[numNames.IndexOf(x)];
        }
        private static string TenthousandToNum(string x)
        {
            string[] stringArrayLevelNames = new string[4] { "", "拾", "佰", "仟" };
            string ret = "";
            int i;
            for (i = x.Length - 1; i >= 0; i--)
                if (x[i] == '0')
                    ret = ChrToNum(x[i]) + ret;
                else
                    ret = ChrToNum(x[i]) + stringArrayLevelNames[x.Length - 1 - i] + ret;
            while ((i = ret.IndexOf("零零", StringComparison.Ordinal)) != -1)
                ret = ret.Remove(i, 1);
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
                ret = ret.Remove(ret.Length - 1, 1);
            //            if (ret.Length>=2 && ret.Substring(0,2)=="壹拾")
            //                ret=ret.Remove(0,1);
            return ret;
        }
        private static string ChgIntegerPart(string x)
        {
            int len = x.Length;
            string ret, temp;
            if (len <= 4)
                ret = TenthousandToNum(x);
            else if (len <= 8)
            {
                ret = TenthousandToNum(x.Substring(0, len - 4)) + "万";
                temp = TenthousandToNum(x.Substring(len - 4, 4));
                if (temp.IndexOf("仟", StringComparison.Ordinal) == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
            }
            else
            {
                ret = TenthousandToNum(x.Substring(0, len - 8)) + "亿";
                temp = TenthousandToNum(x.Substring(len - 8, 4));
                if (temp.IndexOf("仟", StringComparison.Ordinal) == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
                ret += "万";
                temp = TenthousandToNum(x.Substring(len - 4, 4));
                if (temp.IndexOf("仟", StringComparison.Ordinal) == -1 && temp != "")
                    ret += $"零{temp}";
                else
                    ret += temp;
            }
            int i;
            if ((i = ret.IndexOf("零万", StringComparison.Ordinal)) != -1)
                ret = ret.Remove(i + 1, 1);
            while ((i = ret.IndexOf("零零", StringComparison.Ordinal)) != -1)
                ret = ret.Remove(i, 1);
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
                ret = ret.Remove(ret.Length - 1, 1);
            return ret;
        }

        private static string ChgDecimalPart(string x)
        {
            string ret = "";
            for (int i = 0; i < x.Length && i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        ret += ChrToNum(x[i]) + "角";
                        break;
                    case 1:
                        ret += ChrToNum(x[i]) + "分";
                        break;
                }
            }
            return ret;
        }
        /// <summary>
        /// 将阿拉伯小写金额转换成大写金额
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string NumToChn(string x)
        {
            if (x.Length == 0)
                return "";
            string ret = "";
            if (x[0] == '-')
            {
                ret = "负";
                x = x.Remove(0, 1);
            }
            if (x[0].ToString() == ".")
                x = "0" + x;
            if (x[x.Length - 1].ToString() == ".")
                x = x.Remove(x.Length - 1, 1);
            if (x.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                if (Convert.ToDecimal("0" + x.Substring(x.IndexOf(".", StringComparison.Ordinal))) > 0)
                {
                    ret += ChgIntegerPart(x.Substring(0, x.IndexOf(".", StringComparison.Ordinal))) + "元" + ChgDecimalPart(x.Substring(x.IndexOf(".", StringComparison.Ordinal) + 1));
                }
                else
                {
                    ret += ChgIntegerPart(Convert.ToDecimal(x).ToString("0")) + "元整";
                }
            }
            else
            {
                ret += ChgIntegerPart(x) + "元整";
            }
            return ret;
        }

    }
}
