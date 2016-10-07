// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Fond
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Fond
  {
    private int fondId;
    private string fondName;

    public virtual int FondId
    {
      get
      {
        return this.fondId;
      }
      set
      {
        this.fondId = value;
      }
    }

    public virtual string FondName
    {
      get
      {
        return this.fondName;
      }
      set
      {
        this.fondName = value;
      }
    }
  }
}
