// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Person
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Person
  {
    private int personId;
    private LsClient lsClient;
    private string family;
    private string name;
    private string lastName;
    private Registration reg;
    private Relation relation;
    private DateTime? firstPropDate;
    private DateTime? outToDate;
    private DateTime? bornDate;
    private DateTime? lastPropDate;
    private DateTime? regDate;
    private DateTime? regOutDate;
    private DateTime? dieDate;
    private int archive;
    private DateTime? regDEdit;
    private DateTime? outDEdit;
    private Owner owner;
    private string uNameReg;
    private string uNameUnReg;
    private string snils;
    private int familyNum;
    private int? consent;
    private Guild guild;
    private string number;
    private BaseOrg baseOrg;

    public virtual int PersonId
    {
      get
      {
        return this.personId;
      }
      set
      {
        this.personId = value;
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

    [Browsable(false)]
    public virtual Registration Reg
    {
      get
      {
        return this.reg;
      }
      set
      {
        this.reg = value;
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
    public virtual DateTime? LastPropDate
    {
      get
      {
        return this.lastPropDate;
      }
      set
      {
        this.lastPropDate = value;
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
    public virtual DateTime? DieDate
    {
      get
      {
        return this.dieDate;
      }
      set
      {
        this.dieDate = value;
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

    public virtual string FIO
    {
      get
      {
        return this.family + " " + this.name + " " + this.lastName + " " + (this.OutToDate.HasValue ? this.OutToDate.Value.ToShortDateString() : "");
      }
    }

    [Browsable(false)]
    public virtual string Snils
    {
      get
      {
        return this.snils;
      }
      set
      {
        this.snils = value;
      }
    }

    [Browsable(false)]
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
    public virtual Guild Guild { get; set; }

    public virtual string Number { get; set; }

    [Browsable(false)]
    public virtual BaseOrg BaseOrg { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }
  }
}
