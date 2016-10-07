// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Register
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Register
  {
    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual BaseOrg Manager { get; set; }

    [Browsable(false)]
    public virtual Contract Contract { get; set; }

    [Browsable(false)]
    public virtual Person Person { get; set; }

    [Browsable(false)]
    public virtual Guild Guild { get; set; }

    public virtual string TabNum { get; set; }

    [Browsable(false)]
    public virtual int PaymentId { get; set; }

    [Browsable(false)]
    public virtual double Rent { get; set; }

    [Browsable(false)]
    public virtual double RentPeni { get; set; }

    [Browsable(false)]
    public virtual double Deduction { get; set; }

    [Browsable(false)]
    public virtual double DeductionPeni { get; set; }

    [Browsable(false)]
    public virtual Receipt Receipt { get; set; }

    public virtual double Debt { get; set; }

    public virtual double DebtPeni { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }

    public virtual string OrgName
    {
      get
      {
        if (this.Contract != null && this.Contract.BaseOrg != null)
          return this.Contract.BaseOrg.NameOrgMin;
        return "";
      }
    }

    public virtual string ContractNum
    {
      get
      {
        if (this.Contract != null)
          return this.Contract.ContractNum;
        return "";
      }
    }

    public virtual string GuildName
    {
      get
      {
        if (this.Guild != null)
          return this.Guild.GuildName;
        return "";
      }
    }

    public virtual string LS
    {
      get
      {
        if (this.LsClient != null)
          return this.LsClient.ClientId.ToString();
        return "";
      }
    }

    public virtual string FIO
    {
      get
      {
        if (this.Person == null)
          return "";
        return this.Person.Family + " " + this.Person.Name + " " + this.Person.LastName;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Register register = obj as Register;
      return register != null && (this.LsClient.ClientId == register.LsClient.ClientId && this.Period.PeriodId == register.Period.PeriodId && this.Manager.BaseOrgId == register.Manager.BaseOrgId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int clientId = this.LsClient.ClientId;
      int num2 = this.LsClient.ClientId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.Period.PeriodId;
      num2 = this.Period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int baseOrgId = this.Manager.BaseOrgId;
      num2 = this.Manager.BaseOrgId;
      int hashCode3 = num2.GetHashCode();
      return num4 + hashCode3;
    }
  }
}
