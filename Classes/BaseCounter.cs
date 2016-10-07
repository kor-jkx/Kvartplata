// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.BaseCounter
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class BaseCounter
  {
    private int id;
    private string name;

    public virtual int Id
    {
      get
      {
        return this.id;
      }
      set
      {
        this.id = value;
      }
    }

    public virtual string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
      }
    }
  }
}
