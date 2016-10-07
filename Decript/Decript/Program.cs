using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GenUinKv;

namespace Decript
{
    class Program
    {
        public static string getMd5Hash(string input)
        {
            byte[] hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return stringBuilder.ToString();
        }
        static void Main(string[] args)
        {
            GetInformations getInformations = new GetInformations();
            string str1 = getInformations.GetHddSerial().Trim(' ');
            string str6 = str1.Trim(' ');
            string input2 = str6;
            string md5Hash1 = getMd5Hash(input2);

            string data = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "skey.dat");
            string str = Decrypt(data, "RFVtgbYHN");
            string ssa = Encrypt(md5Hash1, "RFVtgbYHN");
            string ret = "ddf";
        }
        public static byte[] Encrypt(byte[] data, string password)
        {
            ICryptoTransform encryptor = Rijndael.Create().CreateEncryptor(new PasswordDeriveBytes(password, (byte[])null).GetBytes(16), new byte[16]);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }
        public static string Encrypt(string data, string password)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), password));
        }

        public static byte[] Decrypt(byte[] data, string password)
        {
            BinaryReader binaryReader = new BinaryReader((Stream)InternalDecrypt(data, password));
            return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
        }

        public static string Decrypt(string data, string password)
        {
            return new StreamReader((Stream)Decript.Program.InternalDecrypt(Convert.FromBase64String(data), password)).ReadToEnd();
        }

        private static CryptoStream InternalDecrypt(byte[] data, string password)
        {
            ICryptoTransform decryptor = Rijndael.Create().CreateDecryptor(new PasswordDeriveBytes(password, (byte[])null).GetBytes(16), new byte[16]);
            return new CryptoStream((Stream)new MemoryStream(data), decryptor, CryptoStreamMode.Read);
        }
    }
}
