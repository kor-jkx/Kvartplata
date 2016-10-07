// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CmpParam
  {
    private int company_id;
    private int param_id;
    private Period period;
    private DateTime dbeg;
    private DateTime dend;
    private double? param_value;
    private string uname;
    private DateTime dedit;

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

    public virtual int Param_id
    {
      get
      {
        return this.param_id;
      }
      set
      {
        if (this.param_id == value)
          return;
        this.param_id = value;
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

    public virtual DateTime Dend
    {
      get
      {
        return this.dend;
      }
      set
      {
        if (this.dend == value)
          return;
        this.dend = value;
      }
    }

    public virtual double? Param_value
    {
      get
      {
        return this.param_value;
      }
      set
      {
        double? paramValue = this.param_value;
        double? nullable = value;
        if (paramValue.GetValueOrDefault() == nullable.GetValueOrDefault() && paramValue.HasValue == nullable.HasValue)
          return;
        this.param_value = value;
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

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpParam cmpParam = obj as CmpParam;
      return cmpParam != null && (this.param_id == cmpParam.param_id && this.period.PeriodId == cmpParam.period.PeriodId && this.dbeg == cmpParam.dbeg && this.company_id == cmpParam.company_id);
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
      int paramId = this.param_id;
      int hashCode3 = this.param_id.GetHashCode();
      return num3 + hashCode3;
    }

    public virtual void Copy(object obj)
    {
      this.Company_id = ((CmpParam) obj).Company_id;
      this.Dbeg = ((CmpParam) obj).Dbeg;
      this.Dend = Convert.ToDateTime("31.12.2999");
      this.param_id = ((CmpParam) obj).param_id;
      this.param_value = ((CmpParam) obj).param_value;
      this.period = ((CmpParam) obj).period;
    }
  }
}
