// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Distribute
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Distribute
  {
    private int distributeId;
    private Company company;
    private Home home;
    private Period period;
    private Period month;
    private Service service;
    private int counterId;
    private short scheme;
    private Decimal rent;
    private string note;
    private string uName;
    private DateTime dEdit;

    [Browsable(false)]
    public virtual int DistributeId
    {
      get
      {
        return this.distributeId;
      }
      set
      {
        this.distributeId = value;
      }
    }

    [Browsable(false)]
    public virtual Company Company
    {
      get
      {
        return this.company;
      }
      set
      {
        this.company = value;
      }
    }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        return this.home;
      }
      set
      {
        this.home = value;
      }
    }

    [Browsable(false)]
    public virtual Period Period
    {
      get
      {
        return this.period;
      }
      set
      {
        this.period = value;
      }
    }

    [Browsable(false)]
    public virtual Period Month
    {
      get
      {
        return this.month;
      }
      set
      {
        this.month = value;
      }
    }

    [Browsable(false)]
    public virtual int CounterId
    {
      get
      {
        return this.counterId;
      }
      set
      {
        this.counterId = value;
      }
    }

    [Browsable(false)]
    public virtual Service Service
    {
      get
      {
        return this.service;
      }
      set
      {
        this.service = value;
      }
    }

    [Browsable(false)]
    public virtual Decimal Rent
    {
      get
      {
        return this.rent;
      }
      set
      {
        this.rent = value;
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

    [Browsable(false)]
    public virtual short Scheme
    {
      get
      {
        return this.scheme;
      }
      set
      {
        this.scheme = value;
      }
    }
  }
}
