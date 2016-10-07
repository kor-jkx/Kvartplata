// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Dog
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Dog
  {
    private short docId;
    private string docName;
    private short? section;
    private string uName;
    private DateTime? dEdit;

    public virtual short DocId
    {
      get
      {
        return this.docId;
      }
      set
      {
        this.docId = value;
      }
    }

    public virtual string DocName
    {
      get
      {
        return this.docName;
      }
      set
      {
        this.docName = value;
      }
    }

    public virtual short? Section
    {
      get
      {
        return this.section;
      }
      set
      {
        this.section = value;
      }
    }

    public virtual string UName
    {
      get
      {
        return this.uName;
      }
      set
      {
        this.uName = value;
      }
    }

    public virtual DateTime? DEdit
    {
      get
      {
        return this.dEdit;
      }
      set
      {
        this.dEdit = value;
      }
    }
  }
}
