// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ServiceParam
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class ServiceParam
  {
    public virtual short Company_id { get; set; }

    public virtual Complex Complex { get; set; }

    public virtual short Service_id { get; set; }

    public virtual string PrintShow { get; set; }

    public virtual short Sorter { get; set; }

    public virtual short Receipt_id { get; set; }

    public virtual short Group_num { get; set; }

    public virtual int CodeSoc_id { get; set; }

    public virtual short AcceptPeni { get; set; }

    public virtual short SpecialId { get; set; }

    public virtual short BalanceIn { get; set; }

    public virtual Service SubsidIn { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public virtual YesNo DistrService { get; set; }

    public virtual YesNo DublService { get; set; }

    public virtual YesNo SaveOverpay { get; set; }

    public virtual YesNo BoilService { get; set; }

    public virtual YesNo SendRent { get; set; }

    public virtual YesNo ShowService { get; set; }

    public virtual YesNo ShowServiceInfo { get; set; }

    public ServiceParam()
    {
    }

    public ServiceParam(short company_id, short service_id, Complex complex)
    {
      this.Service_id = service_id;
      this.Company_id = company_id;
      this.Complex = complex;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      ServiceParam serviceParam = obj as ServiceParam;
      return serviceParam != null && ((int) this.Company_id == (int) serviceParam.Company_id && (int) this.Service_id == (int) serviceParam.Service_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.Company_id;
      short num2 = this.Company_id;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int serviceId = (int) this.Service_id;
      num2 = this.Service_id;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
