// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CorrectRent
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CorrectRent
  {
    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual Service Service { get; set; }

    [Browsable(false)]
    public virtual Supplier Supplier { get; set; }

    [Browsable(false)]
    public virtual Period Month { get; set; }

    [Browsable(false)]
    public virtual string Note { get; set; }

    [Browsable(false)]
    public virtual double Volume { get; set; }

    [Browsable(false)]
    public virtual double RentMain { get; set; }

    [Browsable(false)]
    public virtual double RentEO { get; set; }

    [Browsable(false)]
    public virtual string UName { get; set; }

    [Browsable(false)]
    public virtual DateTime? DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual double RentVat { get; set; }

    [Browsable(false)]
    public virtual short RentType { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CorrectRent correctRent = obj as CorrectRent;
      return correctRent != null && (this.LsClient.ClientId == correctRent.LsClient.ClientId && this.Period.PeriodId == correctRent.Period.PeriodId && ((int) this.Service.ServiceId == (int) correctRent.Service.ServiceId && this.Supplier.SupplierId == correctRent.Supplier.SupplierId) && (this.Month.PeriodId == correctRent.Month.PeriodId && this.Note == correctRent.Note) && (int) this.RentType == (int) correctRent.RentType);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.LsClient.ClientId;
      int num2 = this.LsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId1 = this.Period.PeriodId;
      num2 = this.Period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId = (int) this.Service.ServiceId;
      short num5 = this.Service.ServiceId;
      int hashCode3 = num5.GetHashCode();
      int num6 = num4 + hashCode3;
      int supplierId = this.Supplier.SupplierId;
      num2 = this.Supplier.SupplierId;
      int hashCode4 = num2.GetHashCode();
      int num7 = num6 + hashCode4;
      int periodId2 = this.Month.PeriodId;
      num2 = this.Month.PeriodId;
      int hashCode5 = num2.GetHashCode();
      int num8 = num7 + hashCode5 + (this.Note == null ? 0 : this.Note.GetHashCode());
      int rentType = (int) this.RentType;
      num5 = this.RentType;
      int hashCode6 = num5.GetHashCode();
      return num8 + hashCode6;
    }
  }
}
