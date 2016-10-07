// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.BaseOrg
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class BaseOrg
  {
    public virtual int BaseOrgId { get; set; }

    public virtual string BaseOrgName { get; set; }

    public virtual string NameOrgMin { get; set; }

    public virtual string INN { get; set; }

    public virtual string OGRN { get; set; }

    public virtual string KPP { get; set; }

    public virtual string RSch { get; set; }

    [Browsable(false)]
    public virtual Bank Bank { get; set; }

    public virtual string BossFam { get; set; }

    public virtual string BossName { get; set; }

    public virtual string BossLastName { get; set; }

    public virtual string TypeWork { get; set; }

    public virtual string OKPO { get; set; }

    public virtual string OKONH { get; set; }

    public virtual string UInd { get; set; }

    public virtual string UPost { get; set; }

    public virtual Reg UCity { get; set; }

    public virtual int UPos { get; set; }

    public virtual int UStreet { get; set; }

    public virtual string UHome { get; set; }

    public virtual string UKorp { get; set; }

    public virtual string UFlat { get; set; }

    public virtual string UDop { get; set; }

    public virtual string Phone { get; set; }

    public virtual string Fax { get; set; }

    public virtual Reg PCity { get; set; }

    public virtual int PPos { get; set; }

    public virtual int PStreet { get; set; }

    public virtual string PHome { get; set; }

    public virtual string PKorp { get; set; }

    public virtual string PFlat { get; set; }

    public virtual string PDop { get; set; }

    public virtual string PInd { get; set; }

    public virtual string PPost { get; set; }

    public virtual string AdditionalCode { get; set; }

    public virtual int AdressPriority { get; set; }

    public virtual string Kbk { get; set; }

    public virtual string NameOrgMinDop
    {
      get
      {
        if ((uint) this.BaseOrgId > 0U)
          return this.NameOrgMin;
        return "";
      }
      set
      {
      }
    }

    [Browsable(false)]
    public virtual int Postaver { get; set; }

    public BaseOrg()
    {
    }

    public BaseOrg(int id, string minname)
    {
      this.BaseOrgId = id;
      this.NameOrgMin = minname;
    }

    public BaseOrg(int id, string name, string minname, Bank bank, string inn, string kpp, int postaver)
    {
      this.BaseOrgId = id;
      this.BaseOrgName = name;
      this.NameOrgMin = minname;
      this.Bank = bank;
      this.INN = inn;
      this.KPP = kpp;
      this.Postaver = postaver;
    }
  }
}
