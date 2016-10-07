// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ClassTable
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class ClassTable
  {
    private int tableId;
    private string className;
    private string tableValueField;
    private string tableDisplayField;

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

    public virtual string ClassValueField
    {
      get
      {
        return this.tableValueField;
      }
      set
      {
        this.tableValueField = value;
      }
    }

    public virtual string ClassDisplayField
    {
      get
      {
        return this.tableDisplayField;
      }
      set
      {
        this.tableDisplayField = value;
      }
    }
  }
}
