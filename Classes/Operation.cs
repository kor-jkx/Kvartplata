// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Operation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Operation
  {
    private int fKId;
    private int oprId;
    private string oprName;

    public virtual int FKId
    {
      get
      {
        return this.fKId;
      }
      set
      {
        this.fKId = value;
      }
    }

    public virtual int OprId
    {
      get
      {
        return this.oprId;
      }
      set
      {
        this.oprId = value;
      }
    }

    public virtual string OprName
    {
      get
      {
        return this.oprName;
      }
      set
      {
        this.oprName = value;
      }
    }
  }
}
