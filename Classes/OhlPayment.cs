// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.OhlPayment
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class OhlPayment
  {
    private double _all;

    public int SupplierId { get; set; }

    public string Account { get; set; }

    public int TypeaccountId { get; set; }

    public double Rent { get; set; }

    public double RentPeni { get; set; }

    public double RentAll { get; set; }

    public int CompanyId { get; set; }

    public double All
    {
      get
      {
        this._all = this.Rent + this.RentPeni;
        return this._all;
      }
      set
      {
        this._all = value;
      }
    }

    public int OhlAccountId { get; set; }

    public int PaymentId { get; set; }

    public string BankName { get; set; }

    public DateTime PeriodPay { get; set; }

    public DateTime Period { get; set; }

    public DateTime PaymentDate { get; set; }

    public bool NotEqualsSum { get; set; }

    public double RentForClients { get; set; }

    public double RentPeniForClients { get; set; }

    public double RentForClientsRev { get; set; }

    public double RentPeniForClientsRev { get; set; }
  }
}
