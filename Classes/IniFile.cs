// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.IniFile
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.Runtime.InteropServices;
using System.Text;

namespace Kvartplata.Classes
{
  public class IniFile
  {
    public string path;

    public IniFile(string INIPath)
    {
      this.path = INIPath;
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public void IniWriteValue(string Section, string Key, string Value)
    {
      IniFile.WritePrivateProfileString(Section, Key, Value, this.path);
    }

    public string IniReadValue(string Section, string Key)
    {
      StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
      IniFile.GetPrivateProfileString(Section, Key, "", retVal, (int) byte.MaxValue, this.path);
      return retVal.ToString();
    }
  }
}
