// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.HomeParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class HomeParam
  {
    private Company company;
    private Home home;
    private Period period;
    private DateTime dBeg;
    private DateTime dEnd;
    private Param param;
    private double paramValue;
    private string uname;
    private DateTime dedit;

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
    public virtual short ParamType
    {
      get
      {
        return this.Param.Param_type;
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
    public virtual Home Home
    {
      get
      {
        return this.home;
      }
      set
      {
        this.home = value;
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
      HomeParam homeParam = obj as HomeParam;
      return homeParam != null && ((int) this.company.CompanyId == (int) homeParam.Company.CompanyId && this.home.IdHome == homeParam.home.IdHome && (this.period.PeriodId == homeParam.Period.PeriodId && this.dBeg == homeParam.DBeg) && (int) this.param.ParamId == (int) homeParam.Param.ParamId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.company.CompanyId;
      short num2 = this.company.CompanyId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int idHome = this.home.IdHome;
      int num4 = this.home.IdHome;
      int hashCode2 = num4.GetHashCode();
      int num5 = num3 + hashCode2;
      int periodId = this.period.PeriodId;
      num4 = this.period.PeriodId;
      int hashCode3 = num4.GetHashCode();
      int num6 = num5 + hashCode3;
      DateTime dBeg = this.dBeg;
      int hashCode4 = this.dBeg.GetHashCode();
      int num7 = num6 + hashCode4;
      int paramId = (int) this.param.ParamId;
      num2 = this.param.ParamId;
      int hashCode5 = num2.GetHashCode();
      return num7 + hashCode5;
    }
  }
}
