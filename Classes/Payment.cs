// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Payment
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Payment
  {
    private LsClient lsClient;
    private PurposePay pPay;
    private DateTime paymentDate;
    private Period period;
    private Period periodPay;
    private Receipt receipt;
    private SourcePay sPay;
    private Service service;
    private Supplier supplier;

    [Browsable(false)]
    public virtual int PaymentId { get; set; }

    public virtual string ClientId
    {
      get
      {
        if (this.lsClient != null)
          return this.lsClient.ClientId.ToString();
        return "ИТОГО";
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
    public virtual DateTime PeriodName
    {
      get
      {
        return this.period.PeriodName.Value;
      }
    }

    [Browsable(false)]
    public virtual Period PeriodPay
    {
      get
      {
        return this.periodPay;
      }
      set
      {
        this.periodPay = value;
      }
    }

    public virtual DateTime? PeriodPayValue
    {
      get
      {
        if (this.periodPay != null)
          return new DateTime?(this.periodPay.PeriodName.Value);
        return new DateTime?();
      }
    }

    public virtual DateTime? PeriodValue
    {
      get
      {
        if (this.period != null)
          return new DateTime?(this.period.PeriodName.Value);
        return new DateTime?();
      }
    }

    [Browsable(false)]
    public virtual SourcePay SPay
    {
      get
      {
        return this.sPay;
      }
      set
      {
        this.sPay = value;
      }
    }

    public virtual string SourceName
    {
      get
      {
        if (this.sPay != null)
          return this.sPay.SourcePayName;
        return "";
      }
    }

    [Browsable(false)]
    public virtual PurposePay PPay
    {
      get
      {
        return this.pPay;
      }
      set
      {
        this.pPay = value;
      }
    }

    public virtual string PurposeName
    {
      get
      {
        if (this.pPay != null)
          return this.pPay.PurposePayName;
        return "";
      }
    }

    public virtual DateTime PaymentDate
    {
      get
      {
        return this.paymentDate;
      }
      set
      {
        this.paymentDate = new DateTime();
        this.paymentDate = value;
      }
    }

    public virtual string PacketNum { get; set; }

    public virtual Decimal PaymentValue { get; set; }

    public virtual Decimal PaymentPeni { get; set; }

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

    public virtual string ServiceName
    {
      get
      {
        if (this.service != null)
          return this.service.ServiceName;
        return "";
      }
    }

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

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

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

    [Browsable(false)]
    public virtual PayDoc PayDoc { get; set; }

    public virtual string SupplierName
    {
      get
      {
        if (this.supplier != null && this.supplier.Recipient != null && this.supplier.Perfomer != null)
          return this.supplier.Recipient.NameOrgMin + " - " + this.supplier.Perfomer.NameOrgMin;
        return "";
      }
    }

    public virtual string ReceiptName
    {
      get
      {
        if (this.receipt != null)
          return this.receipt.ReceiptName;
        return "";
      }
    }

    public virtual BaseOrg RecipientId { get; set; }

    public virtual int? OhlaccountId { get; set; }
  }
}
