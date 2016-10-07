// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.CrossService
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class CrossService
  {
    public virtual Company Company { get; set; }

    public virtual Service Service { get; set; }

    public virtual CrossType CrossType { get; set; }

    public virtual Service CrossServ { get; set; }

    public virtual DateTime DBeg { get; set; }

    public virtual DateTime DEnd { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    [Browsable(false)]
    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      CrossService crossService = obj as CrossService;
      return crossService != null && ((int) this.Company.CompanyId == (int) crossService.Company.CompanyId && (int) this.Service.ServiceId == (int) crossService.Service.ServiceId && ((int) this.CrossServ.ServiceId == (int) crossService.CrossServ.ServiceId && (int) this.CrossType.CrossTypeId == (int) crossService.CrossType.CrossTypeId) && this.DBeg == crossService.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = (int) this.Company.CompanyId;
      short num2 = this.Company.CompanyId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int serviceId1 = (int) this.Service.ServiceId;
      num2 = this.Service.ServiceId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      int serviceId2 = (int) this.CrossServ.ServiceId;
      num2 = this.CrossServ.ServiceId;
      int hashCode3 = num2.GetHashCode();
      int num5 = num4 + hashCode3;
      int crossTypeId = (int) this.CrossType.CrossTypeId;
      num2 = this.CrossType.CrossTypeId;
      int hashCode4 = num2.GetHashCode();
      int num6 = num5 + hashCode4;
      DateTime dbeg = this.DBeg;
      int hashCode5 = this.DBeg.GetHashCode();
      return num6 + hashCode5;
    }
  }
}
