// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CounterLocation
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CounterLocation
  {
    private int cntrLocationId;
    private string cntrLocationName;
    private string uname;
    private DateTime dedit;

    public virtual int CntrLocationId
    {
      get
      {
        return this.cntrLocationId;
      }
      set
      {
        if (this.cntrLocationId == value)
          return;
        this.cntrLocationId = value;
      }
    }

    public virtual string CntrLocationName
    {
      get
      {
        return this.cntrLocationName;
      }
      set
      {
        if (this.cntrLocationName == value)
          return;
        this.cntrLocationName = value;
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

    public CounterLocation()
    {
    }

    public CounterLocation(int cntrLocationId, string cntrLocationName)
    {
      this.cntrLocationId = cntrLocationId;
      this.cntrLocationName = cntrLocationName;
    }
  }
}
