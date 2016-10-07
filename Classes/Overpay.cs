// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Overpay
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Overpay
  {
    private LsClient lsClient;
    private Period period;
    private Service service;
    private Receipt receipt;
    private short code;
    private Payment payment;
    private Supplier supplier;
    private double pay;

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
    public virtual Receipt Receipt
    {
      get
      {
        return this.receipt;
      }
      set
      {
        this.receipt = value;
      }
    }

    [Browsable(false)]
    public virtual Payment Payment
    {
      get
      {
        return this.payment;
      }
      set
      {
        this.payment = value;
      }
    }

    [Browsable(false)]
    public virtual Supplier Supplier
    {
      get
      {
        return this.supplier;
      }
      set
      {
        this.supplier = value;
      }
    }

    public virtual double Pay
    {
      get
      {
        return this.pay;
      }
      set
      {
        this.pay = value;
      }
    }

    public virtual short Code
    {
      get
      {
        return this.code;
      }
      set
      {
        this.code = value;
      }
    }

    public virtual string ServiceName
    {
      get
      {
        if (this.service != null)
          return this.service.ServiceName;
        return (string) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Overpay overpay = obj as Overpay;
      return overpay != null && (this.lsClient.ClientId == overpay.LsClient.ClientId && this.period.PeriodId == overpay.Period.PeriodId && ((int) this.service.ServiceId == (int) overpay.Service.ServiceId && (int) this.receipt.ReceiptId == (int) overpay.Receipt.ReceiptId) && (overpay.payment.PaymentId == overpay.Payment.PaymentId && overpay.supplier.SupplierId == overpay.Supplier.SupplierId) && (int) overpay.code == (int) overpay.Code);
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
      int serviceId = (int) this.service.ServiceId;
      short num5 = this.service.ServiceId;
      int hashCode3 = num5.GetHashCode();
      int num6 = num4 + hashCode3;
      int receiptId = (int) this.receipt.ReceiptId;
      num5 = this.receipt.ReceiptId;
      int hashCode4 = num5.GetHashCode();
      int num7 = num6 + hashCode4;
      int paymentId = this.payment.PaymentId;
      num2 = this.payment.PaymentId;
      int hashCode5 = num2.GetHashCode();
      int num8 = num7 + hashCode5;
      int supplierId = this.supplier.SupplierId;
      num2 = this.supplier.SupplierId;
      int hashCode6 = num2.GetHashCode();
      int num9 = num8 + hashCode6;
      int code = (int) this.code;
      int hashCode7 = this.code.GetHashCode();
      return num9 + hashCode7;
    }
  }
}
