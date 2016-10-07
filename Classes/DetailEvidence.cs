// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.DetailEvidence
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class DetailEvidence
  {
    [Browsable(false)]
    public virtual Counter Counter { get; set; }

    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual Period Month { get; set; }

    public virtual double Evidence { get; set; }

    [Browsable(false)]
    public virtual short Type { get; set; }

    [Browsable(false)]
    public virtual int? ClientId
    {
      get
      {
        if (this.Counter != null && this.Counter.LsClient != null)
          return new int?(this.Counter.LsClient.ClientId);
        return new int?();
      }
    }

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
      DetailEvidence detailEvidence = obj as DetailEvidence;
      return detailEvidence != null && (this.Counter.CounterId == detailEvidence.Counter.CounterId && this.Period.PeriodId == detailEvidence.Period.PeriodId && this.Month.PeriodId == detailEvidence.Month.PeriodId && (int) this.Type == (int) detailEvidence.Type);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int counterId = this.Counter.CounterId;
      int num2 = this.Counter.CounterId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId1 = this.Period.PeriodId;
      num2 = this.Period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int periodId2 = this.Month.PeriodId;
      num2 = this.Month.PeriodId;
      int hashCode3 = num2.GetHashCode();
      int num5 = num4 + hashCode3;
      int type = (int) this.Type;
      int hashCode4 = this.Type.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
