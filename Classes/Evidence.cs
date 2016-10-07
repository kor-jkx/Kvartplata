// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Evidence
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Evidence
  {
    private Counter counter;
    private Period period;
    private double past;
    private double current;
    private DateTime dBeg;
    private DateTime dEnd;
    private string uName;
    private DateTime dEdit;

    [Browsable(false)]
    public virtual Counter Counter
    {
      get
      {
        return this.counter;
      }
      set
      {
        this.counter = value;
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
    public virtual double Past
    {
      get
      {
        return this.past;
      }
      set
      {
        this.past = value;
      }
    }

    [Browsable(false)]
    public virtual double Current
    {
      get
      {
        return this.current;
      }
      set
      {
        this.current = value;
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

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public virtual string CounterNum
    {
      get
      {
        if (this.counter != null)
          return this.counter.CounterNum;
        return "0";
      }
    }

    public virtual Decimal Volume
    {
      get
      {
        double current = this.current;
        int num1 = 1;
        double past = this.past;
        int num2 = 1;
        if ((num1 & num2) == 0)
          return Decimal.Zero;
        if (this.current - this.past >= 0.0)
          return Convert.ToDecimal(this.counter.CoeffTrans) * (Convert.ToDecimal(this.current) - Convert.ToDecimal(this.past));
        if ((double) Convert.ToInt32(this.counter.CoeffTrans) * (this.past - this.current) <= 5000.0 && this.counter.BaseCounter.Id == 2 || (double) Convert.ToInt32(this.counter.CoeffTrans) * (this.past - this.current) <= 500000.0 && this.counter.BaseCounter.Id != 2)
          return Convert.ToDecimal(this.current) - Convert.ToDecimal(this.past);
        int num3 = 0;
        Decimal num4 = Convert.ToDecimal(this.past);
        while (num4 > Decimal.One)
        {
          ++num3;
          num4 /= new Decimal(10);
        }
        return (Decimal) Convert.ToInt32(this.counter.CoeffTrans) * (Convert.ToDecimal(Convert.ToDecimal(Math.Pow(10.0, (double) num3))) + Convert.ToDecimal(this.current) - Convert.ToDecimal(this.past));
      }
    }

    public virtual string ServiceName
    {
      get
      {
        if (this.counter != null && this.counter.Service != null)
          return this.counter.Service.ServiceName;
        return (string) null;
      }
    }

    [Browsable(false)]
    public virtual int? ClientId
    {
      get
      {
        if (this.counter != null && this.counter.LsClient != null)
          return new int?(this.counter.LsClient.ClientId);
        return new int?();
      }
    }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        if (this.counter != null && this.counter.Home != null)
          return this.counter.Home;
        return (Home) null;
      }
    }

    public Evidence()
    {
    }

    public Evidence(Counter counter, Period period, double current, double past, DateTime dBeg, DateTime dEnd)
    {
      this.counter = counter;
      this.period = period;
      this.current = current;
      this.past = past;
      this.dBeg = dBeg;
      this.dEnd = dEnd;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Evidence evidence = obj as Evidence;
      return evidence != null && (this.counter.CounterId == evidence.Counter.CounterId && this.period.PeriodId == evidence.Period.PeriodId && this.DBeg == evidence.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int counterId = this.counter.CounterId;
      int num2 = this.counter.CounterId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dbeg = this.DBeg;
      int hashCode3 = this.DBeg.GetHashCode();
      return num4 + hashCode3;
    }
  }
}
