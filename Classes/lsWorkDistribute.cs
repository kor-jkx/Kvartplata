// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.lsWorkDistribute
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class lsWorkDistribute
  {
    private hmWorkDistribute workDistribute;
    private Period period;
    private LsClient client;
    private Supplier supplier;
    private Decimal rent;
    private Decimal paramValue;
    private int scheme;
    private int monthCnt;
    private Decimal rate;
    private Decimal comission;
    private RightDoc rightDocs;
    private Decimal _rentCorrect;
    private Decimal _rentPercentCorrect;

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

    public virtual Decimal ParamValue
    {
      get
      {
        return this.paramValue;
      }
      set
      {
        this.paramValue = value;
      }
    }

    public virtual int Scheme
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

    public virtual int MonthCnt
    {
      get
      {
        return this.monthCnt;
      }
      set
      {
        this.monthCnt = value;
      }
    }

    public virtual Decimal Rate
    {
      get
      {
        return this.rate;
      }
      set
      {
        this.rate = value;
      }
    }

    public virtual Decimal Comission
    {
      get
      {
        return this.comission;
      }
      set
      {
        this.comission = value;
      }
    }

    public virtual string Uname { get; set; }

    public virtual DateTime? Dedit { get; set; }

    public virtual RightDoc RightDocs
    {
      get
      {
        return this.rightDocs;
      }
      set
      {
        this.rightDocs = value;
      }
    }

    public virtual Decimal RentCorrect
    {
      get
      {
        return this._rentCorrect;
      }
      set
      {
        this._rentCorrect = value;
      }
    }

    public virtual Decimal RentPercentCorrect
    {
      get
      {
        return this._rentPercentCorrect;
      }
      set
      {
        this._rentPercentCorrect = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      lsWorkDistribute lsWorkDistribute = obj as lsWorkDistribute;
      return lsWorkDistribute != null && (this.period.PeriodId == lsWorkDistribute.Period.PeriodId && this.Client.ClientId == lsWorkDistribute.Client.ClientId && this.WorkDistribute.WorkDistribute == lsWorkDistribute.WorkDistribute.WorkDistribute);
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
