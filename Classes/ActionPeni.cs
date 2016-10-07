// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ActionPeni
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  internal class ActionPeni
  {
    private Period period;
    private LsClient lsClient;
    private Service service;
    private Supplier supplier;
    private short code;
    private double correct;
    private string note;

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

    public virtual double Correct
    {
      get
      {
        return this.correct;
      }
      set
      {
        this.correct = value;
      }
    }

    public virtual string Note
    {
      get
      {
        return this.note;
      }
      set
      {
        this.note = value;
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

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      ActionPeni actionPeni = obj as ActionPeni;
      return actionPeni != null && (this.lsClient.ClientId == actionPeni.LsClient.ClientId && this.period.PeriodId == actionPeni.Period.PeriodId && ((int) this.service.ServiceId == (int) actionPeni.Service.ServiceId && this.supplier.SupplierId == actionPeni.Supplier.SupplierId) && (int) this.code == (int) actionPeni.Code);
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
      int hashCode3 = this.service.ServiceId.GetHashCode();
      int num5 = num4 + hashCode3;
      int supplierId = this.supplier.SupplierId;
      num2 = this.supplier.SupplierId;
      int hashCode4 = num2.GetHashCode();
      int num6 = num5 + hashCode4;
      int code = (int) this.code;
      int hashCode5 = this.code.GetHashCode();
      return num6 + hashCode5;
    }
  }
}
