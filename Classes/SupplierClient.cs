// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.SupplierClient
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class SupplierClient
  {
    [Browsable(false)]
    public virtual Company Company { get; set; }

    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual BaseOrg Supplier { get; set; }

    [Browsable(false)]
    public virtual double SupplierClientId { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsInsert { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      SupplierClient supplierClient = obj as SupplierClient;
      return supplierClient != null && ((int) this.Company.CompanyId == (int) supplierClient.Company.CompanyId && this.LsClient.ClientId == supplierClient.LsClient.ClientId && this.Supplier.BaseOrgId == supplierClient.Supplier.BaseOrgId && this.SupplierClientId == supplierClient.SupplierClientId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.Company.CompanyId;
      int hashCode1 = this.Company.CompanyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int clientId = this.LsClient.ClientId;
      int num3 = this.LsClient.ClientId;
      int hashCode2 = num3.GetHashCode();
      int num4 = num2 + hashCode2;
      int baseOrgId = this.Supplier.BaseOrgId;
      num3 = this.Supplier.BaseOrgId;
      int hashCode3 = num3.GetHashCode();
      int num5 = num4 + hashCode3;
      double supplierClientId = this.SupplierClientId;
      int hashCode4 = this.SupplierClientId.GetHashCode();
      return num5 + hashCode4;
    }
  }
}
