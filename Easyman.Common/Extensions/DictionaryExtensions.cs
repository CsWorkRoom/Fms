using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMan.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 移动元素
        /// </summary>
        public static void MoveItem<T>(this IList<T> source, int from, int to)
        {
            if (from == to)
                return;
            if (from > to)
            {
                T temp = source[from];
                for (int i = from - 1; i >= to; i--)
                    source[i + 1] = source[i];
                source[to] = temp;
            }
            else
            {
                T temp = source[from];
                for (int i = to - 1; i >= from; i--)
                    source[i + 1] = source[i];
                source[to] = temp;
            }
        }

        /// <summary>
        /// 随机获取数组元素
        /// </summary>
        /// <param name="array">当前数组</param>
        /// <param name="num">需要的数组元素个数,小于等于数组大小</param>
        /// <returns>随机取到的数组</returns>
        public static ArrayList Random(this ArrayList array, int num)
        {
            ArrayList list = new ArrayList();

            int count = array.Count;
            if (num > count)
            {
                return list;
            }
            else
            {
                int m = num;
                int[] have = new int[m];
                for (int i = 0; i < num; i++)
                {
                    RandomNum(count, ref have, i);
                }

                foreach (int k in have)
                {
                    list.Add(array[k]);
                }
                return list;
            }
        }

        /// <summary>
        /// 生成按 逗号 分隔的字符串
        /// </summary>
        public static string GetDelimitedText(this IList source)
        {
            return GetListDelimitedText(source);
        }

        /// <summary>
        /// 赋值按 逗号 分隔的字符串
        /// </summary>
        public static void SetDelimitedText(this IList source, string delimitedText)
        {
            SetListDelimitedText(source, delimitedText, false);
        }


        #region
        private static void SetListDelimitedText(IList list, string delimitedText, bool isAddMode)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            const char cCharDelimiter = ',';
            const char cCharQuoteChar = '"';
            int index = 0;
            int len = delimitedText.Length;

            if (!isAddMode)
                list.Clear();
            while (index < len && index > -1)
            {
                string s;
                if (delimitedText[index] == cCharQuoteChar)
                {
                    s = FromQuote(delimitedText, cCharQuoteChar, ref index);
                }
                else
                {
                    int p = delimitedText.IndexOf(cCharDelimiter, index, len - index);
                    if (p != -1)
                    {
                        s = delimitedText.Substring(index, p - index);
                        p++;
                    }
                    else
                        s = delimitedText.Substring(index, len - index);
                    index = p;
                }
                list.Add(s);
                if (index > -1 && index < len && delimitedText[index] == cCharDelimiter)
                    index++;
            }
        }

        private static string FromQuote(string source, char quoteChar, ref int refIndex)
        {
            if (string.IsNullOrEmpty(source))
            {
                refIndex = -1;
                return "";
            }
            if (refIndex < 0 || refIndex >= source.Length)
            {
                refIndex = -1;
                return "";
            }
            if (source[refIndex] != quoteChar)
            {
                int index = refIndex;
                refIndex = -1;
                return source.Substring(index, source.Length - index);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                int index = refIndex + 1;
                while (index < source.Length)
                {
                    if (source[index] == quoteChar)
                    {
                        index++;
                        if (index == source.Length)
                            break;
                        if (source[index] == quoteChar)
                        {
                            sb.Append(source[index]);
                            index++;
                        }
                        else
                            break;
                    }
                    else
                    {
                        sb.Append(source[index]);
                        index++;
                    }
                }
                if (index < source.Length)
                    refIndex = index;
                else
                    refIndex = -1;
                return sb.ToString();
            }
        }

        private static string GetListDelimitedText(IEnumerable list)
        {
            if (list == null)
                return string.Empty;
            const int cSpaceCharAsTMi = (int)' ';
            const char cCharDelimiter = ',';
            const char cCharQuoteChar = '"';

            StringBuilder sb = new StringBuilder();
            foreach (object o in list)
            {
                bool vIsNeedQuote = false;
                string s = o.ToString();
                foreach (char c in s)
                {
                    int vCharAsTMi = c;
                    if (vCharAsTMi >= 0 && vCharAsTMi <= cSpaceCharAsTMi ||
                        c == cCharQuoteChar || c == cCharDelimiter)
                    {
                        vIsNeedQuote = true;
                        break;
                    }
                }
                if (vIsNeedQuote)
                    sb.Append(ToQuote(s, cCharQuoteChar)).Append(cCharDelimiter);
                else
                    sb.Append(s).Append(cCharDelimiter);
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.Length == 0 ? string.Empty : sb.ToString();
        }

        private static string ToQuote(string source, char quoteChar)
        {
            if (string.IsNullOrEmpty(source))
                return quoteChar.ToString() + quoteChar;
            if (source.IndexOf(quoteChar) == -1)
                return quoteChar + source + quoteChar;
            int vQuoteCharAsTMi = quoteChar;
            StringBuilder sb = new StringBuilder();
            sb.Append(quoteChar);
            foreach (char c in source)
            {
                if (c == vQuoteCharAsTMi)
                    sb.Append(c);
                sb.Append(c);
            }
            sb.Append(quoteChar);
            return sb.ToString();
        }

        private static void RandomNum(int count, ref int[] have, int index)
        {
            Random random = new Random();
            int n = random.Next(0, count);
            if (have.Contains(n) == true)
            {
                RandomNum(count, ref have, index);
            }
            else
            {
                have.SetValue(n, index);
            }
        }


        #endregion
    }
}
