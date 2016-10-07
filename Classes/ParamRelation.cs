// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ParamRelation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class ParamRelation
  {
    private int tableId;
    private short paramId;

    public virtual int TableId
    {
      get
      {
        return this.tableId;
      }
      set
      {
        this.tableId = value;
      }
    }

    public virtual short ParamId
    {
      get
      {
        return this.paramId;
      }
      set
      {
        this.paramId = value;
      }
    }
  }
}
