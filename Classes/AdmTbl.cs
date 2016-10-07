// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.AdmTbl
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class AdmTbl
  {
    private string className;
    private int tableId;

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

    public virtual string TableName { get; set; }

    public virtual string ClassName
    {
      get
      {
        return this.className;
      }
      set
      {
        this.className = value;
      }
    }

    public virtual string ClassNameId
    {
      get
      {
        if (this.tableId == 894)
          return this.className + "_id";
        return this.className + "Id";
      }
    }

    public virtual string ClassNameName
    {
      get
      {
        if (this.tableId == 894)
          return this.className + "_name";
        return this.className + "Name";
      }
    }
  }
}
