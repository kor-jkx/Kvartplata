// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Str
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Str
  {
    private int idStr;
    private string nameStr;
    private string nameStr2;
    private int _idCity;

    public virtual int IdStr
    {
      get
      {
        return this.idStr;
      }
      set
      {
        this.idStr = value;
      }
    }

    public virtual string NameStr
    {
      get
      {
        return this.nameStr;
      }
      set
      {
        this.nameStr = value;
      }
    }

    public virtual string NameStr2
    {
      get
      {
        return this.nameStr2;
      }
      set
      {
        this.nameStr2 = value;
      }
    }

    public virtual int IdCity
    {
      get
      {
        return this._idCity;
      }
      set
      {
        this._idCity = value;
      }
    }
  }
}
