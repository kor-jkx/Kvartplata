// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Company
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Company
  {
    private short companyId;
    private string companyName;
    private string companySName;
    private Raion raion;
    private BaseOrg manager;
    private string workTime;
    private string workTimeCash;
    private string workPlaceCash;
    private string address;
    private string socOrgId;
    private string uName;
    private DateTime dEdit;

    public virtual short CompanyId
    {
      get
      {
        return this.companyId;
      }
      set
      {
        this.companyId = value;
      }
    }

    public virtual string CompanyName
    {
      get
      {
        return this.companyName;
      }
      set
      {
        this.companyName = value;
      }
    }

    public virtual string CompanySName
    {
      get
      {
        return this.companySName;
      }
      set
      {
        this.companySName = value;
      }
    }

    [Browsable(false)]
    public virtual BaseOrg Manager
    {
      get
      {
        return this.manager;
      }
      set
      {
        this.manager = value;
      }
    }

    [Browsable(false)]
    public virtual Raion Raion
    {
      get
      {
        return this.raion;
      }
      set
      {
        this.raion = value;
      }
    }

    public virtual string WorkTime
    {
      get
      {
        return this.workTime;
      }
      set
      {
        this.workTime = value;
      }
    }

    public virtual string WorkTimeCash
    {
      get
      {
        return this.workTimeCash;
      }
      set
      {
        this.workTimeCash = value;
      }
    }

    public virtual string WorkPlaceCash
    {
      get
      {
        return this.workPlaceCash;
      }
      set
      {
        this.workPlaceCash = value;
      }
    }

    public virtual string Address
    {
      get
      {
        return this.address;
      }
      set
      {
        this.address = value;
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

    public virtual string SocOrgId
    {
      get
      {
        return this.socOrgId;
      }
      set
      {
        this.socOrgId = value;
      }
    }

    public virtual string CompanyNameAndNum
    {
      get
      {
        return this.companyName + " (" + (object) this.companyId + ")";
      }
    }

    public Company()
    {
    }

    public Company(short id, Raion raion)
    {
      this.companyId = id;
      this.raion = raion;
    }

    public Company(short id, string name)
    {
      this.companyId = id;
      this.CompanyName = name;
    }
  }
}
