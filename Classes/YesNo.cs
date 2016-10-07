// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.YesNo
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class YesNo
  {
    private short yesNoId;
    private string yesNoName;

    public virtual short YesNoId
    {
      get
      {
        return this.yesNoId;
      }
      set
      {
        this.yesNoId = value;
      }
    }

    public virtual string YesNoName
    {
      get
      {
        return this.yesNoName;
      }
      set
      {
        this.yesNoName = value;
      }
    }
  }
}
