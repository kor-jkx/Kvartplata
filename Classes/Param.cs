// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Param
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Param
  {
    private short paramId;
    private string paramName;
    private short param_type;
    private short sorter;
    private string uname;
    private DateTime dedit;
    private short areal;

    public virtual short ParamId
    {
      get
      {
        return this.paramId;
      }
      set
      {
        this.paramId = value;
      }
    }

    public virtual string ParamName
    {
      get
      {
        return this.paramName;
      }
      set
      {
        this.paramName = value;
      }
    }

    public virtual short Param_type
    {
      get
      {
        return this.param_type;
      }
      set
      {
        if ((int) this.param_type == (int) value)
          return;
        this.param_type = value;
      }
    }

    public virtual short Sorter
    {
      get
      {
        return this.sorter;
      }
      set
      {
        if ((int) this.sorter == (int) value)
          return;
        this.sorter = value;
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

    public virtual short Areal
    {
      get
      {
        return this.areal;
      }
      set
      {
        if ((int) this.areal == (int) value)
          return;
        this.areal = value;
      }
    }
  }
}
