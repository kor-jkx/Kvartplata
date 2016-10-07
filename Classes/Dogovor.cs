// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Dogovor
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.ComponentModel;

namespace Kvartplata.Classes
{
  public class Dogovor
  {
    [Browsable(false)]
    public virtual BaseOrg Manager { get; set; }

    [Browsable(false)]
    public virtual Home Home { get; set; }

    [Browsable(false)]
    public virtual DateTime DBeg { get; set; }

    [Browsable(false)]
    public virtual DateTime DEnd { get; set; }

    public virtual string DogovorNum { get; set; }

    [Browsable(false)]
    public virtual DateTime? DogovorDate { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime DEdit { get; set; }

    public virtual bool IsEdit { get; set; }

    public virtual int OldHashCode { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Dogovor dogovor = obj as Dogovor;
      return dogovor != null && (this.Manager.BaseOrgId == dogovor.Manager.BaseOrgId && this.Home.IdHome == dogovor.Home.IdHome && this.DBeg == dogovor.DBeg);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int idHome = this.Home.IdHome;
      int num2 = this.Home.IdHome;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int baseOrgId = this.Manager.BaseOrgId;
      num2 = this.Manager.BaseOrgId;
      int hashCode2 = num2.GetHashCode();
      int num4 = num3 + hashCode2;
      DateTime dbeg = this.DBeg;
      int hashCode3 = this.DBeg.GetHashCode();
      return num4 + hashCode3;
    }
  }
}
