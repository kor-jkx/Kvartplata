﻿// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsSupplier
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsSupplier
  {
    private LsClient lsClient;
    private Period period;
    private Service service;
    private DateTime dBeg;
    private DateTime dEnd;
    private Supplier supplier;
    private string uname;
    private DateTime dedit;

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
    public virtual DateTime DBeg
    {
      get
      {
        return this.dBeg;
      }
      set
      {
        this.dBeg = value;
      }
    }

    [Browsable(false)]
    public virtual DateTime DEnd
    {
      get
      {
        return this.dEnd;
      }
      set
      {
        this.dEnd = value;
      }
    }

    [Browsable(false)]
    public virtual Supplier Supplier
    {
      get
      {
        return this.supplier;
      }
      set
      {
        this.supplier = value;
      }
    }

    public virtual string Uname
    {
      get
      {
        return this.uname;
      }
      set
      {
        if (this.uname == value)
          return;
        this.uname = value;
      }
    }

    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        if (this.dedit == value)
          return;
        this.dedit = value;
      }
    }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual bool IsInsert { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      LsService lsService = obj as LsService;
      return lsService != null && (this.lsClient.ClientId == lsService.Client.ClientId && this.period.PeriodId == lsService.Period.PeriodId && this.dBeg == lsService.DBeg && (int) this.service.ServiceId == (int) lsService.Service.ServiceId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.lsClient.ClientId;
      int num2 = this.lsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int serviceId = (int) this.service.ServiceId;
      int hashCode4 = this.service.ServiceId.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
