// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CompanyParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CompanyParam
  {
    private Company company;
    private DateTime dBeg;
    private DateTime dEnd;
    private double paramValue;
    private Period period;
    private Param param;
    private string uName;
    private DateTime dEdit;

    [Browsable(false)]
    public virtual Company Company
    {
      get
      {
        return this.company;
      }
      set
      {
        this.company = value;
      }
    }

    [Browsable(false)]
    public virtual double ParamValue
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
    public virtual Param Param
    {
      get
      {
        return this.param;
      }
      set
      {
        this.param = value;
      }
    }

    [Browsable(false)]
    public virtual short ParamId
    {
      get
      {
        return this.param.ParamId;
      }
      set
      {
        this.param.ParamId = value;
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

    public virtual string UName
    {
      get
      {
        return this.uName;
      }
      set
      {
        this.uName = value;
      }
    }

    public virtual DateTime DEdit
    {
      get
      {
        return this.dEdit;
      }
      set
      {
        this.dEdit = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CompanyParam companyParam = obj as CompanyParam;
      return companyParam != null && ((int) this.company.CompanyId == (int) companyParam.Company.CompanyId && this.period.PeriodId == companyParam.Period.PeriodId && this.dBeg == companyParam.DBeg && (int) this.param.ParamId == (int) companyParam.Param.ParamId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.company.CompanyId;
      short num2 = this.company.CompanyId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      int hashCode2 = this.period.PeriodId.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int paramId = (int) this.param.ParamId;
      num2 = this.param.ParamId;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
