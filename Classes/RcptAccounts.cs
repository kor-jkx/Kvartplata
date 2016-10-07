// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.RcptAccounts
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class RcptAccounts
  {
    private Receipt _receipt;

    [Browsable(false)]
    public virtual Company Company { get; set; }

    [Browsable(false)]
    public virtual Receipt Receipt
    {
      get
      {
        return this._receipt;
      }
      set
      {
        this._receipt = value;
      }
    }

    public virtual int IdHome { get; set; }

    [Browsable(false)]
    public virtual Complex Complex { get; set; }

    public virtual short Priorites { get; set; }

    public virtual DateTime DBeg { get; set; }

    public virtual DateTime DEnd { get; set; }

    [Browsable(false)]
    public virtual ohlAccounts ohlAccounts { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public virtual string CompanyName
    {
      get
      {
        return this.Company.CompanyName;
      }
    }

    public virtual string Address { get; set; }

    public virtual short ReceiptId
    {
      get
      {
        return this.Receipt.ReceiptId;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      RcptAccounts rcptAccounts = obj as RcptAccounts;
      return rcptAccounts != null && ((int) this.Company.CompanyId == (int) rcptAccounts.Company.CompanyId && (int) this.Receipt.ReceiptId == (int) rcptAccounts.Receipt.ReceiptId && (this.IdHome == rcptAccounts.IdHome && this.Complex.ComplexId == rcptAccounts.Complex.ComplexId) && (int) this.Priorites == (int) rcptAccounts.Priorites && this.DBeg == rcptAccounts.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13 + (this.Company == null ? 0 : this.Company.CompanyId.GetHashCode()) + (this.Receipt == null ? 0 : this.Receipt.ReceiptId.GetHashCode());
      int idHome = this.IdHome;
      int hashCode1 = this.IdHome.GetHashCode();
      int num2 = num1 + hashCode1 + (this.Complex == null ? 0 : this.Complex.ComplexId.GetHashCode());
      int priorites = (int) this.Priorites;
      int hashCode2 = this.Priorites.GetHashCode();
      int num3 = num2 + hashCode2;
      DateTime dbeg = this.DBeg;
      int hashCode3 = this.DBeg.GetHashCode();
      return num3 + hashCode3;
    }

    public virtual RcptAccounts Clone()
    {
      return (RcptAccounts) this.MemberwiseClone();
    }
  }
}
