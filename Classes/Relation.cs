// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Relation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  public class Relation
  {
    private int relationId;
    private string relationName;

    public virtual int RelationId
    {
      get
      {
        return this.relationId;
      }
      set
      {
        this.relationId = value;
      }
    }

    public virtual string RelationName
    {
      get
      {
        return this.relationName;
      }
      set
      {
        this.relationName = value;
      }
    }
  }
}
