// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpNorm
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CmpNorm
  {
    private int company_id;
    private Norm norm;
    private Period period;
    private DateTime dbeg;
    private DateTime? dend;
    private double norm_value;
    private double normMSP_value;
    private string uname;
    private DateTime dedit;
    private double norm_value_c;

    public virtual int Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        if (this.company_id == value)
          return;
        this.company_id = value;
      }
    }

    public virtual Norm Norm
    {
      get
      {
        return this.norm;
      }
      set
      {
        if (this.norm == value)
          return;
        this.norm = value;
      }
    }

    public virtual int NormId
    {
      get
      {
        if (this.norm != null)
          return this.norm.Norm_id;
        return 0;
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

    public virtual DateTime Dbeg
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

    public virtual double Norm_value
    {
      get
      {
        return this.norm_value;
      }
      set
      {
        if (this.norm_value == value)
          return;
        this.norm_value = value;
      }
    }

    public virtual double NormMSP_value
    {
      get
      {
        return this.normMSP_value;
      }
      set
      {
        if (this.normMSP_value == value)
          return;
        this.normMSP_value = value;
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

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public virtual string AllInfo { get; set; }

    public virtual double Norm_Value_C
    {
      get
      {
        return this.norm_value_c;
      }
      set
      {
        this.norm_value_c = value;
      }
    }

    public CmpNorm()
    {
    }

    public CmpNorm(Norm norm)
    {
      this.norm = norm;
    }

    public CmpNorm(Norm norm, string info)
    {
      this.norm = norm;
      this.AllInfo = info;
    }

    public CmpNorm(Norm norm, double value, string info)
    {
      this.norm = norm;
      this.AllInfo = value.ToString() + " " + info;
      this.norm_value = value;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpNorm cmpNorm = obj as CmpNorm;
      return cmpNorm != null && (this.norm.Norm_id == cmpNorm.Norm.Norm_id && this.period.PeriodId == cmpNorm.period.PeriodId && this.dbeg == cmpNorm.dbeg && this.company_id == cmpNorm.company_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = this.company_id;
      int hashCode1 = this.company_id.GetHashCode();
      int num2 = num1 + hashCode1 + (this.period == null ? 0 : this.period.GetHashCode());
      DateTime dbeg = this.dbeg;
      int hashCode2 = this.dbeg.GetHashCode();
      int num3 = num2 + hashCode2;
      int normId = this.norm.Norm_id;
      int hashCode3 = this.norm.Norm_id.GetHashCode();
      return num3 + hashCode3;
    }

    public virtual void Copy(CmpNorm obj)
    {
      this.Company_id = obj.Company_id;
      this.Dbeg = obj.Dbeg;
      this.Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
      this.Norm = obj.Norm;
      this.Norm_value = obj.Norm_value;
      this.NormMSP_value = obj.NormMSP_value;
      this.period = obj.period;
    }
  }
}
