// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Rent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Rent
  {
    private Period period;
    private LsClient lsClient;
    private Service service;
    private Supplier supplier;
    private Period month;
    private short code;
    private int motive;
    private double volume;
    private double rent;
    private double rentEO;
    private double rentVat;
    private short rentType;

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
    public virtual short Code
    {
      get
      {
        return this.code;
      }
      set
      {
        this.code = value;
      }
    }

    [Browsable(false)]
    public virtual int Motive
    {
      get
      {
        return this.motive;
      }
      set
      {
        this.motive = value;
      }
    }

    [Browsable(false)]
    public virtual double Volume
    {
      get
      {
        return this.volume;
      }
      set
      {
        this.volume = value;
      }
    }

    [Browsable(false)]
    public virtual double RentMain
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

    [Browsable(false)]
    public virtual double RentEO
    {
      get
      {
        return this.rentEO;
      }
      set
      {
        this.rentEO = value;
      }
    }

    [Browsable(false)]
    public virtual double RentVat { get; set; }

    [Browsable(false)]
    public virtual short RentType { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Rent rent = obj as Rent;
      return rent != null && (this.lsClient.ClientId == rent.LsClient.ClientId && this.period.PeriodId == rent.Period.PeriodId && ((int) this.service.ServiceId == (int) rent.Service.ServiceId && this.supplier.SupplierId == rent.Supplier.SupplierId) && (this.month.PeriodId == rent.Month.PeriodId && (int) this.code == (int) rent.Code && this.motive == rent.Motive) && (int) this.rentType == (int) rent.rentType);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.lsClient.ClientId;
      int num2 = this.lsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId1 = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId = (int) this.service.ServiceId;
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int supplierId = this.supplier.SupplierId;
      num2 = this.supplier.SupplierId;
      int hashCode4 = num2.GetHashCode();
      int num6 = num5 + hashCode4;
      int periodId2 = this.month.PeriodId;
      num2 = this.month.PeriodId;
      int hashCode5 = num2.GetHashCode();
      int num7 = num6 + hashCode5;
      int code = (int) this.code;
      int hashCode6 = this.code.GetHashCode();
      int num8 = num7 + hashCode6;
      int motive = this.motive;
      int hashCode7 = this.motive.GetHashCode();
      int num9 = num8 + hashCode7;
      int rentType = (int) this.rentType;
      int hashCode8 = this.rentType.GetHashCode();
      return num9 + hashCode8;
    }
  }
}
