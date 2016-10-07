// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ReceiptParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class ReceiptParam
  {
    private Company company;
    private Receipt receipt;
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
    public virtual Receipt Receipt
    {
      get
      {
        return this.receipt;
      }
      set
      {
        this.receipt = value;
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
      ReceiptParam receiptParam = obj as ReceiptParam;
      return receiptParam != null && ((int) this.company.CompanyId == (int) receiptParam.Company.CompanyId && (int) this.receipt.ReceiptId == (int) receiptParam.receipt.ReceiptId && (this.period.PeriodId == receiptParam.Period.PeriodId && this.dBeg == receiptParam.DBeg) && (int) this.param.ParamId == (int) receiptParam.Param.ParamId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      try
      {
        int num2 = num1;
        int companyId = (int) this.company.CompanyId;
        int hashCode1 = this.company.CompanyId.GetHashCode();
        num1 = num2 + hashCode1;
        int num3 = num1;
        int receiptId = (int) this.receipt.ReceiptId;
        int hashCode2 = this.receipt.ReceiptId.GetHashCode();
        num1 = num3 + hashCode2;
        int num4 = num1;
        int periodId = this.period.PeriodId;
        int hashCode3 = this.period.PeriodId.GetHashCode();
        num1 = num4 + hashCode3;
        int num5 = num1;
        DateTime dBeg = this.dBeg;
        int hashCode4 = this.dBeg.GetHashCode();
        num1 = num5 + hashCode4;
        int num6 = num1;
        int paramId = (int) this.param.ParamId;
        int hashCode5 = this.param.ParamId.GetHashCode();
        num1 = num6 + hashCode5;
      }
      catch
      {
      }
      return num1;
    }
  }
}
