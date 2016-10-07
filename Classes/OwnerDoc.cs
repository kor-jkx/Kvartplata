// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.OwnerDoc
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class OwnerDoc
  {
    private int ownerDocId;
    private Owner owner;
    private RightDoc rightDoc;
    private string percent;
    private string docNum;
    private DateTime? docDate;
    private OwnDoc ownDoc;
    private string seria;
    private string number;
    private DateTime? date;
    private YesNo archive;
    private string uName;
    private DateTime dEdit;

    public virtual int OwnerDocId
    {
      get
      {
        return this.ownerDocId;
      }
      set
      {
        this.ownerDocId = value;
      }
    }

    [Browsable(false)]
    public virtual Owner Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        this.owner = value;
      }
    }

    [Browsable(false)]
    public virtual RightDoc RightDoc
    {
      get
      {
        return this.rightDoc;
      }
      set
      {
        this.rightDoc = value;
      }
    }

    public virtual string Percent
    {
      get
      {
        return this.percent;
      }
      set
      {
        this.percent = value;
      }
    }

    public virtual string DocNum
    {
      get
      {
        return this.docNum;
      }
      set
      {
        this.docNum = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? DocDate
    {
      get
      {
        return this.docDate;
      }
      set
      {
        this.docDate = value;
      }
    }

    [Browsable(false)]
    public virtual OwnDoc OwnDoc
    {
      get
      {
        return this.ownDoc;
      }
      set
      {
        this.ownDoc = value;
      }
    }

    public virtual string Seria
    {
      get
      {
        return this.seria;
      }
      set
      {
        this.seria = value;
      }
    }

    public virtual string Number
    {
      get
      {
        return this.number;
      }
      set
      {
        this.number = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? Date
    {
      get
      {
        return this.date;
      }
      set
      {
        this.date = value;
      }
    }

    [Browsable(false)]
    public virtual YesNo Archive
    {
      get
      {
        return this.archive;
      }
      set
      {
        this.archive = value;
      }
    }

    [Browsable(false)]
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

    [Browsable(false)]
    public virtual DateTime DEdit
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
