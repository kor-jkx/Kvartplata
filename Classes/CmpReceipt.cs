// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpReceipt
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  internal class CmpReceipt
  {
    private short receiptId;
    private short companyId;
    private int supplierId;
    private Bank bank;
    private string account;
    private string uName;
    private DateTime dEdit;
    private BaseOrg seller;
    private BaseOrg consignor;
    private int recipientId;

    public virtual short ReceiptId
    {
      get
      {
        return this.receiptId;
      }
      set
      {
        this.receiptId = value;
      }
    }

    public virtual short CompanyId
    {
      get
      {
        return this.companyId;
      }
      set
      {
        this.companyId = value;
      }
    }

    public virtual int SupplierId
    {
      get
      {
        return this.supplierId;
      }
      set
      {
        this.supplierId = value;
      }
    }

    public virtual BaseOrg Seller
    {
      get
      {
        return this.seller;
      }
      set
      {
        this.seller = value;
      }
    }

    public virtual BaseOrg Consignor
    {
      get
      {
        return this.consignor;
      }
      set
      {
        this.consignor = value;
      }
    }

    public virtual Bank Bank
    {
      get
      {
        return this.bank;
      }
      set
      {
        this.bank = value;
      }
    }

    public virtual string Account
    {
      get
      {
        return this.account;
      }
      set
      {
        this.account = value;
      }
    }

    public virtual string UName
    {
      get
      {
        return this.uName;
      }
      set
      {
        this.uName = value;
      }
    }

    public virtual DateTime DEdit
    {
      get
      {
        return this.dEdit;
      }
      set
      {
        this.dEdit = value;
      }
    }

    public virtual int RecipientId { get; set; }

    public virtual string PrintShow { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpReceipt cmpReceipt = obj as CmpReceipt;
      return cmpReceipt != null && ((int) this.receiptId == (int) cmpReceipt.receiptId && (int) this.companyId == (int) cmpReceipt.companyId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.companyId;
      int hashCode1 = this.companyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int receiptId = (int) this.receiptId;
      int hashCode2 = this.receiptId.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
