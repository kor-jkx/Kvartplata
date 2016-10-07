// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.Scheme
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class Scheme
  {
    public virtual short SchemeId { get; set; }

    public virtual short SchemeType { get; set; }

    public virtual string SchemeName { get; set; }

    public virtual string SchemeNote { get; set; }

    public virtual short Sorter { get; set; }

    public virtual string UName { get; set; }

    public virtual DateTime? DEdit { get; set; }

    public Scheme()
    {
    }

    public Scheme(short id, string name)
    {
      this.SchemeId = id;
      this.SchemeName = name;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Scheme scheme = obj as Scheme;
      return scheme != null && ((int) this.SchemeId == (int) scheme.SchemeId && (int) this.SchemeType == (int) scheme.SchemeType);
    }

    public override int GetHashCode()
    {
      int num1 = 13;
      int schemeId = (int) this.SchemeId;
      short num2 = this.SchemeId;
      int hashCode1 = num2.GetHashCode();
      int num3 = num1 + hashCode1;
      int schemeType = (int) this.SchemeType;
      num2 = this.SchemeType;
      int hashCode2 = num2.GetHashCode();
      return num3 + hashCode2;
    }
  }
}
