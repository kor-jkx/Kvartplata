// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.SdcMSPPercent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class SdcMSPPercent
  {
    private int msp_id;
    private Period period;
    private DateTime dbeg;
    private DateTime? dend;
    private short service;
    private int spreading_id;
    private short? scheme;
    private int percent;
    private short share_id;
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

    public virtual DateTime Dbeg
    {
      get
      {
        return this.dbeg;
      }
      set
      {
        this.dbeg = value;
      }
    }

    public virtual DateTime? Dend
    {
      get
      {
        return this.dend;
      }
      set
      {
        this.dend = value;
      }
    }

    public virtual short Service
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

    public virtual int Spreading_id
    {
      get
      {
        return this.spreading_id;
      }
      set
      {
        this.spreading_id = value;
      }
    }

    public virtual short? Scheme
    {
      get
      {
        return this.scheme;
      }
      set
      {
        this.scheme = value;
      }
    }

    public virtual int Percent
    {
      get
      {
        return this.percent;
      }
      set
      {
        this.percent = value;
      }
    }

    public virtual short Share_id
    {
      get
      {
        return this.share_id;
      }
      set
      {
        if ((int) this.share_id == (int) value)
          return;
        this.share_id = value;
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

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      SdcMSPPercent sdcMspPercent = obj as SdcMSPPercent;
      return sdcMspPercent != null && (this.MSP_id == sdcMspPercent.MSP_id && this.Period.PeriodId == sdcMspPercent.Period.PeriodId && this.Dbeg == sdcMspPercent.Dbeg && (int) this.Service == (int) sdcMspPercent.Service);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int mspId = this.MSP_id;
      int hashCode1 = this.MSP_id.GetHashCode();
      int num2 = num1 + hashCode1 + (this.Period == null ? 0 : this.Period.GetHashCode());
      DateTime dbeg = this.Dbeg;
      int hashCode2 = this.Dbeg.GetHashCode();
      int num3 = num2 + hashCode2;
      int service = (int) this.Service;
      int hashCode3 = this.Service.GetHashCode();
      return num3 + hashCode3;
    }

    public virtual void Copy(SdcMSPPercent obj)
    {
      this.Dbeg = obj.Dbeg;
      this.Dend = new DateTime?(Convert.ToDateTime("31.12.2999"));
      this.msp_id = obj.msp_id;
      this.percent = obj.percent;
      this.period = obj.period;
      this.service = obj.service;
      this.share_id = obj.share_id;
      this.spreading_id = obj.spreading_id;
      this.scheme = obj.scheme;
    }
  }
}
