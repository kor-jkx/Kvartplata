// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.AbsenceCoeff
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  internal class AbsenceCoeff
  {
    public virtual short Absence_id { get; set; }

    public virtual short Service_id { get; set; }

    public virtual double Coeff { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      AbsenceCoeff absenceCoeff = obj as AbsenceCoeff;
      return absenceCoeff != null && ((int) this.Absence_id == (int) absenceCoeff.Absence_id && (int) this.Service_id == (int) absenceCoeff.Service_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int absenceId = (int) this.Absence_id;
      short num2 = this.Absence_id;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int serviceId = (int) this.Service_id;
      num2 = this.Service_id;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
