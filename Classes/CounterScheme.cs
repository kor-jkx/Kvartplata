// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CounterScheme
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CounterScheme
  {
    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual Counter Counter { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    [Browsable(false)]
    public virtual short Scheme { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual short SchemeODN { get; set; }

    [Browsable(false)]
    public virtual Home Home
    {
      get
      {
        if (this.Counter != null && this.Counter.Home != null)
          return this.Counter.Home;
        return (Home) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CounterScheme counterScheme = obj as CounterScheme;
      return counterScheme != null && (this.Period.PeriodId == counterScheme.Period.PeriodId && this.Counter.CounterId == counterScheme.Counter.CounterId && this.DBeg == counterScheme.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int periodId = this.Period.PeriodId;
      int num2 = this.Period.PeriodId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int counterId = this.Counter.CounterId;
      num2 = this.Counter.CounterId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dbeg = this.DBeg;
      int hashCode3 = this.DBeg.GetHashCode();
      return num4 + hashCode3;
    }
  }
}
