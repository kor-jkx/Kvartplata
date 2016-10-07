// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpCoeffLocation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CmpCoeffLocation
  {
    private CounterLocation cntrLocation;
    private int companyId;
    private Period period;
    private DateTime dbeg;
    private DateTime? dend;
    private double coeff;
    private string uname;
    private DateTime dedit;

    public virtual int CompanyId
    {
      get
      {
        return this.companyId;
      }
      set
      {
        if (this.companyId == value)
          return;
        this.companyId = value;
      }
    }

    public virtual CounterLocation CntrLocation
    {
      get
      {
        return this.cntrLocation;
      }
      set
      {
        if (this.cntrLocation == value)
          return;
        this.cntrLocation = value;
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
        if (this.period == value)
          return;
        this.period = value;
      }
    }

    public virtual DateTime DBeg
    {
      get
      {
        return this.dbeg;
      }
      set
      {
        if (this.dbeg == value)
          return;
        this.dbeg = value;
      }
    }

    public virtual DateTime? Dend
    {
      get
      {
        return this.dend;
      }
      set
      {
        DateTime? dend = this.dend;
        DateTime? nullable = value;
        if (dend.HasValue == nullable.HasValue && (!dend.HasValue || dend.GetValueOrDefault() == nullable.GetValueOrDefault()))
          return;
        this.dend = value;
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
        if (this.coeff == value)
          return;
        this.coeff = value;
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
        this.dedit = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpCoeffLocation cmpCoeffLocation = obj as CmpCoeffLocation;
      return cmpCoeffLocation != null && (this.cntrLocation.CntrLocationId == cmpCoeffLocation.cntrLocation.CntrLocationId && this.period.PeriodId == cmpCoeffLocation.period.PeriodId && this.dbeg == cmpCoeffLocation.dbeg && this.CompanyId == cmpCoeffLocation.companyId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = this.companyId;
      int hashCode1 = this.companyId.GetHashCode();
      int num2 = num1 + hashCode1 + (this.period == null ? 0 : this.period.GetHashCode());
      DateTime dbeg = this.dbeg;
      int hashCode2 = this.dbeg.GetHashCode();
      return num2 + hashCode2 + (this.cntrLocation == null ? 0 : this.cntrLocation.GetHashCode());
    }

    public virtual void Copy(CmpCoeffLocation obj)
    {
      this.CompanyId = obj.CompanyId;
      this.DBeg = obj.DBeg;
      this.Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
      this.CntrLocation = obj.CntrLocation;
      this.Coeff = obj.Coeff;
      this.period = obj.period;
    }
  }
}
