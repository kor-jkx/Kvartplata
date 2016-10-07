// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Agreement
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Agreement
  {
    [Browsable(false)]
    public virtual int AgreementId { get; set; }

    [Browsable(false)]
    public virtual LsClient LsClient { get; set; }

    public virtual string AgreementNum { get; set; }

    [Browsable(false)]
    public virtual short MonthCount { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime? DEnd { get; set; }

    [Browsable(false)]
    public virtual Decimal Dept { get; set; }

    [Browsable(false)]
    public virtual Decimal DeptPeni { get; set; }

    public virtual string Note { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }
  }
}
