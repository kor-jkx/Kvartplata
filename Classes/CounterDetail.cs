// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CounterDetail
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CounterDetail
  {
    private Period period;
    private Counter counter;
    private Service service;
    private Period month;
    private double evidence;
    private double evidenceCross;
    private double evidenceNorm;
    private double evidenceCntr;
    private double evidenceNormAr;
    private double evidenceCntrAr;
    private double coeff;
    private double normCount;
    private double normUnit;
    private double evidenceOdnNorm;
    private double evidenceOdnNorm110;
    private double evidenceOdnCntr;
    private double evidenceOdnCntr110;

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
    public virtual Service Service
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

    [Browsable(false)]
    public virtual Period Month
    {
      get
      {
        return this.month;
      }
      set
      {
        this.month = value;
      }
    }

    public virtual double Evidence
    {
      get
      {
        return this.evidence;
      }
      set
      {
        this.evidence = value;
      }
    }

    public virtual double EvidenceCross
    {
      get
      {
        return this.evidenceCross;
      }
      set
      {
        this.evidenceCross = value;
      }
    }

    public virtual double EvidenceNorm
    {
      get
      {
        return this.evidenceNorm;
      }
      set
      {
        this.evidenceNorm = value;
      }
    }

    public virtual double EvidenceCntr
    {
      get
      {
        return this.evidenceCntr;
      }
      set
      {
        this.evidenceCntr = value;
      }
    }

    public virtual double EvidenceNormAr
    {
      get
      {
        return this.evidenceNormAr;
      }
      set
      {
        this.evidenceNormAr = value;
      }
    }

    public virtual double EvidenceCntrAr
    {
      get
      {
        return this.evidenceCntrAr;
      }
      set
      {
        this.evidenceCntrAr = value;
      }
    }

    public virtual double Coeff
    {
      get
      {
        return this.coeff;
      }
      set
      {
        this.coeff = value;
      }
    }

    public virtual double NormCount
    {
      get
      {
        return this.normCount;
      }
      set
      {
        this.normCount = value;
      }
    }

    public virtual double NormUnit
    {
      get
      {
        return this.normUnit;
      }
      set
      {
        this.normUnit = value;
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

    public virtual double EvidenceOdnNorm
    {
      get
      {
        return this.evidenceOdnNorm;
      }
      set
      {
        this.evidenceOdnNorm = value;
      }
    }

    public virtual double EvidenceOdnNorm110
    {
      get
      {
        return this.evidenceOdnNorm110;
      }
      set
      {
        this.evidenceOdnNorm110 = value;
      }
    }

    public virtual double EvidenceOdnCntr
    {
      get
      {
        return this.evidenceOdnCntr;
      }
      set
      {
        this.evidenceOdnCntr = value;
      }
    }

    public virtual double EvidenceOdnCntr110
    {
      get
      {
        return this.evidenceOdnCntr110;
      }
      set
      {
        this.evidenceOdnCntr110 = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CounterDetail counterDetail = obj as CounterDetail;
      return counterDetail != null && (this.period.PeriodId == counterDetail.Period.PeriodId && this.counter.CounterId == counterDetail.Counter.CounterId && (int) this.service.ServiceId == (int) counterDetail.Service.ServiceId && this.month.PeriodId == counterDetail.Month.PeriodId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int periodId1 = this.period.PeriodId;
      int num2 = this.Period.PeriodId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int counterId = this.counter.CounterId;
      num2 = this.counter.CounterId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId = (int) this.service.ServiceId;
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int periodId2 = this.month.PeriodId;
      num2 = this.month.PeriodId;
      int hashCode4 = num2.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
