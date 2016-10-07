// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsService
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsService
  {
    private LsClient client;
    private DateTime dBeg;
    private DateTime dEnd;
    private DateTime dedit;
    private Period period;
    private Service service;
    private Tariff tariff;

    [Browsable(false)]
    public virtual string TariffName
    {
      get
      {
        return this.tariff.Tariff_name;
      }
      set
      {
        this.tariff.Tariff_name = value;
      }
    }

    [Browsable(false)]
    public virtual string ServiceName
    {
      get
      {
        return this.service.ServiceName;
      }
      set
      {
        this.service.ServiceName = value;
      }
    }

    [Browsable(false)]
    public virtual LsClient Client
    {
      get
      {
        return this.client;
      }
      set
      {
        this.client = value;
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
        this.dBeg = new DateTime();
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
        this.dEnd = new DateTime();
        this.dEnd = value;
      }
    }

    [Browsable(false)]
    public virtual Tariff Tariff
    {
      get
      {
        return this.tariff;
      }
      set
      {
        this.tariff = value;
      }
    }

    [Browsable(false)]
    public virtual Norm Norm { get; set; }

    [Browsable(false)]
    public virtual Complex Complex { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit
    {
      get
      {
        return this.dedit;
      }
      set
      {
        this.dedit = new DateTime();
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
      return lsService != null && (this.Client.ClientId == lsService.Client.ClientId && this.Period.PeriodId == lsService.Period.PeriodId && this.DBeg == lsService.DBeg && (int) this.Service.ServiceId == (int) lsService.Service.ServiceId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.client.ClientId;
      int num2 = this.client.ClientId;
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
