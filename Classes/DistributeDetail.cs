// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DistributeDetail
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class DistributeDetail
  {
    private Distribute distribute;
    private Period period;
    private Decimal rent;
    private Decimal rentDstr;
    private double paramDstr;
    private double coeff;

    [Browsable(false)]
    public virtual Distribute Distribute
    {
      get
      {
        return this.distribute;
      }
      set
      {
        this.distribute = value;
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

    public virtual Decimal RentDstr
    {
      get
      {
        return this.rentDstr;
      }
      set
      {
        this.rentDstr = value;
      }
    }

    public virtual double ParamDstr
    {
      get
      {
        return this.paramDstr;
      }
      set
      {
        this.paramDstr = value;
      }
    }

    public virtual double Coeff
    {
      get
      {
        return this.coeff;
      }
      set
      {
        this.coeff = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      DistributeDetail distributeDetail = obj as DistributeDetail;
      return distributeDetail != null && (this.period.PeriodId == distributeDetail.Period.PeriodId && this.distribute.DistributeId == distributeDetail.Distribute.DistributeId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int periodId = this.period.PeriodId;
      int num2 = this.Period.PeriodId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int distributeId = this.distribute.DistributeId;
      num2 = this.Distribute.DistributeId;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
