// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.HomesPhones
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class HomesPhones
  {
    public virtual string Phone { get; set; }

    public virtual string Note { get; set; }

    [Browsable(false)]
    public virtual Home Home { get; set; }

    [Browsable(false)]
    public virtual Di_PhonesServ PhonesServ { get; set; }

    [Browsable(false)]
    public virtual Company Company { get; set; }

    [Browsable(false)]
    public virtual int ClientId { get; set; }

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

    [Browsable(false)]
    public virtual Receipt Receipt { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      HomesPhones homesPhones = obj as HomesPhones;
      return homesPhones != null && ((int) this.Company.CompanyId == (int) homesPhones.Company.CompanyId && this.PhonesServ.Idservice == homesPhones.PhonesServ.Idservice && (this.Home.IdHome == homesPhones.Home.IdHome && this.ClientId == homesPhones.ClientId) && this.DBeg == homesPhones.DBeg && this.Receipt == homesPhones.Receipt);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.Company.CompanyId;
      int hashCode1 = this.Company.CompanyId.GetHashCode();
      int num2 = num1 + hashCode1;
      int idservice = this.PhonesServ.Idservice;
      int num3 = this.PhonesServ.Idservice;
      int hashCode2 = num3.GetHashCode();
      int num4 = num2 + hashCode2;
      int idHome = this.Home.IdHome;
      num3 = this.Home.IdHome;
      int hashCode3 = num3.GetHashCode();
      int num5 = num4 + hashCode3;
      int clientId = this.ClientId;
      num3 = this.ClientId;
      int hashCode4 = num3.GetHashCode();
      int num6 = num5 + hashCode4 + (this.Receipt == null ? 0 : this.Receipt.GetHashCode());
      DateTime dbeg = this.DBeg;
      int hashCode5 = this.DBeg.GetHashCode();
      return num6 + hashCode5;
    }
  }
}
