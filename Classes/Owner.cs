// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Owner
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Owner
  {
    private int ownerId;
    private LsClient lsClient;
    private string family;
    private string name;
    private string lastName;
    private DateTime? firstPropDate;
    private DateTime? outToDate;
    private DateTime? bornDate;
    private DateTime? regDate;
    private DateTime? regOutDate;
    private Relation relation;
    private int archive;
    private DateTime? regDEdit;
    private DateTime? outDEdit;
    private string uNameReg;
    private string uNameUnReg;
    private int familyNum;
    private string note;
    private int? consent;

    [Browsable(false)]
    public virtual int OwnerId
    {
      get
      {
        return this.ownerId;
      }
      set
      {
        this.ownerId = value;
      }
    }

    [Browsable(false)]
    public virtual LsClient LsClient
    {
      get
      {
        return this.lsClient;
      }
      set
      {
        this.lsClient = value;
      }
    }

    public virtual string Family
    {
      get
      {
        return this.family;
      }
      set
      {
        this.family = value;
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

    public virtual string LastName
    {
      get
      {
        return this.lastName;
      }
      set
      {
        this.lastName = value;
      }
    }

    public virtual DateTime? FirstPropDate
    {
      get
      {
        return this.firstPropDate;
      }
      set
      {
        this.firstPropDate = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? OutToDate
    {
      get
      {
        return this.outToDate;
      }
      set
      {
        this.outToDate = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? BornDate
    {
      get
      {
        return this.bornDate;
      }
      set
      {
        this.bornDate = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? RegDate
    {
      get
      {
        return this.regDate;
      }
      set
      {
        this.regDate = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? RegOutDate
    {
      get
      {
        return this.regOutDate;
      }
      set
      {
        this.regOutDate = value;
      }
    }

    [Browsable(false)]
    public virtual Relation Relation
    {
      get
      {
        return this.relation;
      }
      set
      {
        this.relation = value;
      }
    }

    [Browsable(false)]
    public virtual int Archive
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
    public virtual DateTime? RegDEdit
    {
      get
      {
        return this.regDEdit;
      }
      set
      {
        this.regDEdit = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime? OutDEdit
    {
      get
      {
        return this.outDEdit;
      }
      set
      {
        this.outDEdit = value;
      }
    }

    [Browsable(false)]
    public virtual string UNameReg
    {
      get
      {
        return this.uNameReg;
      }
      set
      {
        this.uNameReg = value;
      }
    }

    [Browsable(false)]
    public virtual string UNameUnReg
    {
      get
      {
        return this.uNameUnReg;
      }
      set
      {
        this.uNameUnReg = value;
      }
    }

    public virtual int FamilyNum
    {
      get
      {
        return this.familyNum;
      }
      set
      {
        this.familyNum = value;
      }
    }

    public virtual string Note
    {
      get
      {
        return this.note;
      }
      set
      {
        this.note = value;
      }
    }

    [Browsable(false)]
    public virtual int? Consent
    {
      get
      {
        return this.consent;
      }
      set
      {
        this.consent = value;
      }
    }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }
  }
}
