// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.SocSaldo
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class SocSaldo
  {
    public virtual DateTime Period { get; set; }

    public virtual LsClient LsClient { get; set; }

    public virtual Person Person { get; set; }

    public virtual DcMSP MSP { get; set; }

    public virtual Service Service { get; set; }

    public virtual double Incoming { get; set; }

    public virtual double Past { get; set; }

    public virtual double Calc { get; set; }

    public virtual double Corr { get; set; }

    public virtual double Pay { get; set; }

    public virtual double Outcoming { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      SocSaldo socSaldo = obj as SocSaldo;
      return socSaldo != null && (this.Period == socSaldo.Period && this.LsClient.ClientId == socSaldo.LsClient.ClientId && (this.Person.PersonId == socSaldo.Person.PersonId && this.MSP.MSP_id == socSaldo.MSP.MSP_id) && (int) this.Service.ServiceId == (int) socSaldo.Service.ServiceId);
    }

    public override int GetHashCode()
    {
      int num = 13;
      DateTime period = this.Period;
      int hashCode = this.Period.GetHashCode();
      return num + hashCode + (this.LsClient == null ? 0 : this.LsClient.GetHashCode()) + (this.Person == null ? 0 : this.Person.GetHashCode()) + (this.MSP == null ? 0 : this.MSP.GetHashCode()) + (this.Service == null ? 0 : this.Service.GetHashCode());
    }
  }
}
