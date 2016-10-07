// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Instalment
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Instalment
  {
    [Browsable(false)]
    public virtual Agreement Agreement { get; set; }

    [Browsable(false)]
    public virtual Period Period { get; set; }

    [Browsable(false)]
    public virtual Decimal Debt { get; set; }

    [Browsable(false)]
    public virtual Decimal DebtPeni { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Instalment instalment = obj as Instalment;
      return instalment != null && (this.Agreement.AgreementId == instalment.Agreement.AgreementId && this.Period.PeriodId == instalment.Period.PeriodId);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int agreementId = this.Agreement.AgreementId;
      int num2 = this.Agreement.AgreementId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int periodId = this.Period.PeriodId;
      num2 = this.Period.PeriodId;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
