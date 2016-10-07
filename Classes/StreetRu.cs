// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.StreetRu
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class StreetRu
  {
    public virtual string StrId { get; set; }

    public virtual string StrName { get; set; }

    public virtual int NumStr { get; set; }

    public virtual int PrinNumStr { get; set; }

    public virtual string Abbr { get; set; }

    public virtual string FullName
    {
      get
      {
        return this.StrName + " " + this.Abbr;
      }
    }

    public StreetRu()
    {
    }

    public StreetRu(string id, string name)
    {
      this.StrId = id;
      this.StrName = name;
    }
  }
}
