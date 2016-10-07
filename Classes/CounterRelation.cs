// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CounterRelation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CounterRelation
  {
    private Period period;
    private LsClient lsClient;
    private Counter counter;
    private DateTime dBeg;
    private DateTime dEnd;
    private string uName;
    private DateTime dEdit;
    private YesNo onOff;

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
    public virtual YesNo OnOff
    {
      get
      {
        return this.onOff;
      }
      set
      {
        this.onOff = value;
      }
    }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CounterRelation counterRelation = obj as CounterRelation;
      return counterRelation != null && (this.period.PeriodId == counterRelation.Period.PeriodId && this.lsClient.ClientId == counterRelation.LsClient.ClientId && this.dBeg == counterRelation.DBeg && this.counter.CounterId == counterRelation.Counter.CounterId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int periodId = this.period.PeriodId;
      int num2 = this.Period.PeriodId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int clientId = this.lsClient.ClientId;
      num2 = this.lsClient.ClientId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int counterId = this.counter.CounterId;
      num2 = this.counter.CounterId;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
