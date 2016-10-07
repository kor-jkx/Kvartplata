// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.UnitMeasuring
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class UnitMeasuring
  {
    private short unitMeasuringId;
    private string unitMeasuringName;
    private string uname;
    private DateTime dedit;

    public virtual short UnitMeasuringId
    {
      get
      {
        return this.unitMeasuringId;
      }
      set
      {
        this.unitMeasuringId = value;
      }
    }

    public virtual string UnitMeasuringName
    {
      get
      {
        return this.unitMeasuringName;
      }
      set
      {
        this.unitMeasuringName = value;
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
        this.dedit = new DateTime();
        this.dedit = value;
      }
    }
  }
}
