// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.BalancePeni
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  internal class BalancePeni
  {
    private LsClient lsClient;
    private Period period;
    private Service service;
    private Supplier supplier;
    private double balanceIn;
    private double rent;
    private double correct;
    private double payment;
    private double balanceOut;
    private double rentFull;

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

    public virtual double BalanceIn
    {
      get
      {
        return this.balanceIn;
      }
      set
      {
        this.balanceIn = value;
      }
    }

    public virtual double Rent
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

    public virtual double Correct
    {
      get
      {
        return this.correct;
      }
      set
      {
        this.correct = value;
      }
    }

    public virtual double Payment
    {
      get
      {
        return this.payment;
      }
      set
      {
        this.payment = value;
      }
    }

    public virtual double BalanceOut
    {
      get
      {
        return this.balanceOut;
      }
      set
      {
        this.balanceOut = value;
      }
    }

    public virtual double RentFull
    {
      get
      {
        return this.rentFull;
      }
      set
      {
        this.rentFull = value;
      }
    }

    public virtual string ServiceName
    {
      get
      {
        if (this.service != null)
          return this.service.ServiceName;
        return (string) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      BalancePeni balancePeni = obj as BalancePeni;
      return balancePeni != null && (this.lsClient.ClientId == balancePeni.LsClient.ClientId && this.period.PeriodId == balancePeni.Period.PeriodId && (int) this.service.ServiceId == (int) balancePeni.Service.ServiceId && this.supplier.SupplierId == balancePeni.Supplier.SupplierId);
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
      int serviceId = (int) this.service.ServiceId;
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int supplierId = this.supplier.SupplierId;
      num2 = this.supplier.SupplierId;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
