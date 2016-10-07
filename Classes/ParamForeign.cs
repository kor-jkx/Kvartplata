// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ParamForeign
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class ParamForeign
  {
    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual short Code { get; set; }

    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    [Browsable(false)]
    public virtual Service Service { get; set; }

    public virtual DateTime DBeg { get; set; }

    public virtual DateTime DEnd { get; set; }

    public virtual string ServiceName
    {
      get
      {
        return this.Service.ServiceName;
      }
    }

    public virtual short P101 { get; set; }

    public virtual short P102 { get; set; }

    public virtual Decimal P2 { get; set; }

    [Browsable(false)]
    public virtual BaseTariff BaseTariff { get; set; }

    public virtual string BaseTariffName
    {
      get
      {
        return this.BaseTariff.BaseTariff_name;
      }
    }

    public virtual double Cost { get; set; }

    public virtual double NormValue { get; set; }

    public virtual double ParamValue { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      ParamForeign paramForeign = obj as ParamForeign;
      return paramForeign != null && (this.LsClient.ClientId == paramForeign.LsClient.ClientId && this.Period.PeriodId == paramForeign.Period.PeriodId && ((int) this.Service.ServiceId == (int) paramForeign.Service.ServiceId && (int) this.Code == (int) paramForeign.Code) && this.DBeg == paramForeign.DBeg);
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
      int serviceId = (int) this.Service.ServiceId;
      short num5 = this.Service.ServiceId;
      int hashCode3 = num5.GetHashCode();
      int num6 = num4 + hashCode3;
      int code = (int) this.Code;
      num5 = this.Code;
      int hashCode4 = num5.GetHashCode();
      int num7 = num6 + hashCode4;
      DateTime dbeg = this.DBeg;
      int hashCode5 = this.DBeg.GetHashCode();
      return num7 + hashCode5;
    }
  }
}
