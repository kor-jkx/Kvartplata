// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Balance
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Balance
  {
    private LsClient lsClient;
    private Period period;
    private Service service;
    private Supplier supplier;
    private double balanceIn;
    private double rent;
    private double rentPast;
    private double msp;
    private double mspPast;
    private double payment;
    private double paymentPast;
    private double subsidy;
    private double balanceOut;
    private double rentComp;
    private Period monthDept;

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

    public virtual double RentPast
    {
      get
      {
        return this.rentPast;
      }
      set
      {
        this.rentPast = value;
      }
    }

    public virtual double MSP
    {
      get
      {
        return this.msp;
      }
      set
      {
        this.msp = value;
      }
    }

    public virtual double MSPPast
    {
      get
      {
        return this.mspPast;
      }
      set
      {
        this.mspPast = value;
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

    public virtual double PaymentPast
    {
      get
      {
        return this.paymentPast;
      }
      set
      {
        this.paymentPast = value;
      }
    }

    public virtual double Subsidy
    {
      get
      {
        return this.subsidy;
      }
      set
      {
        this.subsidy = value;
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

    public virtual double RentComp
    {
      get
      {
        return this.rentComp;
      }
      set
      {
        this.rentComp = value;
      }
    }

    public virtual Period MonthDept
    {
      get
      {
        return this.monthDept;
      }
      set
      {
        this.monthDept = value;
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
      Balance balance = obj as Balance;
      return balance != null && (this.lsClient.ClientId == balance.LsClient.ClientId && this.period.PeriodId == balance.Period.PeriodId && (int) this.service.ServiceId == (int) balance.Service.ServiceId && this.supplier.SupplierId == balance.Supplier.SupplierId);
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
