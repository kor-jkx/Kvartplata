// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DcMSP
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class DcMSP
  {
    private int msp_id;
    private string msp_name;
    private int codeSoc;
    private int complex_id;
    private Period mspPeriod;
    private short priority;
    private short account;
    private string uname;
    private DateTime dedit;

    public virtual int MSP_id
    {
      get
      {
        return this.msp_id;
      }
      set
      {
        this.msp_id = value;
      }
    }

    public virtual string MSP_name
    {
      get
      {
        return this.msp_name;
      }
      set
      {
        this.msp_name = value;
      }
    }

    public virtual int CodeSoc
    {
      get
      {
        return this.codeSoc;
      }
      set
      {
        this.codeSoc = value;
      }
    }

    public virtual int Complex_id
    {
      get
      {
        return Options.Complex.ComplexId;
      }
      set
      {
        this.complex_id = value;
      }
    }

    public virtual short Priority
    {
      get
      {
        return this.priority;
      }
      set
      {
        this.priority = value;
      }
    }

    public virtual Period MSPPeriod
    {
      get
      {
        return this.mspPeriod;
      }
      set
      {
        this.mspPeriod = value;
      }
    }

    public virtual short Account
    {
      get
      {
        return this.account;
      }
      set
      {
        this.account = value;
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
  }
}
