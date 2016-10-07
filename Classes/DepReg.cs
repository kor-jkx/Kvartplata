// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DepReg
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class DepReg
  {
    private int depRegId;
    private string depRegName;

    public virtual int DepRegId
    {
      get
      {
        return this.depRegId;
      }
      set
      {
        this.depRegId = value;
      }
    }

    public virtual string DepRegName
    {
      get
      {
        return this.depRegName;
      }
      set
      {
        this.depRegName = value;
      }
    }
  }
}
