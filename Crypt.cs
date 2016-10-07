// Decompiled with JetBrains decompiler
// Type: Kvartplata.Crypt
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kvartplata
{
  public class Crypt
  {
    public static byte[] Encrypt(byte[] data, string password)
    {
      ICryptoTransform encryptor = Rijndael.Create().CreateEncryptor(new PasswordDeriveBytes(password, (byte[]) null).GetBytes(16), new byte[16]);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(data, 0, data.Length);
      cryptoStream.FlushFinalBlock();
      return memoryStream.ToArray();
    }

    public static string Encrypt(string data, string password)
    {
      return Convert.ToBase64String(Crypt.Encrypt(Encoding.UTF8.GetBytes(data), password));
    }

    public static byte[] Decrypt(byte[] data, string password)
    {
      BinaryReader binaryReader = new BinaryReader((Stream) Crypt.InternalDecrypt(data, password));
      return binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
    }

    public static string Decrypt(string data, string password)
    {
      return new StreamReader((Stream) Crypt.InternalDecrypt(Convert.FromBase64String(data), password)).ReadToEnd();
    }

    private static CryptoStream InternalDecrypt(byte[] data, string password)
    {
      ICryptoTransform decryptor = Rijndael.Create().CreateDecryptor(new PasswordDeriveBytes(password, (byte[]) null).GetBytes(16), new byte[16]);
      return new CryptoStream((Stream) new MemoryStream(data), decryptor, CryptoStreamMode.Read);
    }
  }
}
