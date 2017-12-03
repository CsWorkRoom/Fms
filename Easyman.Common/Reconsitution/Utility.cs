using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Easyman.Common.Reconsitution
{
    public static class Utility
    {
        const string Key64 = "VavicApp";//注意了，是8个字符，64位

        const string Iv64 = "VavicApp";

        public static string GetEncrypt(string input)
        {
            var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            var result = new StringBuilder();

            foreach (var t in data)
            {
                result.AppendFormat("{0:X2}", t);
            }

            return result.ToString();
        }

        public static string EncryptUserPassword(string text)
        {
            var data = text.ToCharArray();
            var buffer = new byte[data.Length];
            var sb = new StringBuilder();
            HashAlgorithm sha = new SHA1CryptoServiceProvider();

            for (var i = 0; i < data.Length; ++i)
                buffer[i] = (byte)data[i];

            buffer = sha.ComputeHash(buffer);
            foreach (var b in buffer)
                sb.AppendFormat("{0:X2}", b);

            return sb.ToString();
        }

        public static string Encode(string data)
        {
            var byKey = System.Text.Encoding.ASCII.GetBytes(Key64);
            var byIv = System.Text.Encoding.ASCII.GetBytes(Iv64);

            var cryptoProvider = new DESCryptoServiceProvider();
            var i = cryptoProvider.KeySize;
            var ms = new MemoryStream();
            var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIv), CryptoStreamMode.Write);

            var sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }

        public static string Decode(string data)
        {
            var byKey = System.Text.Encoding.ASCII.GetBytes(Key64);
            var byIv = System.Text.Encoding.ASCII.GetBytes(Iv64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream(byEnc);
            var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIv), CryptoStreamMode.Read);
            var sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
    }
}
