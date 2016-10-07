// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.lsWorkPayRent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class lsWorkPayRent
  {
    private hmWorkDistribute workDistribute;
    private Period period;
    private LsClient client;
    private Supplier supplier;
    private Decimal pay;
    private Decimal payDept;
    private Decimal payPercent;
    private Decimal payComission;
    private Decimal balanceIn;
    private Decimal balanceOut;
    private Decimal rent;
    private Decimal rentPercent;
    private Decimal rentDept;
    private Decimal rentComission;

    public virtual hmWorkDistribute WorkDistribute
    {
      get
      {
        return this.workDistribute;
      }
      set
      {
        this.workDistribute = value;
      }
    }

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

    public virtual Decimal Pay
    {
      get
      {
        return this.pay;
      }
      set
      {
        this.pay = value;
      }
    }

    public virtual Decimal PayDept
    {
      get
      {
        return this.payDept;
      }
      set
      {
        this.payDept = value;
      }
    }

    public virtual Decimal PayPercent
    {
      get
      {
        return this.payPercent;
      }
      set
      {
        this.payPercent = value;
      }
    }

    public virtual Decimal PayComission
    {
      get
      {
        return this.payComission;
      }
      set
      {
        this.payComission = value;
      }
    }

    public virtual Decimal BalanceIn
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

    public virtual Decimal BalanceOut
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

    public virtual Decimal RentPercent
    {
      get
      {
        return this.rentPercent;
      }
      set
      {
        this.rentPercent = value;
      }
    }

    public virtual Decimal RentDept
    {
      get
      {
        return this.rentDept;
      }
      set
      {
        this.rentDept = value;
      }
    }

    public virtual Decimal RentComission
    {
      get
      {
        return this.rentComission;
      }
      set
      {
        this.rentComission = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      lsWorkPayRent lsWorkPayRent = obj as lsWorkPayRent;
      return lsWorkPayRent != null && (this.period.PeriodId == lsWorkPayRent.Period.PeriodId && this.Client.ClientId == lsWorkPayRent.Client.ClientId && this.WorkDistribute.WorkDistribute == lsWorkPayRent.WorkDistribute.WorkDistribute);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int periodId = this.period.PeriodId;
      int num2 = this.Period.PeriodId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int clientId = this.Client.ClientId;
      num2 = this.Client.ClientId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int workDistribute = this.WorkDistribute.WorkDistribute;
      num2 = this.WorkDistribute.WorkDistribute;
      int hashCode3 = num2.GetHashCode();
      return num4 + hashCode3;
    }
  }
}
