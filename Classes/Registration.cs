// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Registration
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Registration
  {
    private int regId;
    private string regName;

    public virtual int RegId
    {
      get
      {
        return this.regId;
      }
      set
      {
        this.regId = value;
      }
    }

    public virtual string RegName
    {
      get
      {
        return this.regName;
      }
      set
      {
        this.regName = value;
      }
    }
  }
}
