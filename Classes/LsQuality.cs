// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsQuality
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsQuality
  {
    private LsClient lsClient;
    private Period period;
    private Quality quality;
    private DateTime dBeg;
    private DateTime? dEnd;
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
    public virtual Quality Quality
    {
      get
      {
        return this.quality;
      }
      set
      {
        this.quality = value;
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
    public virtual DateTime? DEnd
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

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      LsQuality lsQuality = obj as LsQuality;
      return lsQuality != null && (this.lsClient.ClientId == lsQuality.LsClient.ClientId && this.period.PeriodId == lsQuality.Period.PeriodId && this.dBeg == lsQuality.DBeg && this.quality.Quality_id == lsQuality.Quality.Quality_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.lsClient.ClientId;
      int num2 = this.lsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.period.PeriodId;
      num2 = this.period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dBeg = this.dBeg;
      int hashCode3 = this.dBeg.GetHashCode();
      int num5 = num4 + hashCode3;
      int qualityId = this.quality.Quality_id;
      num2 = this.quality.Quality_id;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
