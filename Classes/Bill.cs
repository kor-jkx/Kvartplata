// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Bill
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Bill
  {
    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual Period Month { get; set; }

    [Browsable(false)]
    public virtual short BillType { get; set; }

    public virtual int BillNum { get; set; }

    public virtual DateTime BillDate { get; set; }

    [Browsable(false)]
    public virtual string UName { get; set; }

    [Browsable(false)]
    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual Receipt Receipt { get; set; }

    public virtual string ReceiptName
    {
      get
      {
        return this.Receipt.ReceiptName;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Bill bill = obj as Bill;
      return bill != null && (this.LsClient.ClientId == bill.LsClient.ClientId && this.Period.PeriodId == bill.Period.PeriodId && (this.Month.PeriodId == bill.Month.PeriodId && (int) this.BillType == (int) bill.BillType) && (int) this.Receipt.ReceiptId == (int) bill.Receipt.ReceiptId);
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
      int periodId2 = this.Month.PeriodId;
      num2 = this.Month.PeriodId;
      int hashCode3 = num2.GetHashCode();
      int num5 = num4 + hashCode3;
      int billType = (int) this.BillType;
      int hashCode4 = this.BillType.GetHashCode();
      return num5 + hashCode4 + (this.Receipt == null ? 0 : this.Receipt.ReceiptId.GetHashCode());
    }
  }
}
