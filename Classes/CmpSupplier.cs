// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CmpSupplier
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class CmpSupplier
  {
    public virtual Supplier SupplierOrg { get; set; }

    public virtual Service Service { get; set; }

    public virtual Company Company { get; set; }

    public virtual int Priority { get; set; }

    public virtual int Vat { get; set; }

    public virtual Supplier SupplierPeni { get; set; }

    public virtual BaseOrg RecipientPeni { get; set; }

    public virtual BaseOrg PerfomerPeni { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public virtual BaseOrg RecipientId { get; set; }

    public virtual BaseOrg RecipientPeniId { get; set; }

    public CmpSupplier()
    {
    }

    public CmpSupplier(Supplier b)
    {
      this.SupplierOrg = b;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CmpSupplier cmpSupplier = obj as CmpSupplier;
      return cmpSupplier != null && (this.SupplierOrg.SupplierId == cmpSupplier.SupplierOrg.SupplierId && this.Service == cmpSupplier.Service && (int) this.Company.CompanyId == (int) cmpSupplier.Company.CompanyId);
    }

    public override int GetHashCode()
    {
      int num1 = 13 + (this.SupplierOrg == null ? 0 : this.SupplierOrg.GetHashCode());
      int serviceId = (int) this.Service.ServiceId;
      int hashCode1 = this.Service.ServiceId.GetHashCode();
      int num2 = num1 + hashCode1;
      int companyId = (int) this.Company.CompanyId;
      int hashCode2 = this.Company.CompanyId.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
