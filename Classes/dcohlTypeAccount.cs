// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.dcohlTypeAccount
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Classes
{
  public class dcohlTypeAccount
  {
    public virtual int TypeAccountId { get; set; }

    public virtual string TypeAccountName { get; set; }

    public virtual string Uname { get; set; }

    public virtual DateTime Dedit { get; set; }

    public virtual string TypeAccountStr
    {
      get
      {
        return this.TypeAccountId.ToString();
      }
      set
      {
        this.TypeAccountId = Convert.ToInt32(value);
      }
    }
  }
}
