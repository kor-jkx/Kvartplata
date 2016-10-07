// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Absence
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using Iesi.Collections;
using System;

namespace Kvartplata.Classes
{
  public class Absence
  {
    private ISet absenceCoeff = (ISet) new HashedSet();

    public virtual short Absence_id { get; set; }

    public virtual string Absence_name { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public virtual ISet AbsenceCoeff
    {
      get
      {
        return this.absenceCoeff;
      }
      set
      {
        this.absenceCoeff = value;
      }
    }
  }
}
