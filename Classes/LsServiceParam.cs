// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsServiceParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsServiceParam
  {
    private LsClient lsClient;
    private Service service;
    private DateTime dBeg;
    private DateTime dEnd;
    private double paramValue;
    private Period period;
    private Param param;
    private string uName;
    private DateTime dEdit;

    [Browsable(false)]
    public virtual LsClient LsClient
    {
      get
      {
        return this.lsClient;
      }
      set
      {
        this.lsClient = value;
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
    public virtual Service Service
    {
      get
      {
        return this.service;
      }
      set
      {
        this.service = value;
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
        this.dEdit = new DateTime();
        this.dEdit = value;
      }
    }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    public override bool Equals(object obj)
    {
      if (this.period == null || this.param == null || obj == null)
        return false;
      LsServiceParam lsServiceParam = obj as LsServiceParam;
      return lsServiceParam != null && (this.lsClient == lsServiceParam.LsClient && this.period == lsServiceParam.Period && (this.dBeg == lsServiceParam.DBeg && this.param == lsServiceParam.Param) && this.service == lsServiceParam.Service);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      if (this.period != null && this.param != null)
      {
        int num2 = num1 + (this.lsClient == null ? 0 : this.lsClient.GetHashCode()) + (this.period == null ? 0 : this.period.GetHashCode());
        DateTime dBeg = this.dBeg;
        int hashCode = this.dBeg.GetHashCode();
        num1 = num2 + hashCode + (this.param == null ? 0 : this.param.GetHashCode()) + (this.service == null ? 0 : this.service.GetHashCode());
      }
      return num1;
    }
  }
}
