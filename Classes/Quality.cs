// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Quality
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Quality
  {
    public virtual int Quality_id { get; set; }

    public virtual string Quality_name { get; set; }

    public virtual short Service_id { get; set; }

    public virtual short Company_id { get; set; }

    public virtual string DocNumber { get; set; }

    public virtual DateTime DocDate { get; set; }

    public virtual short? Legislation_id { get; set; }

    public virtual double Coeff { get; set; }

    public virtual short Quantity_hour { get; set; }

    public virtual short Quantity_degree { get; set; }

    public virtual Supplier Supplier { get; set; }

    public virtual BaseOrg Recipient { get; set; }

    public virtual BaseOrg Perfomer { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public Quality()
    {
    }

    public Quality(int quality_id, string name, short serv_id, short comp_id)
    {
      this.Quality_id = quality_id;
      this.Company_id = comp_id;
      this.Quality_name = name;
      this.Service_id = serv_id;
    }
  }
}
