// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.LsArenda
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class LsArenda
  {
    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual BaseOrg BaseOrg { get; set; }

    public virtual string DogovorNum { get; set; }

    [Browsable(false)]
    public virtual DateTime DogovorDate { get; set; }

    [Browsable(false)]
    public virtual TypeArendaDocument TypeArendaDocument { get; set; }

    public virtual string KumiNum { get; set; }

    [Browsable(false)]
    public virtual DateTime? KumiDate { get; set; }

    [Browsable(false)]
    public virtual string UName { get; set; }

    [Browsable(false)]
    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    [Browsable(false)]
    public virtual string RentPrior { get; set; }

    public virtual string NameOrg
    {
      get
      {
        if (this.BaseOrg != null)
          return this.BaseOrg.NameOrgMin;
        return "";
      }
    }

    public virtual string NumLs
    {
      get
      {
        if (this.LsClient != null)
          return this.LsClient.ClientId.ToString();
        return "";
      }
    }

    public virtual int Status { get; set; }

    public virtual double Rent { get; set; }

    public virtual double RentPeni { get; set; }

    public virtual double Balance { get; set; }

    public virtual double Peni { get; set; }

    public virtual int? Months { get; set; }

    public LsArenda()
    {
    }

    public LsArenda(LsClient client, string num)
    {
      this.Status = client == null ? 0 : client.ClientId;
      this.LsClient = client;
      this.DogovorNum = num;
    }

    public LsArenda(LsClient client, string num, double balance, double peni, int? months)
    {
      this.LsClient = client;
      this.DogovorNum = num;
      this.Balance = balance;
      this.Peni = peni;
      this.Months = months;
    }

    public LsArenda(LsClient client, string num, double rent, double rentpeni, double balance, double peni, int? months)
    {
      this.LsClient = client;
      this.DogovorNum = num;
      this.Balance = balance;
      this.Peni = peni;
      this.Months = months;
      this.Rent = rent;
      this.RentPeni = rentpeni;
    }

    public LsArenda(LsClient client, string num, BaseOrg org, DateTime data)
    {
      this.LsClient = client;
      this.DogovorNum = num;
      this.BaseOrg = org;
      this.DogovorDate = data;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      LsArenda lsArenda = obj as LsArenda;
      return lsArenda != null && this.LsClient.ClientId == lsArenda.LsClient.ClientId;
    }

    public override int GetHashCode()
    {
      int num = 13;
      int clientId = this.LsClient.ClientId;
      int hashCode = this.LsClient.ClientId.GetHashCode();
      return num + hashCode;
    }
  }
}
