using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rohr.EPC.Business
{
    public class CriptografiaBusiness
    {
        static byte[] _chave = { };
        static readonly byte[] Iv = { 12, 34, 56, 78, 90, 102, 114, 126 };

        public static string Criptografar(string valor)
        {
            return Criptografar(valor, "#$&%#$&%");
        }
        public static string Criptografar(string valor, string chaveCriptografia)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();

            byte[] input = Encoding.UTF8.GetBytes(valor); _chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(_chave, Iv), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Descriptografar(string valor)
        {
            return Descriptografar(valor, "#$&%#$&%");
        }
        public static string Descriptografar(string valor, string chaveCriptografia)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();

            byte[] input = Convert.FromBase64String(valor.Replace(" ", "+"));

            _chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(_chave, Iv), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
