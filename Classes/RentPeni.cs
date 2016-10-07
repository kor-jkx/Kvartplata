// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.RentPeni
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class RentPeni
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
    public virtual short Code { get; set; }

    [Browsable(false)]
    public virtual short Days { get; set; }

    [Browsable(false)]
    public virtual Payment Payment { get; set; }

    [Browsable(false)]
    public virtual double Rent { get; set; }

    [Browsable(false)]
    public virtual double Dept { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      RentPeni rentPeni = obj as RentPeni;
      return rentPeni != null && (this.LsClient.ClientId == rentPeni.LsClient.ClientId && this.Period.PeriodId == rentPeni.Period.PeriodId && ((int) this.Service.ServiceId == (int) rentPeni.Service.ServiceId && this.Supplier.SupplierId == rentPeni.Supplier.SupplierId) && (this.Month.PeriodId == rentPeni.Month.PeriodId && (int) this.Code == (int) rentPeni.Code && (int) this.Days == (int) rentPeni.Days) && this.Payment == rentPeni.Payment);
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
      int num8 = num7 + hashCode5;
      int code = (int) this.Code;
      num5 = this.Code;
      int hashCode6 = num5.GetHashCode();
      int num9 = num8 + hashCode6;
      int days = (int) this.Days;
      num5 = this.Days;
      int hashCode7 = num5.GetHashCode();
      return num9 + hashCode7 + (this.Payment == null ? 0 : this.Payment.GetHashCode());
    }
  }
}
