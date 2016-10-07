// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CompanyPeriod
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CompanyPeriod
  {
    private Company company;
    private Complex complex;
    private Period period;
    private string uname;
    private DateTime dedit;

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

    public virtual Complex Complex
    {
      get
      {
        return this.complex;
      }
      set
      {
        this.complex = value;
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
      CompanyPeriod companyPeriod = obj as CompanyPeriod;
      return companyPeriod != null && ((int) this.company.CompanyId == (int) companyPeriod.Company.CompanyId && this.complex.ComplexId == companyPeriod.Complex.ComplexId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.company.CompanyId;
      int hashCode1 = this.company.CompanyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int complexId = this.complex.ComplexId;
      int hashCode2 = this.complex.ComplexId.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
