// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ClientParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class ClientParam
  {
    private int clientId;
    private DateTime dBeg;
    private DateTime dEnd;
    private double paramValue;
    private Period period;
    private Param param;
    private bool currentRecord;
    private string uname;
    private DateTime dedit;

    [Browsable(false)]
    public virtual string ParamName
    {
      get
      {
        return this.param.ParamName;
      }
      set
      {
        value = this.param.ParamName;
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
        value = this.param.ParamId;
      }
    }

    [Browsable(false)]
    public virtual DateTime PeriodName
    {
      get
      {
        return this.period.PeriodName.Value;
      }
      set
      {
        this.period.PeriodName = new DateTime?(value);
      }
    }

    [Browsable(false)]
    public virtual int ClientId
    {
      get
      {
        return this.clientId;
      }
      set
      {
        this.clientId = value;
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
    public virtual bool CurrentRecord
    {
      get
      {
        return this.currentRecord;
      }
      set
      {
        this.currentRecord = value;
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
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual bool IsInsert { get; set; }

    public ClientParam()
    {
    }

    public ClientParam(double value)
    {
      this.paramValue = value;
    }

    public override bool Equals(object obj)
    {
      if (this.period == null || this.param == null || obj == null)
        return false;
      ClientParam clientParam = obj as ClientParam;
      return clientParam != null && (this.clientId == clientParam.ClientId && this.period == clientParam.Period && this.dBeg == clientParam.DBeg && this.param == clientParam.Param);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      if (this.period != null && this.param != null)
      {
        int num2 = num1;
        int clientId = this.clientId;
        int hashCode1 = this.clientId.GetHashCode();
        int num3 = num2 + hashCode1 + (this.period == null ? 0 : this.period.GetHashCode());
        DateTime dBeg = this.dBeg;
        int hashCode2 = this.dBeg.GetHashCode();
        num1 = num3 + hashCode2 + (this.param == null ? 0 : this.param.GetHashCode());
      }
      return num1;
    }
  }
}
