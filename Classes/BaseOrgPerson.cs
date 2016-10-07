// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.BaseOrgPerson
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class BaseOrgPerson
  {
    [Browsable(false)]
    public virtual int PersonId { get; set; }

    [Browsable(false)]
    public virtual BaseOrg BaseOrg { get; set; }

    public virtual string Family { get; set; }

    public virtual string Name { get; set; }

    public virtual string LastName { get; set; }

    public virtual string PlaceWork { get; set; }

    public virtual string Phone { get; set; }

    public virtual string ICQ { get; set; }

    public virtual string EMail { get; set; }

    public virtual string Note { get; set; }

    public virtual string BasedOn { get; set; }

    [Browsable(false)]
    public virtual YesNo IncRep { get; set; }

    [Browsable(false)]
    public virtual bool IsEdit { get; set; }
  }
}
