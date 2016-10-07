// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpHmReceipt
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CmpHmReceipt
  {
    [Browsable(false)]
    public virtual Company Company { get; set; }

    [Browsable(false)]
    public virtual Complex Complex { get; set; }

    [Browsable(false)]
    public virtual int HomeId { get; set; }

    [Browsable(false)]
    public virtual Service Service { get; set; }

    [Browsable(false)]
    public virtual Supplier Supplier { get; set; }

    [Browsable(false)]
    public virtual Receipt Receipt { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpHmReceipt cmpHmReceipt = obj as CmpHmReceipt;
      return cmpHmReceipt != null && ((int) this.Service.ServiceId == (int) cmpHmReceipt.Service.ServiceId && (int) this.Company.CompanyId == (int) cmpHmReceipt.Company.CompanyId && (this.Complex.IdFk == cmpHmReceipt.Complex.IdFk && this.HomeId == cmpHmReceipt.HomeId) && (this.Supplier.SupplierId == cmpHmReceipt.Supplier.SupplierId && (int) this.Receipt.ReceiptId == (int) cmpHmReceipt.Receipt.ReceiptId) && this.DBeg == cmpHmReceipt.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int serviceId = (int) this.Service.ServiceId;
      short num2 = this.Service.ServiceId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int companyId = (int) this.Company.CompanyId;
      num2 = this.Company.CompanyId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int idFk = this.Complex.IdFk;
      int num5 = this.Complex.IdFk;
      int hashCode3 = num5.GetHashCode();
      int num6 = num4 + hashCode3;
      int receiptId = (int) this.Receipt.ReceiptId;
      num2 = this.Receipt.ReceiptId;
      int hashCode4 = num2.GetHashCode();
      int num7 = num6 + hashCode4;
      int homeId = this.HomeId;
      num5 = this.HomeId;
      int hashCode5 = num5.GetHashCode();
      int num8 = num7 + hashCode5;
      int supplierId = this.Supplier.SupplierId;
      num5 = this.Supplier.SupplierId;
      int hashCode6 = num5.GetHashCode();
      int num9 = num8 + hashCode6;
      DateTime dbeg = this.DBeg;
      int hashCode7 = this.DBeg.GetHashCode();
      return num9 + hashCode7;
    }
  }
}
