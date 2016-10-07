// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Period
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Period
  {
    private int periodId;
    private DateTime? periodName;

    public virtual int PeriodId
    {
      get
      {
        return this.periodId;
      }
      set
      {
        this.periodId = value;
      }
    }

    public virtual DateTime? PeriodName
    {
      get
      {
        return this.periodName;
      }
      set
      {
        this.periodName = value;
      }
    }

    public Period()
    {
    }

    public Period(int id, DateTime? name)
    {
      this.periodId = id;
      this.periodName = name;
    }
  }
}
