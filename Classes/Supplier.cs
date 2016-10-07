// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Supplier
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Supplier
  {
    public virtual int SupplierId { get; set; }

    [Browsable(false)]
    public virtual BaseOrg Recipient { get; set; }

    [Browsable(false)]
    public virtual BaseOrg Perfomer { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    public virtual string RecName
    {
      get
      {
        if (this.Recipient != null && (uint) this.Recipient.BaseOrgId > 0U)
          return this.Recipient.NameOrgMin;
        return "";
      }
    }

    public virtual string PerName
    {
      get
      {
        if (this.Perfomer != null && (uint) this.Perfomer.BaseOrgId > 0U)
          return this.Perfomer.NameOrgMin;
        return "";
      }
    }

    public Supplier()
    {
    }

    public Supplier(int id)
    {
      this.SupplierId = id;
    }

    public Supplier(int id, BaseOrg rec, BaseOrg per)
    {
      this.SupplierId = id;
      this.Recipient = rec;
      this.Perfomer = per;
    }
  }
}
