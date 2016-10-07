// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.ClosedPeriod
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

namespace Kvartplata.Classes
{
  internal class ClosedPeriod
  {
    private int period_id;
    private int complex_id;
    private int company_id;

    public virtual int Period_id
    {
      get
      {
        return this.period_id;
      }
      set
      {
        this.period_id = value;
      }
    }

    public virtual int Complex_id
    {
      get
      {
        return this.complex_id;
      }
      set
      {
        this.complex_id = value;
      }
    }

    public virtual int Company_id
    {
      get
      {
        return this.company_id;
      }
      set
      {
        this.company_id = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      ClosedPeriod closedPeriod = obj as ClosedPeriod;
      return closedPeriod != null && (this.company_id == closedPeriod.company_id && this.complex_id == closedPeriod.complex_id);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int companyId = this.company_id;
      int hashCode1 = this.company_id.GetHashCode();
      int num2 = num1 + hashCode1;
      int complexId = this.complex_id;
      int hashCode2 = this.complex_id.GetHashCode();
      return num2 + hashCode2;
    }
  }
}
